using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.API.Data;
using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Respository
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team?> GetByIdAsync(long id);
        Task<Team> CreateAsync(Team entity);
        Task<Team> UpdateAsync(Team entity);
        Task<bool> AddMemberAsync(TeamMember member);
        Task<bool> RemoveMemberAsync(long teamId, long userId);
    }

    
}