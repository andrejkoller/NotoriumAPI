using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NotoriumAPI.DTOs;
using NotoriumAPI.Services;

namespace NotoriumAPI.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UserService _userService;

        protected BaseController(UserService userService)
        {
            _userService = userService;
        }

        protected async Task<UserDTO?> GetCurrentUserAsync()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userId, out int id))
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user != null)
                    return user;

                return null;
            }

            return null;
        }
    }
}
