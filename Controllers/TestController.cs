using BrewTrack.Helpers;
using BrewTrack.Infra;
using BrewTrack.Models;
using BrewTrack.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrewTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IBreweriesService _breweriesService;
        public TestController(IBreweriesService breweriesService)
        {
            _breweriesService = breweriesService;
        }
        [HttpGet("breweriesdatafromapi")]
        public async Task<IActionResult> Get()
        {
            var bp = await Breweries.GetData();

            var dataBook = new DataBook<BrewPub>(bp, 10);

            return Ok(bp);
        }

        [HttpGet("brewerydatafromservice")]
        public async Task<IActionResult> GetBrewData()
        {
            var data = await _breweriesService.GetData();
            return Ok(data);
        }
    }
}
