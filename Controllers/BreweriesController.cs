﻿using BrewTrack.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

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

        [HttpGet]
        public IActionResult GetBrewDataSet()
        // public async Task<IActionResult> GetBrewDataSet()
        {
            _logger.LogInformation("Get Complete Breweries Data Set");
            
            return Ok();
        }
    }
}
