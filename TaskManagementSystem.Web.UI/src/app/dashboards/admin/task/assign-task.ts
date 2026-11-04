import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { TaskService } from '../../../services/task.service';
import { UserService } from '../../../services/user.service';
import { TaskResponseDto, AssignTaskDto } from '../../../models/task.model';
import { UserResponseDto } from '../../../models/user.model';
import { SpinnerLoadingService } from '../../../services/spinner-loading-service';

@Component({
  selector: 'app-assign-task',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './assign-task.html',
  styleUrls: ['./assign-task.css']
})
export class AssignTaskComponent implements OnInit {

  tasks: TaskResponseDto[] = [];
  users: UserResponseDto[] = [];

  model: AssignTaskDto = {   // fixed: was invalid syntax `AssignTaskDto {}= null`
    taskId: 0,
    assigneeId: 0
  };

  isLoading = false;
  loadingLookups = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private taskService: TaskService,
    private userService: UserService,
    private toastr: ToastrService,
    private spinner: SpinnerLoadingService
  ) {}

  ngOnInit(): void {
    // Optional preselect from query param: /manager/assign?taskId=42
    const taskIdParam = this.route.snapshot.queryParamMap.get('taskId');
    if (taskIdParam) {
      this.model.taskId = +taskIdParam;
    }

    this.loadTasks();
    this.loadUsers();
  }

  private loadTasks(): void {
    this.loadingLookups = true;
    this.taskService.getAll().subscribe({
      next: (t) => {
        this.tasks = t ?? [];
        this.loadingLookups = false;
      },
      error: () => {
        this.loadingLookups = false;
        this.toastr.error('Failed to load tasks.', 'Error');
      }
    });
  }

  private loadUsers(): void {
    this.userService.getAll().subscribe({
      next: (u) => (this.users = (u ?? []).filter(x => x.isActive)),
      error: () => this.toastr.error('Failed to load users.', 'Error')
    });
  }

  onSubmit(form: NgForm): void {
    if (!this.model.taskId || !this.model.assigneeId) {
      this.toastr.error('Please select a task and a user.', 'Validation');
      return;
    }

    this.isLoading = true;
    this.spinner.show('Assigning task...');

    this.taskService.assign(this.model).subscribe({
      next: () => {
        this.isLoading = false;
        this.spinner.forceHide();
        this.toastr.success('Task assigned successfully.', 'Success');
        this.router.navigate(['/manager/tasks']);
      },
      error: (err) => {
        this.isLoading = false;
        this.spinner.forceHide();
        this.toastr.error(err?.error?.message || 'Failed to assign task.', 'Error');
      }
    });
  }

  // ============================================================
  // HELPERS
  // ============================================================
  selectedTask(): TaskResponseDto | null {
    return this.tasks.find(t => t.id === this.model.taskId) ?? null;
  }

  cancel(): void {
    this.router.navigate(['/manager/tasks']);
  }
}