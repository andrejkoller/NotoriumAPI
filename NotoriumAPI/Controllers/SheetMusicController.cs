﻿using Microsoft.AspNetCore.Authorization;
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                await service.DeleteAsync(id);
                return Ok(new { message = "Sheet music deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("bygenre")]
        public async Task<IActionResult> GetByGenre([FromQuery] string genre)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var result = await service.GetByGenreAsync(genre);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("bydifficulty")]
        public async Task<IActionResult> GetByDifficulty([FromQuery] string difficulty)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var result = await service.GetByDifficultyAsync(difficulty);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("byinstrument")]
        public async Task<IActionResult> GetByInstrument([FromQuery] string instrument)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var result = await service.GetByInstrumentAsync(instrument);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("byuploaddate")]
        public async Task<IActionResult> GetByUploadDate([FromQuery] bool isOrderByDescending)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var result = await service.GetByUploadDateAsync(isOrderByDescending);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var result = await service.SearchAsync(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/favorites")]
        public async Task<IActionResult> GetFavoriteSheetMusic(int id)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { message = "User not authenticated" });

            if (currentUser.Id != id)
                return Forbid("You can only access your own favorite sheet music.");

            try
            {
                var result = await service.GetFavoriteSheetMusicAsync(currentUser.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("favorite/{id}")]
        public async Task<IActionResult> AddToFavorites(int id)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var result = await service.AddToFavoritesAsync(id, currentUser.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("favorite/{id}")]
        public async Task<IActionResult> RemoveFromFavorites(int id)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var result = await service.RemoveFromFavoritesAsync(id, currentUser.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadSheetMusic(int id)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var filePath = await service.GetSheetMusicFilePathAsync(id);

                if (string.IsNullOrEmpty(filePath))
                    return NotFound(new { message = "Sheet music file not found." });

                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var fileName = Path.GetFileName(filePath);

                return File(fileStream, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/print")]
        public async Task<IActionResult> PrintSheetMusic(int id)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var filePath = await service.GetSheetMusicFilePathAsync(id);

                if (string.IsNullOrEmpty(filePath))
                    return NotFound(new { message = "Sheet music file not found." });

                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var fileName = Path.GetFileName(filePath);

                return File(fileStream, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
