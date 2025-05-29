using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotoriumAPI.DTOs;
using NotoriumAPI.Services;

namespace NotoriumAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(UserService userService) : BaseController(userService)
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { error = "User not authenticated" });

            var users = await userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("currentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { error = "User not authenticated" });

            var user = await userService.GetUserByIdAsync(currentUser.Id);
            return Ok(user);
        }

        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdateUserInformation(int id, UserUpdateDTO updateDto)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { error = "User not authenticated" });

            try
            {
                var user = await userService.UpdateUserInformationAsync(id, updateDto);

                if (user == null)
                    return NotFound(new { error = "User not found" });

                return Ok(user);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpPut("{id}/uploadProfileImage")]
        public async Task<IActionResult> UpdateProfilePicture(int id, [FromForm] ProfileImageUpdateDTO updateDto)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { error = "User not authenticated" });

            try
            {
                var user = await userService.UpdateProfileImageAsync(id, updateDto);

                if (user == null)
                    return NotFound(new { error = "User not found" });

                return Ok(user);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpPut("{id}/updateBackgroundImage")]
        public async Task<IActionResult> UpdateBackgroundImage(int id, [FromForm] BannerImageUpdateDTO updateDto)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { error = "User not authenticated" });

            try
            {
                var user = await userService.UpdateBackgroundImageAsync(id, updateDto);
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
