using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Models.Mappers
{
    public static class TeamMemberMapper
    {
        public static TeamMemberResponseDto ToDto(this TeamMember entity)
        {
            return new TeamMemberResponseDto
            {
                Id = entity.Id,
                TeamId = entity.TeamId,
                UserId = entity.UserId,
                UserName = entity.User != null
                    ? $"{entity.User.FirstName} {entity.User.LastName}".Trim()
                    : string.Empty,
                UserEmail = entity.User?.Email ?? string.Empty,
                AssignedOn = entity.AssignedOn,
                IsActive = entity.IsActive
            };
        }

        public static TeamMember ToEntity(this AssignTeamMemberDto dto, long assignedByUserId)
        {
            return new TeamMember
            {
                TeamId = dto.TeamId,
                UserId = dto.UserId,
                AssignedBy = assignedByUserId,
                AssignedOn = DateTimeOffset.UtcNow,
                IsActive = true
            };
        }
    }
}