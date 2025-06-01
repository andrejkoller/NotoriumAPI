using NotoriumAPI.DTOs;
using NotoriumAPI.Models;

namespace NotoriumAPI.Mappers
{
    public static class SheetMusicMapper
    {
        public static SheetMusicDTO ToDTO(SheetMusic sheetMusic)
        {
            return new SheetMusicDTO
            {
                Id = sheetMusic.Id,
                Title = sheetMusic.Title,
                Composer = sheetMusic.Composer,
                Genre = sheetMusic.Genre,
                Instrument = sheetMusic.Instrument,
                Difficulty = sheetMusic.Difficulty,
                PreviewImage = sheetMusic.PreviewImage,
                FileName = sheetMusic.FileName,
                FilePath = sheetMusic.FilePath,
                UploadedAt = sheetMusic.UploadedAt,
                Description = sheetMusic.Description,
                IsPublic = sheetMusic.IsPublic,
                Downloads = sheetMusic.Downloads,
                Favorites = sheetMusic.FavoritedByUsers?.Count ?? 0,
                Views = sheetMusic.Views,
                UserId = sheetMusic.UserId,
                User = sheetMusic.User != null ? UserMapper.ToDTO(sheetMusic.User) : null
            };
        }
    }
}
