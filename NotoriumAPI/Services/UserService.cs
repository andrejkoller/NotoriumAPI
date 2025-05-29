using Azure.Core;
using Microsoft.EntityFrameworkCore;
using NotoriumAPI.DTOs;
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
                .Include(u => u.FavoriteSheetMusic)
                .SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            return new UserDTO
            {
                Name = user.Name,
                Email = user.Email,
                Username = user.Username,
                Description = user.Description,
                ProfileImage = user.ProfileImage,
                BackgroundImage = user.BackgroundImage,
                Role = user.Role,
                SheetMusic = user.SheetMusic,
                FavoriteSheetMusic = user.FavoriteSheetMusic
            };
        }

        public async Task<User?> UpdateUserInformationAsync(int id, UserUpdateDTO updateDto)
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
            return user;
        }

        public async Task<User?> UpdateProfileImageAsync(int id, ProfileImageUpdateDTO updateDto)
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
            return user;
        }

        public async Task<User?> UpdateBackgroundImageAsync(int id, BannerImageUpdateDTO updateDto)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Id == id)
                ?? throw new KeyNotFoundException($"User with ID {id} not found.");

            if (updateDto.BackgroundImageFile != null && updateDto.BackgroundImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "banner-images");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{updateDto.BackgroundImageFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updateDto.BackgroundImageFile.CopyToAsync(stream);
                }

                if (!string.IsNullOrWhiteSpace(user.BackgroundImage))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.BackgroundImage);
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                user.BackgroundImage = Path.Combine("banner-images", fileName).Replace("\\", "/");
            }

            context.Users.Update(user);
            await context.SaveChangesAsync();
            return user;
        }
    }
}
