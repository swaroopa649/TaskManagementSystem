using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Mappers;
using TaskManagementSystem.Web.API.Respository;

namespace TaskManagementSystem.Web.API.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<TeamResponseDto>> GetAllTeamsAsync();
        Task<TeamResponseDto?> GetTeamByIdAsync(long id);
        Task<TeamResponseDto> CreateTeamAsync(CreateTeamDto dto, long userId);
        Task<TeamResponseDto?> UpdateTeamAsync(long id, UpdateTeamDto dto, long userId);
        Task<bool> AssignMemberAsync(AssignTeamMemberDto dto, long assignedByUserId);
        Task<bool> RemoveMemberAsync(long teamId, long userId);
    }

   
}