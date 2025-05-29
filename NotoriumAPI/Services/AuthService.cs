using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NotoriumAPI.DTOs;
using NotoriumAPI.Mappers;
using NotoriumAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NotoriumAPI.Services
{
    public class AuthService(NotoriumDbContext context, IConfiguration configuration)
    {
        public async Task<bool> EmailExists(string email)
        {
            return await context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<UserDTO?> Register(RegisterDTO request)
        {
            if (await EmailExists(request.Email))
                throw new ArgumentException("Email is already taken.");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Username = request.Username,
                Password = passwordHash,
                Role = request.Role
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return UserMapper.ToDTO(user);
        }

        public async Task<(string token, object user)?> Login(LoginDTO request)
        {
            var user = await context.Users.SingleOrDefaultAsync(x => x.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return null;

            string token = GenerateJwtToken(user);

            return (token, new { id = user.Id, user.Username });
        }

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                ]),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
