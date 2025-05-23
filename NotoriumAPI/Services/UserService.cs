using Microsoft.EntityFrameworkCore;
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

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public User? GetUserById(int id)
        {
            return _context.Users.SingleOrDefault(u => u.Id == id);
        }
    }
}
