using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NotoriumAPI.Models;
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

        protected async Task<User?> GetCurrentUserAsync()
        {
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userId, out int id))
                    return null;

                return await _userService.GetUserByIdAsync(id);
            }
        }
    }
}
