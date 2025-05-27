using System.ComponentModel.DataAnnotations;

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
        public string? Role { get; set; }
    }
}
