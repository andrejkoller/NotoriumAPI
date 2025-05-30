using NotoriumAPI.DTOs;
using NotoriumAPI.Models;

namespace NotoriumAPI.Mappers
{
    public static class UserMapper
    {
        public static UserDTO ToDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Username = user.Username,
                Description = user.Description,
                ProfileImage = user.ProfileImage,
                BannerImage = user.BannerImage,
                Role = user.Role,
            };
        }
    }
}
