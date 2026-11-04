import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

import { TaskService } from '../../../services/task.service';
import { AccountService } from '../../../services/account.service';
import { TaskResponseDto, TaskPriority } from '../../../models/task.model';
import { SpinnerLoadingService } from '../../../services/spinner-loading-service';
import { TaskCommentsComponent } from '../../shared/tasks/task-comments/task-comments';

@Component({
  selector: 'app-task-details',
  standalone: true,
  imports: [CommonModule, RouterLink, TaskCommentsComponent],
  templateUrl: './task-details.html',
  styleUrls: ['./task-details.css']
})
export class TaskDetailsComponent implements OnInit, OnDestroy {

  taskId = 0;
  task: TaskResponseDto | null = null;
  isLoading = false;

  // TODO CONFIRM: same completed-status guess as task-list.ts — keep both
  // in sync until you confirm the real value, or factor this out into a
  // shared helper/service once confirmed.
  private readonly COMPLETED_STATUS_NAMES = new Set(['done', 'completed']);

  private sub?: Subscription;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private taskService: TaskService,
    public account: AccountService,
    private toastr: ToastrService,
    private spinner: SpinnerLoadingService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (!idParam) {
      this.toastr.error('Invalid task id.', 'Error');
      this.back();
      return;
    }
    this.taskId = +idParam;
    this.loadTask();
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
    this.spinner.forceHide();
  }

  // ============================================================
  // LOAD
  // ============================================================
  loadTask(): void {
    this.isLoading = true;
    this.spinner.show('Loading task...');

    this.sub?.unsubscribe();
    this.sub = this.taskService.getById(this.taskId).subscribe({
      next: (t) => {
        this.task = t;
        this.isLoading = false;
        this.spinner.forceHide();
      },
      error: () => {
        this.isLoading = false;
        this.spinner.forceHide();
        this.toastr.error('Failed to load task.', 'Error');
        this.back();
      }
    });
  }

  // ============================================================
  // ACTIONS
  // ============================================================
  edit(): void {
    this.router.navigate([this.basePath(), 'tasks', 'edit', this.taskId]);
  }

  assign(): void {
    this.router.navigate(['/manager/assign'], { queryParams: { taskId: this.taskId } });
  }

  back(): void {
    this.router.navigate([this.basePath(), 'tasks']);
  }

  private basePath(): string {
    if (this.account.isAdmin()) return '/admin';
    if (this.account.isManager()) return '/manager';
    return '/user';
  }

  // ============================================================
  // HELPERS
  // ============================================================
  isCompleted(): boolean {
    if (!this.task) return false;
    return this.COMPLETED_STATUS_NAMES.has((this.task.statusName ?? '').trim().toLowerCase());
  }

  isOverdue(): boolean {
    if (!this.task?.dueDate || this.isCompleted()) return false;
    return new Date(this.task.dueDate).getTime() < Date.now();
  }

  priorityBadgeClass(priority?: TaskPriority): string {
    switch ((priority ?? '').toUpperCase()) {
      case 'CRITICAL': return 'badge-light-danger';
      case 'HIGH':     return 'badge-light-warning';
      case 'MEDIUM':   return 'badge-light-primary';
      case 'LOW':      return 'badge-light-success';
      default:         return 'badge-light-secondary';
    }
  }
}