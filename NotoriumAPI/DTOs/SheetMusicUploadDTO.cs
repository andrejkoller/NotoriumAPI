using NotoriumAPI.Models;

namespace NotoriumAPI.DTOs
{
    public class SheetMusicUploadDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Composer { get; set; } = string.Empty;
        public Genre Genre { get; set; }
        public Instrument Instrument { get; set; }
        public Difficulty Difficulty { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = true;
        public string PreviewImage { get; set; } = string.Empty;
        public int UserId { get; set; }

        public IFormFile File { get; set; } = null!;
    }
}
