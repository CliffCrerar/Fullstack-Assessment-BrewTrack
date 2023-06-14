using BrewTrack.Dto;
using BrewTrack.Models;
using BrewTrack.Services;
using Microsoft.AspNetCore.Mvc;

namespace BrewTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("Email/{emailAddress}")]
        public IActionResult GetUserByEmail(string emailAddress)
        {
            _logger.LogInformation("Get User By Email");
            if (!_userService.CheckUserByEmail(emailAddress))
            {
                return NotFound();
            }
            return Ok(_userService.User);
            }

        [HttpGet("UserId/{userId}")]
        public IActionResult GetUserById(Guid userId)
        {
            if (!_userService.CheckUserById(userId))
            {
                return NotFound();
            }
            return Ok(_userService.User);

        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateRequestDto user)
        {
            if (!ModelState.IsValid) return BadRequest();

            if(_userService.CheckUserByEmail(user.EmailAddress))
            {
                return Conflict();
            }

            var createdUser = await _userService.CreateUser(user);

            if (createdUser == null) return new ObjectResult("User Not Created")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            return Created(Request.Path, createdUser);
        }
    }
}
