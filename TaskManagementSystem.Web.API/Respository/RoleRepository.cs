using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.API.Data;
using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Respository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDBContext _context;

        public RoleRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(long id)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Role?> GetByCodeAsync(string code)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<bool> ExistsByCodeAsync(
            string code,
            long? excludeId = null)
        {
            return await _context.Roles
                .AnyAsync(x =>
                    x.Code == code &&
                    (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task<Role> AddAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            return role;
        }

        public async Task UpdateAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }
    }
}
