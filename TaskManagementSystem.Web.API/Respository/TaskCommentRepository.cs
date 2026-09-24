using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.API.Data;
using TaskManagementSystem.Web.API.Models.Entities;
using TaskManagementSystem.Web.API.Respository;

namespace TaskManagementSystem.Web.API.Respository
{
    public class TaskCommentRepository : ITaskCommentRepository
    {
        private readonly ApplicationDBContext _context;

        public TaskCommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskComment>> GetByTaskIdAsync(long taskId)
        {
            return await _context.Set<TaskComment>()
                .Include(c => c.User)
                .Where(c => c.TaskId == taskId && c.IsActive)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();
        }

        public async Task<TaskComment> CreateAsync(TaskComment entity)
        {
            await _context.Set<TaskComment>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
