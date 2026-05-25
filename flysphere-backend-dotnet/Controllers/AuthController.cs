using FlysphereBackendDotnet.Models;
using FlysphereBackendDotnet.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] User user)
        {
            var createdUser = await _authService.RegisterAsync(user);
            return Ok(createdUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var loggedInUser = await _authService.LoginAsync(request.Email, request.Password);

            if (loggedInUser == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(loggedInUser);
        }
    }
}
