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

        public async Task<SheetMusic> GetSheetMusicById(int id)
        {
            var sheetMusic = await _context.SheetMusic.FindAsync(id);

            return sheetMusic ?? throw new Exception("Sheet music not found.");
        }

        public async Task<SheetMusic> UploadAsync(SheetMusicUploadDTO upload)
        {
            if (upload.File == null || upload.File.Length == 0)
                throw new ArgumentException("No file uploaded.");

            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsPath);

            var fileName = Path.GetFileName(upload.File.FileName);
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await upload.File.CopyToAsync(stream);
            }

            var relativePdfPath = Path.Combine("uploads", fileName).Replace("\\", "/");

            var sheetMusic = new SheetMusic
            {
                Title = upload.Title,
                Composer = upload.Composer,
                Genre = upload.Genre,
                Instrument = upload.Instrument,
                Difficulty = upload.Difficulty,
                Description = upload.Description,
                IsPublic = upload.IsPublic,
                FilePath = relativePdfPath,
                FileName = fileName,
                UserId = upload.UserId,
            };

            try
            {
                await _context.SheetMusic.AddAsync(sheetMusic);
                await _context.SaveChangesAsync();
                return sheetMusic;
            }
            catch (Exception ex)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                throw new ApplicationException($"Failed to upload sheet music: {ex.Message}", ex);
            }
        }
    }
}
