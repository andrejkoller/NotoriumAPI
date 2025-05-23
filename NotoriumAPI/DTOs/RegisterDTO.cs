using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static NotoriumAPI.Models.User;

namespace NotoriumAPI.DTOs
{
    public class RegisterDTO
    {
        [Required, MinLength(3)]
        public string Name { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(3)]
        public string Username { get; set; } = string.Empty;
        [Required, MinLength(8)]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole Role { get; set; } = UserRole.User;
    }
}
