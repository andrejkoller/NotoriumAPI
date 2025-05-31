using Microsoft.EntityFrameworkCore;
using NotoriumAPI.DTOs;
using NotoriumAPI.Mappers;
using NotoriumAPI.Models;

namespace NotoriumAPI.Services
{
    public class SheetMusicService(NotoriumDbContext context, PdfThumbnailService pdfThumbnailService, IWebHostEnvironment env)
    {
        public async Task<List<SheetMusicDTO>> GetAllSheetMusicAsync()
        {
            var sheetMusic = await context.SheetMusic
                .Where(sm => sm.IsPublic)
                .OrderByDescending(sm => sm.UploadedAt)
                .Include(sm => sm.User)
                .ToListAsync();

            return [.. sheetMusic.Select(SheetMusicMapper.ToDTO)];
        }

        public async Task<List<SheetMusicDTO>> GetSheetMusicByUserId(int userId)
        {
            var user = await context.Users
                .Include(u => u.SheetMusic)
                .SingleOrDefaultAsync(u => u.Id == userId);

            return user == null
                ? throw new Exception("User with sheet music not found.")
                : [.. user.SheetMusic.Where(sm => sm.IsPublic).Select(SheetMusicMapper.ToDTO)];
        }

        public async Task<SheetMusicDTO> GetSheetMusicById(int id)
        {
            var sheetMusic = await context.SheetMusic.FindAsync(id);
            return SheetMusicMapper.ToDTO(sheetMusic ?? throw new Exception("Sheet music not found."));
        }

        public async Task<SheetMusicDTO> UploadAsync(SheetMusicUploadDTO upload)
        {
            if (upload.File == null || upload.File.Length == 0)
                throw new ArgumentException("No file uploaded.");

            var uploadsPath = Path.Combine(env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsPath);

            var fileName = Path.GetFileName(upload.File.FileName);
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await upload.File.CopyToAsync(stream);
            }

            var relativePdfPath = Path.Combine("uploads", fileName).Replace("\\", "/");
            var thumbnailPath = pdfThumbnailService.GenerateThumbnail(filePath);
            var relativeThumbnailPath = thumbnailPath.Replace(env.WebRootPath, "").Replace("\\", "/");

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
                await context.SheetMusic.AddAsync(sheetMusic);
                await context.SaveChangesAsync();
                return SheetMusicMapper.ToDTO(sheetMusic);
            }
            catch (Exception ex)
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                throw new ApplicationException($"Failed to upload sheet music: {ex.Message}", ex);
            }
        }

        public async Task<List<SheetMusicDTO>> GetByGenreAsync(string genre)
        {
            if (!Enum.TryParse<Genre>(genre, true, out var genreEnum))
                throw new ArgumentException("Invalid genre");

            var sheetMusic = await context.SheetMusic
                .Where(sm => sm.Genre == genreEnum && sm.IsPublic)
                .OrderByDescending(sm => sm.UploadedAt)
                .Include(sm => sm.User)
                .ToListAsync();

            return [.. sheetMusic.Select(SheetMusicMapper.ToDTO)];
        }

        public async Task<List<SheetMusicDTO>> GetByDifficultyAsync(string difficulty)
        {
            if (!Enum.TryParse<Difficulty>(difficulty, true, out var difficultyEnum))
                throw new ArgumentException("Invalid difficulty");

            var sheetMusic = await context.SheetMusic
                .Where(sm => sm.Difficulty == difficultyEnum && sm.IsPublic)
                .OrderByDescending(sm => sm.UploadedAt)
                .Include(sm => sm.User)
                .ToListAsync();

            return [.. sheetMusic.Select(SheetMusicMapper.ToDTO)];
        }

        public async Task<List<SheetMusicDTO>> GetByInstrumentAsync(string instrument)
        {
            if (!Enum.TryParse<Instrument>(instrument, true, out var instrumentEnum))
                throw new ArgumentException("Invalid instrument");

            var sheetMusic = await context.SheetMusic
                .Where(sm => sm.Instrument == instrumentEnum && sm.IsPublic)
                .OrderByDescending(sm => sm.UploadedAt)
                .Include(sm => sm.User)
                .ToListAsync();

            return [.. sheetMusic.Select(SheetMusicMapper.ToDTO)];
        }

        public async Task<List<SheetMusicDTO>> GetByUploadDateAsync(bool isOrderByDescending)
        {
            var sheetMusic = await context.SheetMusic
                .Where(sm => sm.IsPublic)
                .Include(sm => sm.User)
                .ToListAsync();

            return isOrderByDescending
                ? [.. sheetMusic.OrderByDescending(sm => sm.UploadedAt).Select(SheetMusicMapper.ToDTO)]
                : [.. sheetMusic.OrderBy(sm => sm.UploadedAt).Select(SheetMusicMapper.ToDTO)];
        }
    }
}
