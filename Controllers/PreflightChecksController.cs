using BrewTrack.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace BrewTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreflightChecksController : ControllerBase
    {
        private readonly ILogger<PreflightChecksController> _logger;
        private readonly ConnectionMultiplexer _redis;
        private readonly BrewTrackDbContext _dbContext;

        public PreflightChecksController(ILogger<PreflightChecksController> logger, ConnectionMultiplexer redis, BrewTrackDbContext dbContext)
        {
            _logger = logger;
            _redis = redis;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Ping from front end to check if Backend is present.");
            return Ok();
        }

        [HttpPost]
        public IActionResult Post()
        {
            try
            {
                _logger.LogInformation("Ping from backend to check if database is online.");
                int timeStampFromDataBase = _dbContext.Database.ExecuteSqlRaw("select TIMESTAMP");
                if (timeStampFromDataBase > 0)
                {
                    _logger.LogInformation("Database is online.");
                    return Ok();
                }
                else
                {
                    _logger.LogInformation("Database is offline.");
                    throw new Exception("Database is not present");
                }
            }
            catch (Exception ex)
            {
                return _internalError(ex);
            };
        }

        [HttpPatch]
        public IActionResult Patch()
        {
            try
            {
                _logger.LogInformation("Ping from backend to check if cache is online.");
                _redis.GetDatabase();
                if (_redis.IsConnected)
                {
                    return Ok();
                }
                else
                {
                    _logger.LogInformation("Cache is offline.");
                    throw new Exception("Cache is not present");
                };
            } 
            catch (Exception ex)
            {
                return _internalError(ex);
            }
        }

        [NonAction]
        private IActionResult _internalError(Exception ex)
        {
            return new ObjectResult(ex)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
