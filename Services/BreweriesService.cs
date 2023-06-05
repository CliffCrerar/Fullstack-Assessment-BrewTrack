using BrewTrack.Contracts.IBrewery;
using BrewTrack.Data;
using BrewTrack.Dto;
using BrewTrack.Helpers;
using BrewTrack.Infra;
using BrewTrack.Models;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Text.Json;


namespace BrewTrack.Services
{
    /// <summary>
    /// BrewTrack breweries dependancjy injection service container extension
    /// </summary>
    public static class BreweriesServiceExtension
    {
        public static IServiceCollection AddBreweriesService(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IBreweriesService, BreweriesService>(provider =>
            {
                BrewTrackDbContext context = Ensure.ArgumentNotNull(provider.GetService<BrewTrackDbContext>());
                IConnectionMultiplexer redis = Ensure.ArgumentNotNull(provider.GetService<IConnectionMultiplexer>());
                ILogger<BreweriesService> logger = Ensure.ArgumentNotNull(provider.GetService<ILogger<BreweriesService>>());
                return new BreweriesService(context, redis, logger);
            });
            return services;
        }
    }
    /// <summary>
    /// Breweries service contract
    /// </summary>
    public interface IBreweriesService
    {
        Task<IList<BrewPub>> GetData();
        Task<IList<BrewPub>> GetPageDataForUser(Guid userId);
        Task<IList<BrewPub>> GetNextPageDataForUser(Guid userId);
        Task<IList<BrewPub>> GetPrevPageDataForUser(Guid userId);
        Task<BreweriesMetaDto> GetBreweriesMeta();
        Task<int> RetrieveLastPageFromUser(Guid userId);
    }
    /// <summary>
    /// Breweries service concrete class
    /// </summary>
    public class BreweriesService : IBreweriesService
    {
        private readonly int _recordsPerPage;
        private readonly string _sourceKey;
        private readonly string _sourceKeyMeta;
        private readonly BrewTrackDbContext _dbContext;
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<BreweriesService> _logger;
        private readonly IDatabase _db;
        private DateTime _cacheDate => _getCacheDate();
        public BreweriesService(BrewTrackDbContext dbContext, IConnectionMultiplexer redis, ILogger<BreweriesService> logger)
        {
            _sourceKey = Ensure.ArgumentNotNull( BrewTrackContstants.BrewerySourceKey);
            _sourceKeyMeta = Ensure.ArgumentNotNull(BrewTrackContstants.BrewerySourceKey) + "Meta";
            _dbContext = dbContext;
            _redis = redis;
            _logger = logger;
            _recordsPerPage = 10;
            _db = _redis.GetDatabase();
        }
        /// <summary>
        /// Get the last date data was cached in the database or if not data is cashed update the cache register
        /// </summary>
        /// <returns></returns>
        private DateTime _getCacheDate()
        {
            try
            {
                DateTime cacheDate = _dbContext.CachedTimeline.Select(x => x.Date).FirstOrDefault();
                bool isNullOrEmptyDate = string.IsNullOrEmpty(cacheDate.ToString());
                if(isNullOrEmptyDate)
                {
                    throw new ArgumentNullException("No cachedate in database");
                }
                return cacheDate;
            } 
                catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return DateTime.MinValue;
            }
        }
        /// <summary>
        /// Not used
        /// </summary>
        private void _getBreweriesData()
        {
            if (!_db.KeyExists("Breweries"))
            {
                var data = Breweries.GetData().Result;
                IDatabase db = _redis.GetDatabase();
                db.StringSet("Breweries", JsonSerializer.Serialize(data));
            }
        }
        /// <summary>
        /// Get data from third party api integration
        /// </summary>
        /// <returns>IList<BrewPub></returns>
        private async Task<IList<BrewPub>> _brewPubApi()
        {
            return await Breweries.GetData();
        }
        /// <summary>
        /// Runs a check to test of the cache is stale
        /// </summary>
        /// <returns>bool</returns>
        private bool _isCacheStale()
        {
            return _cacheDate.AddHours(1) < DateTime.UtcNow;
        }

