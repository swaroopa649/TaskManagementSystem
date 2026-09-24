using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.API.Data;
using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Respository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDBContext _context;

        public TaskRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeamTask>> GetAllAsync()
        {
            return await _context.Set<TeamTask>()
                .Include(t => t.Status)
                .Include(t => t.Team)
                .Include(t => t.Assignee)
                .Include(t => t.Creator)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .Where(t => t.IsActive)
                .ToListAsync();
        }

        public async Task<TeamTask?> GetByIdAsync(long id)
        {
            return await _context.Set<TeamTask>()
                .Include(t => t.Status)
                .Include(t => t.Team)
                .Include(t => t.Assignee)
                .Include(t => t.Creator)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive);
        }

        public async Task<IEnumerable<TeamTask>> GetByAssigneeIdAsync(long assigneeId)
        {
            return await _context.Set<TeamTask>()
                .Include(t => t.Status)
                .Include(t => t.Team)
                .Include(t => t.Assignee)
                .Where(t => t.AssigneeId == assigneeId && t.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<TeamTask>> GetByTeamIdAsync(long teamId)
        {
            return await _context.Set<TeamTask>()
                .Include(t => t.Status)
                .Include(t => t.Team)
                .Include(t => t.Assignee)
                .Where(t => t.TeamId == teamId && t.IsActive)
                .ToListAsync();
        }

        public async Task<TeamTask> CreateAsync(TeamTask entity)
        {
            await _context.Set<TeamTask>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TeamTask> UpdateAsync(TeamTask entity)
        {
            _context.Set<TeamTask>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.Set<TeamTask>().FindAsync(id);
            if (entity == null) return false;

            entity.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
