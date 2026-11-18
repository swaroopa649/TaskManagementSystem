using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Models.Mappers
{
    public static class ProjectMapper
    {
        public static ProjectResponseDto ToDto(this Project project)
        {
            return new ProjectResponseDto
            {
                ProjectId = project.ProjectId,
                ProjectKey = project.ProjectKey,
                Name = project.Name,
                Description = project.Description,
                LeadUserId = project.LeadUserId,
                CreatedBy = project.CreatedBy,
                CreatedOn = project.CreatedOn,
                ModifiedBy = project.ModifiedBy,
                ModifiedOn = project.ModifiedOn,
                IsActive = project.IsActive,
                CreatedDate = project.CreatedDate
            };
        }

        public static Project ToEntity(this CreateProjectDto dto)
        {
            return new Project
            {
                ProjectKey = dto.ProjectKey,
                Name = dto.Name,
                Description = dto.Description,
                LeadUserId = dto.LeadUserId,
                IsActive = dto.IsActive
            };
        }

        public static void UpdateEntity(this UpdateProjectDto dto, Project project)
        {
            project.ProjectKey = dto.ProjectKey;
            project.Name = dto.Name;
            project.Description = dto.Description;
            project.LeadUserId = dto.LeadUserId;
            project.IsActive = dto.IsActive;
        }
    }
}