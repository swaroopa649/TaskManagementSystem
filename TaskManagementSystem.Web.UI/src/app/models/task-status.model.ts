// Mirrors: TaskManagementSystem.Web.API.Models.DTOs.TaskStatusResponseDto
export interface TaskStatusResponseDto {
  id: number;
  code: string;
  name: string;
  isActive: boolean;
}

// Mirrors: TaskManagementSystem.Web.API.Models.DTOs.CreateTaskStatusDto
export interface CreateTaskStatusDto {
  id: number;
  code: string;
  name: string;
  isActive: boolean;
}