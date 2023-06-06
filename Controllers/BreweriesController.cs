using BrewTrack.Dto;
using BrewTrack.Services;
using Microsoft.AspNetCore.Mvc;

namespace BrewTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreweriesController : ControllerBase
    {
        private readonly ILogger<BreweriesController> _logger;
        private readonly IBreweriesService _breweriesService;
        public BreweriesController(ILogger<BreweriesController> logger, IBreweriesService breweriesService)
        {
            _logger = logger;
            _breweriesService = breweriesService;
        }

        [HttpGet("current/{userId}")]
        [ProducesDefaultResponseType(typeof(BreweriesCurrentUserStateDto))]
        public async Task<IActionResult> GetBrewDataPageForUser(Guid userId)
        {
            _logger.LogInformation("Get breweries current data page for user");
            BreweriesCurrentUserStateDto response = await _breweriesService.GetPageDataForUser(userId);
            return Ok(response);
        }

        [HttpGet("next-page/{userId}")]
        [ProducesDefaultResponseType(typeof(BreweriesCurrentUserStateDto))]
        public async Task<IActionResult> GetBrewDataNextPageForUser(Guid userId)
        {
            _logger.LogInformation("Get breweries data next page for user");
            BreweriesCurrentUserStateDto response = await _breweriesService.GetNextPageDataForUser(userId);
            return Ok(response);
        }

        [HttpGet("prev-page/{userId}")]
        [ProducesDefaultResponseType(typeof(BreweriesCurrentUserStateDto))]
        public async Task<IActionResult> GetBrewDataPrevPageForUser(Guid userId)
        {
            _logger.LogInformation("Get breweries data previous page for user");
            BreweriesCurrentUserStateDto response = await _breweriesService.GetPrevPageDataForUser(userId);
            return Ok(response);
        }

        [HttpGet("select-page/{userId}/page-no/{page}")]
        [ProducesDefaultResponseType(typeof(BreweriesCurrentUserStateDto))]
        public async Task<IActionResult> GetBrewDataPrevPageForUser(Guid userId, int pageNo)
        {
            _logger.LogInformation("Get breweries data for selected page");
            BreweriesCurrentUserStateDto response = await _breweriesService.GetDataForPage(userId, pageNo);
            return Ok(response);
        }


    }
}
