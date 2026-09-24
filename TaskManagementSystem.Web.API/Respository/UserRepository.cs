using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.API.Data;
using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Respository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;

        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.Include(x => x.Role).AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetByIdAsync(long id)
        {
            return await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<bool> ExistsByEmailAsync(string email, long? excludeId = null)
        {
            return await _context.Users.AnyAsync(x => x.Email == email && (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task<bool> RoleExistsAsync(long roleId)
        {
            return await _context.Roles.AnyAsync(x => x.Id == roleId && x.IsActive == true);
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);

            await _context.SaveChangesAsync();
        }
    }
}
