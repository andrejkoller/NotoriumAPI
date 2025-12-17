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
            try
            {
                var user = await userService.UpdateUserInformationAsync(id, updateDto);

                if (user != null)
                    return Ok(user);

                return NotFound(new { error = "User not found" });
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpPut("{id}/uploadProfileImage")]
        public async Task<IActionResult> UpdateProfilePicture(int id, [FromForm] ProfileImageUpdateDTO updateDto)
        {
            try
            {
                var user = await userService.UpdateProfileImageAsync(id, updateDto);

                if (user != null)
                    return Ok(user);

                return NotFound(new { error = "User not found" });
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpPut("{id}/uploadBannerImage")]
        public async Task<IActionResult> UpdateBackgroundImage(int id, [FromForm] BannerImageUpdateDTO updateDto)
        {
            try
            {
                var user = await userService.UpdateBackgroundImageAsync(id, updateDto);

                if (user != null)
                    return Ok(user);

                return NotFound(new { error = "User not found" });
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
