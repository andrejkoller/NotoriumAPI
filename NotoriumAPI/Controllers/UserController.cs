using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("currentUser")]
        public IActionResult GetCurrentUser()
        {
            if (CurrentUser == null)
            {
                return Unauthorized(new { error = "User not authenticated" });
            }

            var user = _userService.GetUserById(CurrentUser.Id);

            return Ok(user);
        }
    }
}
