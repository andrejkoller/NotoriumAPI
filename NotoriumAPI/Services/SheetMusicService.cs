using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotoriumAPI.DTOs;
using NotoriumAPI.Models;

namespace NotoriumAPI.Services
{
    public class SheetMusicService
    {
        private readonly NotoriumDbContext _context;
        private readonly PdfThumbnailService _pdfThumbnailService;
        private readonly IWebHostEnvironment _env;

        public SheetMusicService(NotoriumDbContext context, PdfThumbnailService pdfThumbnailService, IWebHostEnvironment env)
        {
            _context = context;
            _pdfThumbnailService = pdfThumbnailService;
            _env = env;
        }

        public async Task<List<SheetMusic>> GetAllSheetMusicAsync()
        {
            return await _context.SheetMusic
                .Where(sm => sm.IsPublic)
                .OrderByDescending(sm => sm.UploadedAt)
                .Include(sm => sm.User)
                .ToListAsync();
        }

        public async Task<List<SheetMusic>> GetSheetMusicByUserId(int userId)
        {
            return await _context.SheetMusic
                .Where(sm => sm.UserId == userId)
                .OrderByDescending(sm => sm.UploadedAt)
                .Include(sm => sm.User)
                .ToListAsync();
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
            var thumbnailPath = _pdfThumbnailService.GenerateThumbnail(filePath);
            var relativeThumbnailPath = thumbnailPath.Replace(_env.WebRootPath, "").Replace("\\", "/");

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
                PreviewImage = relativeThumbnailPath
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
                    File.Delete(filePath);

                throw new ApplicationException($"Failed to upload sheet music: {ex.Message}", ex);
            }
        }

        public async Task<List<SheetMusic>> GetByGenreAsync(string genre)
        {
            if (!Enum.TryParse<Genre>(genre, true, out var genreEnum))
                throw new ArgumentException("Invalid genre");

            return await _context.SheetMusic
                .Where(sm => sm.Genre == genreEnum && sm.IsPublic)
                .OrderByDescending(sm => sm.UploadedAt)
                .Include(sm => sm.User)
                .ToListAsync();
        }
    }
}
