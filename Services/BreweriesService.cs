using BrewTrack.Contracts.IBrewery;
using BrewTrack.Data;
using BrewTrack.Helpers;
using BrewTrack.Infra;
using BrewTrack.Models;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Text.Json;

namespace BrewTrack.Services
{
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
    public interface IBreweriesService
    {
        Task<IList<BrewPub>> GetData();
    }
    public class BreweriesService : IBreweriesService
    {
        private readonly string _sourceKey;
        private readonly BrewTrackDbContext _dbContext;
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<BreweriesService> _logger;
        private readonly IDatabase _db;
        private DateTime _cacheDate => _getCacheDate();
        public BreweriesService(BrewTrackDbContext dbContext, IConnectionMultiplexer redis, ILogger<BreweriesService> logger)
        {
            _sourceKey = Ensure.ArgumentNotNull( BrewTrackContstants.BrewerySourceKey);
            _dbContext = dbContext;
            _redis = redis;
            _logger = logger;
            _db = _redis.GetDatabase();
        }

        private DateTime _getCacheDate()
        {
            DateTime cacheDate = _dbContext.CachedTimeline.Select(x => x.Date).FirstOrDefault();
            DateTime timeStamp = DateTime.UtcNow;
            bool isNullOrEmptyDate = string.IsNullOrEmpty(cacheDate.ToString());
            if (isNullOrEmptyDate) _updateCacheRegister();
            return isNullOrEmptyDate ? timeStamp : cacheDate;
        }

        private void _getBreweriesData()
        {
            if (!_db.KeyExists("Breweries"))
            {
                var data = Breweries.GetData().Result;
                IDatabase db = _redis.GetDatabase();
                db.StringSet("Breweries", JsonSerializer.Serialize(data));
            }
        }

        private async Task<IList<BrewPub>> _brewPubApi()
        {
            return await Breweries.GetData();
        }

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
                _dbContext.CachedTimeline.Add(new CachedTimeline
                {
                    ApiSourceRefId = breweriesSoureNameId.First(),
                    Date = DateTime.UtcNow
                });
                _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        private void _placeInDataBase(IList<BrewPub> data)
        {
            try
            {
                _dbContext.Brewpubs.ExecuteDelete();
                _dbContext.Brewpubs.AddRange(data);
                _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        private string _serialize<T>(T data) => JsonSerializer.Serialize<T>(data);
        private T _deserialize<T>(string data) => Ensure.ArgumentNotNull(JsonSerializer.Deserialize<T>(data));

        private bool _storeApiDataInCache(IList<BrewPub> breweriesData)
        {
            try
            {
                var dataToString = _serialize<IList<BrewPub>>(breweriesData);
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

        private bool _isBreweriesDataInCache => _db.KeyExists("Breweries");

        public async Task<IList<BrewPub>> GetData()
        {
            _logger.LogInformation("Service BreweriesService, running GetData Method");
            IList<BrewPub> data;
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
                _db.StringSet("Breweries", JsonSerializer.Serialize(data));
                return data;
            }
            var redisData = Ensure.ArgumentNotNull(_db.StringGet("Breweries").ToString());
            return Ensure.ArgumentNotNull(_deserialize<BrewPub[]>(redisData));
        }

        public async Task<DataPage<BrewPub>> GetPagedData(string pageId)
        {
            if(!_db.KeyExists(pageId))
            {
                var data = await GetData();
                var dataBook = DataBook<BrewPub>.Create(data, 10);
                var dataPages = dataBook.GetDataPages();
                foreach(var page in dataPages.Pages)
                {
                    page.PageKeysLookup = dataPages.PageKeysLookup;
                    page.KeysPageLookup = dataPages.KeysPageLookup;
                    string pageToString = _serialize<DataPage<BrewPub>>(page);
                    _db.StringSet(page.PageKey, pageToString);
                }
            }

            RedisValue stringData = Ensure.ArgumentNotNull( _db.StringGet(pageId));
            DataPage<BrewPub> requestedPage = _deserialize<DataPage<BrewPub>>(stringData.ToString());
            return requestedPage;
        }
    }
}
