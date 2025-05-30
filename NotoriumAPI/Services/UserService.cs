using Microsoft.EntityFrameworkCore;
using NotoriumAPI.DTOs;
using NotoriumAPI.Mappers;
using NotoriumAPI.Models;

namespace NotoriumAPI.Services
{
    public class UserService(NotoriumDbContext context)
    {
        public async Task<bool> EmailExists(string email)
        {
            return await context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await context.Users
                .Include(u => u.SheetMusic)
                .SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            return UserMapper.ToDTO(user);
        }

        public async Task<UserDTO?> UpdateUserInformationAsync(int id, UserUpdateDTO updateDto)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Id == id)
                ?? throw new KeyNotFoundException($"User with ID {id} not found.");

            if (!string.IsNullOrWhiteSpace(updateDto.Name))
                user.Name = updateDto.Name;

            if (!string.IsNullOrWhiteSpace(updateDto.Email))
                user.Email = updateDto.Email;

            if (!string.IsNullOrWhiteSpace(updateDto.Username))
                user.Username = updateDto.Username;

            if (!string.IsNullOrWhiteSpace(updateDto.Description))
                user.Description = updateDto.Description;

            user.Role = updateDto.Role;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            return UserMapper.ToDTO(user);
        }

        public async Task<UserDTO?> UpdateProfileImageAsync(int id, ProfileImageUpdateDTO updateDto)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Id == id)
                ?? throw new KeyNotFoundException($"User with ID {id} not found.");

            if (updateDto.ProfileImageFile != null && updateDto.ProfileImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profile-images");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{updateDto.ProfileImageFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updateDto.ProfileImageFile.CopyToAsync(stream);
                }

                if (!string.IsNullOrWhiteSpace(user.ProfileImage))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfileImage);
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                user.ProfileImage = Path.Combine("profile-images", fileName).Replace("\\", "/");
            }

            context.Users.Update(user);
            await context.SaveChangesAsync();

            return UserMapper.ToDTO(user);
        }

        public async Task<UserDTO?> UpdateBackgroundImageAsync(int id, BannerImageUpdateDTO updateDto)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Id == id)
                ?? throw new KeyNotFoundException($"User with ID {id} not found.");

            if (updateDto.BannerImageFile != null && updateDto.BannerImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "banner-images");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{updateDto.BannerImageFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updateDto.BannerImageFile.CopyToAsync(stream);
                }

                if (!string.IsNullOrWhiteSpace(user.BannerImage))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.BannerImage);
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                user.BannerImage = Path.Combine("banner-images", fileName).Replace("\\", "/");
            }

            context.Users.Update(user);
            await context.SaveChangesAsync();

            return UserMapper.ToDTO(user);
        }
    }
}
