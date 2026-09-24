using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Mappers;
using TaskManagementSystem.Web.API.Respository;

namespace TaskManagementSystem.Web.API.Services
{
    public interface ITaskCommentService
    {
        Task<IEnumerable<TaskCommentResponseDto>> GetCommentsByTaskIdAsync(long taskId);
        Task<TaskCommentResponseDto> AddCommentAsync(CreateTaskCommentDto dto, long userId);
    }

    
}