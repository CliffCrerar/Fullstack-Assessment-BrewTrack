using BrewTrack.Helpers;
using BrewTrack.Infra;
using BrewTrack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrewTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var bp = await Breweries.GetData();

            var dataBook = new DataBook<BrewPub>(bp, 10);

            return Ok(bp);
        }
    }
}
