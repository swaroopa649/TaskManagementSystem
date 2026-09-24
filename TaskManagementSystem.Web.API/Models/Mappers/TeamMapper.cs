using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Models.Mappers
{
    public static class TeamMapper
    {
        public static TeamResponseDto ToDto(this Team entity)
        {
            return new TeamResponseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                ManagerId = entity.ManagerId,
                ManagerName = entity.Manager != null
                    ? $"{entity.Manager.FirstName} {entity.Manager.LastName}".Trim()
                    : null,
                IsActive = entity.IsActive,
                CreatedOn = entity.CreatedOn,
                Members = entity.TeamMembers?.Select(m => m.ToDto()).ToList() ?? new List<TeamMemberResponseDto>()
            };
        }

        public static Team ToEntity(this CreateTeamDto dto, long createdByUserId)
        {
            return new Team
            {
                Name = dto.Name,
                Description = dto.Description,
                ManagerId = dto.ManagerId,
                IsActive = true,
                CreatedBy = createdByUserId,
                CreatedOn = DateTimeOffset.UtcNow
            };
        }

        public static void UpdateEntity(this UpdateTeamDto dto, Team entity, long modifiedByUserId)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.ManagerId = dto.ManagerId;
            entity.IsActive = dto.IsActive;
            entity.ModifiedBy = modifiedByUserId;
            entity.ModifiedOn = DateTimeOffset.UtcNow;
        }
    }
}