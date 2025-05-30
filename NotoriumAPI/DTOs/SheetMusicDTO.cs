using NotoriumAPI.Models;

namespace NotoriumAPI.DTOs
{
    public class SheetMusicDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Composer { get; set; } = string.Empty;
        public Genre Genre { get; set; }
        public Instrument Instrument { get; set; }
        public Difficulty Difficulty { get; set; }
        public string PreviewImage { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsPublic { get; set; }
        public int Downloads { get; set; }
        public int UserId { get; set; }
        public UserDTO? User { get; set; }
    }
}
