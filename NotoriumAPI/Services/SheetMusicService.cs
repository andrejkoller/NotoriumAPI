using Microsoft.EntityFrameworkCore;
using NotoriumAPI.DTOs;
using NotoriumAPI.Models;

namespace NotoriumAPI.Services
{
    public class SheetMusicService
    {
        private readonly NotoriumDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SheetMusicService(NotoriumDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<List<SheetMusic>> GetAllAsync()
        {
            return await _context.SheetMusic.ToListAsync();
        }

        public async Task<SheetMusic> UploadAsync(SheetMusicUploadDTO upload)
        {
            if (upload == null || upload.File.Length == 0)
                throw new Exception("No file uploaded.");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = Path.GetFileName(upload.File.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await upload.File.CopyToAsync(stream);
            }

            var sheet = new SheetMusic
            {
                Title = upload.Title,
                Composer = upload.Composer,
                Genre = upload.Genre,
                Instrument = upload.Instrument,
                Difficulty = upload.Difficulty,
                Description = upload.Description,
                IsPublic = upload.IsPublic,
                FileName = fileName,
                FilePath = $"/uploads/{fileName}",
                UploadedAt = DateTime.UtcNow,
                Downloads = 0
            };

            _context.SheetMusic.Add(sheet);
            await _context.SaveChangesAsync();

            return sheet;
        }
    }
}
