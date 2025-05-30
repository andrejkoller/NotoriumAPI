using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NotoriumAPI.DTOs;

namespace NotoriumAPI.Models
{
    public class SheetMusic
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Composer { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Genre Genre { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Instrument Instrument { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Difficulty Difficulty { get; set; }

        public string PreviewImage { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public string Description { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = true;
        public int Downloads { get; set; } = 0;

        public int UserId { get; set; }
        public User? User { get; set; }
        public ICollection<User> FavoritedByUsers { get; set; } = [];
    }

    public enum Genre
    {
        Classical,
        Jazz,
        Rock,
        Pop,
        Blues,
        Folk,
        Country,
        Electronic,
        Reggae,
        Metal,
        Soundtrack,
        ModernClassical,
    }

    public enum Instrument
    {
        Piano,
        Guitar,
        Violin,
        Flute,
        Trumpet,
        Drums,
        Saxophone,
        Cello,
        Clarinet,
        Trombone
    }

    public enum Difficulty
    {
        Beginner,
        Intermediate,
        Advanced
    }
}