        private void _updateCacheRegister()
        {
            try
            {
                var breweriesSoureNameId = from brewery in _dbContext.ApiSources
                                           where brewery.ApiSourceName == BrewTrackContstants.BrewerySourceKey
                                           select brewery.Id;
                var record = new CachedTimeline
                {
                    ApiSourceRefId = breweriesSoureNameId.First(),
                    Date = DateTime.UtcNow
                };
                _dbContext.CachedTimeline.Add(record);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
        /// <summary>
        /// Ayncrounously updates the database with the latest data retreived from the api
        /// </summary>
        /// <param name="data"></param>
        private void _placeInDataBase(IList<BrewPub> data)
        {
            try
            {
                _dbContext.Brewpubs.ExecuteDelete();
                _dbContext.Brewpubs.AddRange(data);
                _dbContext.SaveChanges();
                _updateCacheRegister();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
        /// <summary>
        /// Serialization short hand method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private string _serialize<T>(T data) => JsonSerializer.Serialize<T>(data);
        /// <summary>
        /// Deserialization short hand method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private T _deserialize<T>(string data) => Ensure.ArgumentNotNull(JsonSerializer.Deserialize<T>(data));
        /// <summary>
        /// Store api data in redis cahce
        /// </summary>
        /// <param name="breweriesData"></param>
        /// <returns></returns>
        private bool _storeApiDataInCache(IList<BrewPub> breweriesData)
        {
            try
            {
                var dataToString = _serialize(breweriesData);
                _db.StringSet(_sourceKey, dataToString);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }

        private IList<BrewPub> _retreiveApiDataFromCache()
        {
            RedisValue breweryStringData = Ensure.ArgumentNotNull( _db.StringGet(_sourceKey) );
            return _deserialize<IList<BrewPub>>(breweryStringData.ToString());
        }

        private bool _isBreweriesDataInCache => _db.KeyExists(BrewTrackContstants.BreweryApiResource);

        private void _updateUserPage(int pageNo, Guid userId)
        {
            var userHistRow = new UserHistory
            {
                LastPage = pageNo,
                UserId = userId
            };
            _dbContext.UserHistory.Add(userHistRow);
            _dbContext.SaveChanges(_isBreweriesDataInCache);
        }

        private async Task<int> _getLastPageForUser(Guid userId)
        {
            try
            {
                return await _dbContext.UserHistory
                    .Where(row => row.UserId == userId)
                    .OrderByDescending(row => row.Id)
                    .Select(row => row.LastPage)
                    .LastAsync<int>();
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                _logger.LogInformation("No Record for user");
                return 0;
            }

        }

        private async Task<IList<BrewPub>> _retreivePage(int page)
        {
            string pageKey = BrewTrackContstants.BrewerySourceKey + page.ToString();
            if (_db.KeyExists(pageKey))
            {
                var pageDataString = _db.StringGet(pageKey);
                return _deserialize<IList<BrewPub>>(Ensure.ArgumentNotNull(pageDataString));
            }
            var pageDataFromApi = await Breweries.GetPagedData(page.ToString(), _recordsPerPage.ToString());
            _db.StringSet(pageKey, _serialize(pageDataFromApi));
            return pageDataFromApi;
        }

        public async Task<IList<BrewPub>> GetData()
        {
            _logger.LogInformation("Service BreweriesService, running GetData Method");
            IList<BrewPub> data;
            if(_isCacheStale())
            {
                data = await _brewPubApi();
                _placeInDataBase(data);
                _storeApiDataInCache(data);
                return data;
            }
            if (!_isBreweriesDataInCache)
            {
                _logger.LogInformation("Data is not in Redis Database");
                data = _dbContext.Brewpubs.ToList();
                if(data.Count == 0) 
                {
                    _logger.LogInformation("Data is not in MySql Database");
                    data = await _brewPubApi();
                    _placeInDataBase(data);
                }
                _storeApiDataInCache(data);
                return data;
            }
            return _retreiveApiDataFromCache();
        }

        public async Task<IList<BrewPub>> GetNextPageDataForUser(Guid userId)
        {
            return await _getPagedData(userId, true);
        }
        public async Task<IList<BrewPub>> GetPrevPageDataForUser(Guid userId)
        {
            return await _getPagedData(userId, false);
        }
        public async Task<IList<BrewPub>> GetPageDataForUser(Guid userId)
        {
            return await _getPagedData(userId);
        }
        private async Task<IList<BrewPub>> _getPagedData(Guid userId)
        {
            var lastPage = await _getLastPageForUser(userId);
            if (lastPage == 0)
            {
                lastPage = 1;
            }
            return await _retreivePage(lastPage);
        }
        private async Task<IList<BrewPub>> _getPagedData(Guid userId, bool nextPage)
        {
            var lastPage = await _getLastPageForUser(userId);
            if(lastPage == 0)
            {
                lastPage = 1;
            }
            else if(nextPage)
            {
                lastPage++;
            } else
            {
                lastPage = lastPage == 1 ? 1 : lastPage--;
            }
            _updateUserPage(lastPage, userId);
            return await _retreivePage(lastPage);
        }


        public async Task<BreweriesMetaDto> GetBreweriesMeta()
        {
            BreweriesMetaDto data;
            if (_db.KeyExists(_sourceKeyMeta))
            {
                var stringData = _db.StringGet(_sourceKeyMeta);
                data = _deserialize<BreweriesMetaDto>(Ensure.ArgumentNotNull(stringData));
            } else
            {
                data = await Breweries.GetBreweriesMeta();
                var stringData = _serialize(data);
                _db.StringSet(_sourceKeyMeta, stringData);
            }
            return data;
        }

        public async Task<int> RetrieveLastPageFromUser(Guid userId)
        {
            return await _getLastPageForUser(userId);
        }


        //public async Task<DataPage<BrewPub>> GetPagedData(string pageId)
        //{
        //    if(!_db.KeyExists(pageId))
        //    {
        //        var data = await GetData();
        //        var dataBook = DataBook<BrewPub>.Create(data, 10);
        //        var dataPages = dataBook.GetDataPages();
        //        foreach(var page in dataPages.Pages)
        //        {
        //            page.PageKeysLookup = dataPages.PageKeysLookup;
        //            page.KeysPageLookup = dataPages.KeysPageLookup;
        //            string pageToString = _serialize<DataPage<BrewPub>>(page);
        //            _db.StringSet(page.PageKey, pageToString);
        //        }
        //    }

        //    RedisValue stringData = Ensure.ArgumentNotNull( _db.StringGet(pageId));
        //    DataPage<BrewPub> requestedPage = _deserialize<DataPage<BrewPub>>(stringData.ToString());
        //    return requestedPage;
        //}
    }
}
