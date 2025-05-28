using Azure.Core;
using Microsoft.EntityFrameworkCore;
using NotoriumAPI.DTOs;
using NotoriumAPI.Models;

namespace NotoriumAPI.Services
{
    public class UserService
    {
        private readonly NotoriumDbContext _context;

        public UserService(NotoriumDbContext context)
        {
            _context = context;
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public User? GetUserById(int id)
        {
            return _context.Users.SingleOrDefault(u => u.Id == id);
        }

        public async Task<User?> UpdateUserAsync(int id, UserUpdateDTO updateDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id)
                ?? throw new KeyNotFoundException($"User with ID {id} not found.");

            if (!string.IsNullOrWhiteSpace(updateDto.Name))
                user.Name = updateDto.Name;

            if (!string.IsNullOrWhiteSpace(updateDto.Email))
                user.Email = updateDto.Email;

            if (!string.IsNullOrWhiteSpace(updateDto.Username))
                user.Username = updateDto.Username;

            if (!string.IsNullOrWhiteSpace(updateDto.Description))
                user.Description = updateDto.Description;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateProfilePicture(int id, ProfilePictureUpdateDTO updateDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id)
                ?? throw new KeyNotFoundException($"User with ID {id} not found.");

            if (updateDto.ProfileImageFile != null && updateDto.ProfileImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profilepics");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{updateDto.ProfileImageFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updateDto.ProfileImageFile.CopyToAsync(stream);
                }

                if (!string.IsNullOrWhiteSpace(user.ProfilePicture))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfilePicture);
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                user.ProfilePicture = Path.Combine("profilepics", fileName).Replace("\\", "/");
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
