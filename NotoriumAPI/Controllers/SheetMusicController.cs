using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotoriumAPI.DTOs;
using NotoriumAPI.Services;

namespace NotoriumAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SheetMusicController(SheetMusicService service, UserService userService) : BaseController(userService)
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSheetMusic()
        {
            try
            {
                var sheetMusicList = await service.GetAllSheetMusicAsync();
                return Ok(sheetMusicList);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetCurrentUserSheetMusic(int id)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var sheetMusicList = await service.GetSheetMusicByUserId(id);
                return Ok(sheetMusicList);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var sheetMusic = await service.GetSheetMusicById(id);
                return Ok(sheetMusic);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] SheetMusicUploadDTO upload)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { message = "User not authenticated" });

            try
            {

                if (upload.File == null || upload.File.Length == 0)
                    return BadRequest(new { message = "No file uploaded." });

                upload.UserId = currentUser.Id;
                var sheetMusic = await service.UploadAsync(upload);
                return Ok(sheetMusic);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("bygenre")]
        public async Task<IActionResult> GetByGenre([FromQuery] string genre)
        {
            var result = await service.GetByGenreAsync(genre);
            return Ok(result);
        }

        [HttpGet("bydifficulty")]
        public async Task<IActionResult> GetByDifficulty([FromQuery] string difficulty)
        {
            var result = await service.GetByDifficultyAsync(difficulty);
            return Ok(result);
        }
    }
}
