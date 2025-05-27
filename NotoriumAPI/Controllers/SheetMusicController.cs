using Microsoft.AspNetCore.Mvc;
using NotoriumAPI.DTOs;
using NotoriumAPI.Services;

namespace NotoriumAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SheetMusicController : BaseController
    {
        private readonly SheetMusicService _service;
        private readonly PdfThumbnailService _thumbnailService;

        public SheetMusicController(SheetMusicService service, PdfThumbnailService thumbnailService, UserService userService) : base(userService)
        {
            _service = service;
            _thumbnailService = thumbnailService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var sheetMusicList = await _service.GetAllAsync();
                return Ok(sheetMusicList);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var sheetMusic = await _service.GetSheetMusicById(id);
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
            try
            {
                if (CurrentUser == null)
                    return Unauthorized();

                if (upload.File == null || upload.File.Length == 0)
                    return BadRequest(new { message = "No file uploaded." });

                upload.UserId = CurrentUser.Id;
                var sheetMusic = await _service.UploadAsync(upload);
                return Ok(sheetMusic);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
