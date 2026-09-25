// ============================================================
// Task Status
// ============================================================
export interface TaskStatusResponseDto {
  id: number;
  code: string;
  name: string;
  isActive: boolean;
}

// ============================================================
// Task Priority (backend sends this as a plain string, not an enum)
// ============================================================
export type TaskPriority = 'Low' | 'Medium' | 'High' | 'Critical';

// ============================================================
// Task Comment (referenced by TaskResponseDto.Comments — define based on
// your actual TaskCommentResponseDto; placeholder shape below, please confirm)
// ============================================================
export interface TaskCommentResponseDto {
  id: number;
  taskId: number;
  comment: string;
  createdBy: number;
  createdByName?: string | null;
  createdOn: string;
}

// ============================================================
// Task Response — matches TaskManagementSystem.Web.API.Models.DTOs.TaskResponseDto
// ============================================================
export interface TaskResponseDto {
  id: number;                      // long
  title: string;
  description?: string | null;
  statusId: number;                // long
  statusName: string;              // non-nullable in C#, defaults to ""
  priority: TaskPriority;          // string in C#
  teamId?: number | null;          // long?
  teamName?: string | null;
  assigneeId?: number | null;      // long? — renamed from assigneeId
  assigneeName?: string | null;    // renamed from assigneeName
  dueDate?: string | null;         // DateTimeOffset? → ISO string
  isActive: boolean;
  createdBy: number;               // long, non-nullable
  createdByName?: string | null;
  createdOn: string;               // DateTimeOffset, non-nullable
  modifiedBy?: number | null;      // long?
  modifiedOn?: string | null;      // DateTimeOffset?
  comments: TaskCommentResponseDto[]; // List<>, defaults to []
}

// ============================================================
// Create Task — matches CreateTaskDto
// ============================================================
export interface CreateTaskDto {
  title: string;
  description?: string | null;
  statusId: number;           // long, default 1 ('To Do') — send explicitly
  priority: TaskPriority;     // default 'Medium' server-side
  teamId?: number | null;
  assigneeId?: number | null;
  dueDate?: string | null;    // DateTimeOffset? → ISO string
}

// ============================================================
// Update Task — matches UpdateTaskDto
// ============================================================
export interface UpdateTaskDto {
  title: string;
  description?: string | null;
  statusId: number;           // long, NON-nullable here (differs from CreateTaskDto's default-1 optionality — must always be sent)
  priority: TaskPriority;     // default 'Medium' server-side
  teamId?: number | null;
  assigneeId?: number | null;
  dueDate?: string | null;
  isActive: boolean;
}

// ============================================================
// Update Task Status — matches UpdateTaskStatusDto
// ============================================================
export interface UpdateTaskStatusDto {
  statusId: number; // long
}

// ============================================================
// Assign Task — NOT present in the C# file you shared.
// No AssignTaskDto class exists there. Placeholder below assumes
// taskId is passed via route param and only assigneeId is in the body —
// please confirm the actual endpoint/DTO shape before using this.
// ============================================================
export interface AssignTaskDto {
  assigneeId: number;
  taskId: number,
}