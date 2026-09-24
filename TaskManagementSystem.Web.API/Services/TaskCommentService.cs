using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Mappers;
using TaskManagementSystem.Web.API.Respository;

namespace TaskManagementSystem.Web.API.Services
{
    public class TaskCommentService : ITaskCommentService
    {
        private readonly ITaskCommentRepository _commentRepository;

        public TaskCommentService(ITaskCommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<IEnumerable<TaskCommentResponseDto>> GetCommentsByTaskIdAsync(long taskId)
        {
            var comments = await _commentRepository.GetByTaskIdAsync(taskId);
            return comments.Select(c => c.ToDto());
        }

        public async Task<TaskCommentResponseDto> AddCommentAsync(CreateTaskCommentDto dto, long userId)
        {
            var entity = dto.ToEntity(userId);
            var created = await _commentRepository.CreateAsync(entity);
            return created.ToDto();
        }
    }
}
