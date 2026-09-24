using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Mappers;
using TaskManagementSystem.Web.API.Respository;

namespace TaskManagementSystem.Web.API.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<IEnumerable<TeamResponseDto>> GetAllTeamsAsync()
        {
            var teams = await _teamRepository.GetAllAsync();
            return teams.Select(t => t.ToDto());
        }

        public async Task<TeamResponseDto?> GetTeamByIdAsync(long id)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            return team?.ToDto();
        }

        public async Task<TeamResponseDto> CreateTeamAsync(CreateTeamDto dto, long userId)
        {
            var entity = dto.ToEntity(userId);
            var created = await _teamRepository.CreateAsync(entity);

            var result = await _teamRepository.GetByIdAsync(created.Id);
            return result!.ToDto();
        }

        public async Task<TeamResponseDto?> UpdateTeamAsync(long id, UpdateTeamDto dto, long userId)
        {
            var entity = await _teamRepository.GetByIdAsync(id);
            if (entity == null) return null;

            dto.UpdateEntity(entity, userId);
            await _teamRepository.UpdateAsync(entity);

            var updated = await _teamRepository.GetByIdAsync(id);
            return updated!.ToDto();
        }

        public async Task<bool> AssignMemberAsync(AssignTeamMemberDto dto, long assignedByUserId)
        {
            var entity = dto.ToEntity(assignedByUserId);
            return await _teamRepository.AddMemberAsync(entity);
        }

        public async Task<bool> RemoveMemberAsync(long teamId, long userId)
        {
            return await _teamRepository.RemoveMemberAsync(teamId, userId);
        }
    }
}
