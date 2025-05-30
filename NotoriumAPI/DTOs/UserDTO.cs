using NotoriumAPI.Models;
using static NotoriumAPI.Models.User;
using System.Text.Json.Serialization;

namespace NotoriumAPI.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? ProfileImage { get; set; } = string.Empty;
        public string? BannerImage { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole Role { get; set; } = UserRole.User;
    }
}
