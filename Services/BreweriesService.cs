using BrewTrack.Data;
using BrewTrack.Dto;
using BrewTrack.Helpers;
using BrewTrack.Infra;
using BrewTrack.Models;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
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
        Task<BreweriesCurrentUserStateDto> GetPageDataForUser(Guid userId);
        Task<BreweriesCurrentUserStateDto> GetNextPageDataForUser(Guid userId);
        Task<BreweriesCurrentUserStateDto> GetPrevPageDataForUser(Guid userId);
        Task<BreweriesCurrentUserStateDto> GetDataForPage(Guid userId, int pageNo);
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
            _recordsPerPage = BrewTrackContstants.RecordsPerPage;
            _db = _redis.GetDatabase();
        }
        // get cache date
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
        // get data from external api
        private async Task<IList<BrewPub>> _brewPubApi()
        {
            return await Breweries.GetData();
        }
        // check if cache is stale
        private bool _isCacheStale()
        {
            return _cacheDate.AddHours(1) < DateTime.UtcNow;
        }
        // serialize shorthand
        private string _serialize<T>(T data) => JsonSerializer.Serialize<T>(data);
        // deserialize shorthand
        private T _deserialize<T>(string data) => Ensure.ArgumentNotNull(JsonSerializer.Deserialize<T>(data));

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

        private async Task _updateUserPage(int pageNo, Guid userId)
        {
            try
            {
                var userHistRow = new UserHistory
                {
                    LastPage = pageNo,
                    UserId = userId
                };
                _dbContext.UserHistory.Add(userHistRow);
                await _dbContext.SaveChangesAsync(_isBreweriesDataInCache);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

        }

        private async Task<int> _getPageForUser(Guid userId)
        {
            try
            {
                return await _dbContext.UserHistory
                    .Where(row => row.UserId == userId)
                    .OrderBy(row => row.Id)
                    .Select(row => row.LastPage)
                    .LastAsync<int>();
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                _logger.LogInformation("No Record for user");
                return 1;
            }
        }

        private async Task<IList<BrewPub>> _retreivePageData(int page)
        {
            try
            {
                string pageKey = BrewTrackContstants.BrewerySourceKey + "_page_" + page.ToString();
                if (_db.KeyExists(pageKey))
                {
                    var pageDataString = _db.StringGet(pageKey);
                    return _deserialize<IList<BrewPub>>(Ensure.ArgumentNotNull(pageDataString));
                }
                var pageDataFromApi = await Breweries.GetPagedData(page.ToString(), _recordsPerPage.ToString());
                _db.StringSet(pageKey, _serialize(pageDataFromApi));
                return pageDataFromApi;
            } 
            catch(Exception ex)
            {
                _logger.LogInformation("Error occured while _retreivePageData.");
                _logger.LogError(ex.Message, ex);
                throw;
            }

        }

        private async Task<BreweriesCurrentUserStateDto> _createPageResponse(int page)
        {
            try
            {
                return new BreweriesCurrentUserStateDto
                {
                    OfPages = (await _getBreweriesMeta()).Total / _recordsPerPage,
                    PageData = await _retreivePageData(page),
                    PageNo = page
                };
            } catch(Exception ex)
            {
                _logger.LogInformation("Error occured while creating user page data dto.");
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task<BreweriesMetaDto> _getBreweriesMeta()
        {
            BreweriesMetaDto data;
            if (_db.KeyExists(_sourceKeyMeta))
            {
                var stringData = _db.StringGet(_sourceKeyMeta);
                data = _deserialize<BreweriesMetaDto>(Ensure.ArgumentNotNull(stringData));
            }
            else
            {
                data = await Breweries.GetBreweriesMeta();
                var stringData = _serialize(data);
                _db.StringSet(_sourceKeyMeta, stringData);
            }
            return data;
        }

        public async Task<BreweriesCurrentUserStateDto> GetNextPageDataForUser(Guid userId)
        {
            int currentPage = await _getPageForUser(userId);
            await _updateUserPage(++currentPage, userId);
            return await _createPageResponse(currentPage);
        }
        public async Task<BreweriesCurrentUserStateDto> GetPrevPageDataForUser(Guid userId)
        {
            int currentPage = await _getPageForUser(userId);
            await _updateUserPage(--currentPage == 0 ? 1 : currentPage, userId);
            return await _createPageResponse(currentPage);
        }
        public async Task<BreweriesCurrentUserStateDto> GetPageDataForUser(Guid userId)
        {
            int currentPage = await _getPageForUser(userId);
            return await _createPageResponse(currentPage);
        }

        public async Task<BreweriesCurrentUserStateDto> GetDataForPage(Guid userId, int pageNo)
        {
            await _updateUserPage(pageNo, userId);
            return await _createPageResponse(pageNo);
        }

        public async Task<int> RetrieveLastPageFromUser(Guid userId)
        {
            return await _getPageForUser(userId);
        }
    }
}
