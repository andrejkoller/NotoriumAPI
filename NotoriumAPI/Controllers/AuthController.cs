using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotoriumAPI.DTOs;
using NotoriumAPI.Services;

namespace NotoriumAPI.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(AuthService authService) : Controller
    {
        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            try
            {
                var user = await authService.Register(request);
                return Ok(user);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            var result = await authService.Login(request);

            if (result != null)
                return Ok(new
                {
                    result.Value.token,
                    result.Value.user
                });

            return Unauthorized(new { error = "Invalid username or password" });
        }
    }
}