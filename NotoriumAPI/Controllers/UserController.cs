using Microsoft.AspNetCore.Mvc;
using NotoriumAPI.DTOs;
using NotoriumAPI.Services;

namespace NotoriumAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private new readonly UserService _userService;

        public UserController(UserService userService) : base(userService)
        {
            _userService = userService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            if (CurrentUser == null)
                return Unauthorized(new { error = "User not authenticated" });

            var users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpGet("currentUser")]
        public IActionResult GetCurrentUser()
        {
            if (CurrentUser == null)
                return Unauthorized(new { error = "User not authenticated" });

            var user = _userService.GetUserById(CurrentUser.Id);

            return Ok(user);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(UserUpdateDTO updateDto)
        {
            if (CurrentUser == null)
                return Unauthorized(new { error = "User not authenticated" });

            try
            {
                var user = await _userService.UpdateUserAsync(CurrentUser.Id, updateDto);

                if (user == null)
                    return NotFound(new { error = "User not found" });

                return Ok(user);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
