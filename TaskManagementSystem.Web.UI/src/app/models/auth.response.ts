// Mirrors: TaskManagementSystem.Web.API.Models.DTOs.RoleResponseDto
export interface RoleResponseDto {
  id: number;
  name?: string | null;
  code?: string | null;
  isActive?: boolean | null;
}

// Mirrors: TaskManagementSystem.Web.API.Models.DTOs.ApplicationUser
export interface ApplicationUser {
  userId: number;
  firstName?: string | null;
  lastName?: string | null;
  email?: string | null;
  phone?: string | null;
  roleId: number;
  isActive: boolean;
  roleInfo?: RoleResponseDto | null;
}

// Mirrors: TaskManagementSystem.Web.API.Models.DTOs.AuthResponseDTO
export interface AuthResponse {
  message: string;
  isValidUser: boolean;
  isValidPassword: boolean;
  jwtToken: string;
  applicationUser: ApplicationUser;
}