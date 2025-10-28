using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Data;
using StudentActivities.src.Models;
using StudentActivities.src.Repositories.Interfaces;

namespace StudentActivities.src.Repositories.Implements
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Users?> GetByUserNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}
