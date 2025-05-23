using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NotoriumAPI.Models;
using NotoriumAPI.Services;

namespace NotoriumAPI.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UserService _userService;

        protected User? CurrentUser
        {
            get
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userId, out int id))
                {
                    return null;
                }

                return _userService.GetUserById(id);
            }
        }

        protected BaseController(UserService userService)
        {
            _userService = userService;
        }
    }
}
