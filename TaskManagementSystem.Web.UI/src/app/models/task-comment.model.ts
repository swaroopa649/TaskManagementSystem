// Mirrors: TaskManagementSystem.Web.API.Models.DTOs.TaskCommentResponseDto
export interface TaskCommentResponseDto {
  id: number;
  taskId: number;
  userId: number;
  userName: string;
  comment: string;
  createdOn: string;   // ISO date string
}

// Mirrors: TaskManagementSystem.Web.API.Models.DTOs.CreateTaskCommentDto
export interface CreateTaskCommentDto {
  taskId: number;
  comment: string;
}