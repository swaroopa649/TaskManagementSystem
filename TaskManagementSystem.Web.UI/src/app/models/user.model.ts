// ============================================================
// Mirrors: TaskManagementSystem.Web.API.Models.DTOs.UserResponseDto
// ============================================================
export interface UserResponseDto {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string | null;
  roleId?: number | null;
  roleName?: string | null;
  roleCode?: string | null;
  lastLogin?: string | null;      // ISO date string from backend
  isActive: boolean;
  createdOn: string;              // ISO date string
  modifiedOn?: string | null;
}

// ============================================================
// Mirrors: TaskManagementSystem.Web.API.Models.DTOs.CreateUserDto
// ============================================================
export interface CreateUserDto {
  firstName: string;
  lastName: string;
  email: string;
  phone?: string | null;
  roleId?: number | null;
  password: string;
  isActive: boolean;
}

// ============================================================
// Mirrors: TaskManagementSystem.Web.API.Models.DTOs.UpdateUserDto
// ============================================================
export interface UpdateUserDto {
  firstName: string;
  lastName: string;
  email: string;
  phone?: string | null;
  roleId?: number | null;
  isActive: boolean;
}