using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.API.Data;
using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Respository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDBContext _context;

        public TeamRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await _context.Set<Team>()
                .Include(t => t.Manager)
                .Include(t => t.TeamMembers)
                    .ThenInclude(m => m.User)
                .Where(t => t.IsActive)
                .ToListAsync();
        }

        public async Task<Team?> GetByIdAsync(long id)
        {
            return await _context.Set<Team>()
                .Include(t => t.Manager)
                .Include(t => t.TeamMembers)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive);
        }

        public async Task<Team> CreateAsync(Team entity)
        {
            await _context.Set<Team>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Team> UpdateAsync(Team entity)
        {
            _context.Set<Team>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> AddMemberAsync(TeamMember member)
        {
            var exists = await _context.Set<TeamMember>()
                .AnyAsync(m => m.TeamId == member.TeamId && m.UserId == member.UserId && m.IsActive);

            if (exists) return false;

            await _context.Set<TeamMember>().AddAsync(member);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveMemberAsync(long teamId, long userId)
        {
            var member = await _context.Set<TeamMember>()
                .FirstOrDefaultAsync(m => m.TeamId == teamId && m.UserId == userId && m.IsActive);

            if (member == null) return false;

            member.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
