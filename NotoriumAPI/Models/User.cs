using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NotoriumAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole Role { get; set; } = UserRole.User;

        public ICollection<SheetMusic> SheetMusics { get; set; } = [];

        public enum UserRole
        {
            Admin,
            User
        }
    }
}
