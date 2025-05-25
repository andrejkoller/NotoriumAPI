using Microsoft.AspNetCore.Mvc;
using NotoriumAPI.DTOs;
using NotoriumAPI.Services;

namespace NotoriumAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SheetMusicController : Controller
    {
        private readonly SheetMusicService _service;

        public SheetMusicController(SheetMusicService service)
        {
            _service = service;
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

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] SheetMusicUploadDTO upload)
        {
            try
            {
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
