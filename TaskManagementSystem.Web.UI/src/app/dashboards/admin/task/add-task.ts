import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { TaskService } from '../../../services/task.service';
import { UserService } from '../../../services/user.service';
import { TeamService } from '../../../services/team.service';
import {
  CreateTaskDto,
  UpdateTaskDto,
  TaskPriority,
  TaskStatusResponseDto
} from '../../../models/task.model';
import { UserResponseDto } from '../../../models/user.model';
import { TeamResponseDto } from '../../../models/team.model';
import { SpinnerLoadingService } from '../../../services/spinner-loading-service';

@Component({
  selector: 'app-add-task',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-task.html',
  styleUrls: ['./add-task.css']
})
export class AddTaskComponent implements OnInit {

  id = 0;
  isEditMode = false;

  model: CreateTaskDto = {
    title: '',
    description: '',
    priority: 'Medium',   // was 'MEDIUM' — TaskPriority is now 'Low' | 'Medium' | 'High' | 'Critical'
    assigneeId: null,
    teamId: null,
    dueDate: null,
    statusId: 1
  };

  isActive = true;

  users: UserResponseDto[] = [];
  teams: TeamResponseDto[] = [];
  statuses: TaskStatusResponseDto[] = [];

  priorities: TaskPriority[] = ['Low', 'Medium', 'High', 'Critical']; // was uppercase — must match TaskPriority literal casing

  isLoading = false;
  loadingLookups = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private taskService: TaskService,
    private userService: UserService,
    private teamService: TeamService,
    private toastr: ToastrService,
    private spinner: SpinnerLoadingService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      this.isEditMode = true;
    }

    this.loadLookups();

    if (this.isEditMode) {
      this.loadTask();
    }
  }

  // ============================================================
  // LOOKUPS (users, teams, statuses)
  // ============================================================
  private loadLookups(): void {
    this.loadingLookups = true;

    this.userService.getAll().subscribe({
      next: (u) => (this.users = (u ?? []).filter(x => x.isActive)),
      error: () => this.toastr.error('Failed to load users.', 'Error')
    });

    this.teamService.getAll().subscribe({
      next: (t) => (this.teams = (t ?? []).filter(x => x.isActive)),
      error: () => {/* not critical */}
    });

    this.taskService.getStatuses().subscribe({
      next: (s) => {
        this.statuses = s ?? [];
        this.loadingLookups = false;
      },
      error: () => {
        this.loadingLookups = false;
        // Not critical — statusId falls back to the default (1)
      }
    });
  }

  // ============================================================
  // LOAD TASK (edit mode)
  // ============================================================
  private loadTask(): void {
    this.isLoading = true;
    this.spinner.show('Loading task...');

    this.taskService.getById(this.id).subscribe({
      next: (t) => {
        this.model = {
          title: t.title,
          description: t.description ?? '',
          priority: t.priority,
          assigneeId: t.assigneeId ?? null, // fixed: was t.assigneeId, which doesn't exist on TaskResponseDto
          teamId: t.teamId ?? null,
          dueDate: t.dueDate ?? null,
          statusId: t.statusId ?? 1
        };
        this.isActive = t.isActive;
        this.isLoading = false;
        this.spinner.forceHide();
      },
      error: () => {
        this.isLoading = false;
        this.spinner.forceHide();
        this.toastr.error('Failed to load task.', 'Error');
        this.router.navigate([this.basePath(), 'tasks']);
      }
    });
  }

  // ============================================================
  // SUBMIT
  // ============================================================
  onSubmit(form: NgForm): void {
    if (form.invalid) return;
    if (!this.model.assigneeId) {
      this.toastr.error('Please select an assignee.', 'Validation');
      return;
    }

    this.isLoading = true;
    this.spinner.show(this.isEditMode ? 'Saving task...' : 'Creating task...');

    if (this.isEditMode) {
      const dto: UpdateTaskDto = {
        ...this.model,
        statusId: this.model.statusId ?? 1, // UpdateTaskDto.statusId is non-nullable — guard against null
        isActive: this.isActive
      };
      this.taskService.update(this.id, dto).subscribe({
        next: () => {
          this.isLoading = false;
          this.spinner.forceHide();
          this.toastr.success('Task updated.', 'Success');
          this.router.navigate([this.basePath(), 'tasks']);
        },
        error: (err) => {
          this.isLoading = false;
          this.spinner.forceHide();
          this.toastr.error(err?.error?.message || 'Failed to update task.', 'Error');
        }
      });
    } else {
      this.taskService.create(this.model).subscribe({
        next: () => {
          this.isLoading = false;
          this.spinner.forceHide();
          this.toastr.success('Task created.', 'Success');
          this.router.navigate([this.basePath(), 'tasks']);
        },
        error: (err) => {
          this.isLoading = false;
          this.spinner.forceHide();
          this.toastr.error(err?.error?.message || 'Failed to create task.', 'Error');
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate([this.basePath(), 'tasks']);
  }

  private basePath(): string {
    // Preserve current role prefix
    return this.router.url.startsWith('/manager') ? '/manager' : '/admin';
  }
}