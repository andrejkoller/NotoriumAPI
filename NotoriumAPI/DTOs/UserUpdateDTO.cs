using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static NotoriumAPI.Models.User;

namespace NotoriumAPI.DTOs
{
    public class UserUpdateDTO
    {
        [Required, MinLength(3)]
        public string? Name { get; set; }
        [Required, EmailAddress]
        public string? Email { get; set; }
        [Required, MinLength(3)]
        public string? Username { get; set; }
        public string? Description { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole Role { get; set; }
    }
}
