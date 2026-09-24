// ============================================================
// Mirrors: TaskManagementSystem.Web.API.Models.DTOs.TeamMemberResponseDto
// ============================================================
export interface TeamMemberResponseDto {
  id: number;
  teamId: number;
  userId: number;
  userName: string;
  userEmail: string;
  assignedOn: string;     // ISO date
  isActive: boolean;
}

// ============================================================
// Mirrors: TaskManagementSystem.Web.API.Models.DTOs.TeamResponseDto
// ============================================================
export interface TeamResponseDto {
  id: number;
  name: string;
  description?: string | null;
  managerId: number;
  managerName?: string | null;
  isActive: boolean;
  createdOn: string;      // ISO date
  members: TeamMemberResponseDto[];
}

// ============================================================
// Mirrors: TaskManagementSystem.Web.API.Models.DTOs.CreateTeamDto
// ============================================================
export interface CreateTeamDto {
  name: string;
  description?: string | null;
  managerId: number;
}

// ============================================================
// Mirrors: TaskManagementSystem.Web.API.Models.DTOs.UpdateTeamDto
// ============================================================
export interface UpdateTeamDto {
  name: string;
  description?: string | null;
  managerId: number;
  isActive: boolean;
}

// ============================================================
// Mirrors: TaskManagementSystem.Web.API.Models.DTOs.AssignTeamMemberDto
// ============================================================
export interface AssignTeamMemberDto {
  teamId: number;
  userId: number;
}