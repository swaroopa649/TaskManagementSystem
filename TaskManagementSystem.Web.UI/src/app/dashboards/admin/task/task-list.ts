import { Component, OnInit, OnDestroy, Input, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

import { TaskService } from '../../../services/task.service';
import { AccountService } from '../../../services/account.service';
import { TaskResponseDto, TaskPriority } from '../../../models/task.model';
import { SpinnerLoadingService } from '../../../services/spinner-loading-service';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './task-list.html',
  styleUrls: ['./task-list.css']
})
export class TaskListComponent implements OnInit, OnDestroy {

  @Input() scope: 'all' | 'team' | 'my' | 'assigned' = 'all';
  @Input() title = 'Tasks';

  tasks: TaskResponseDto[] = [];
  filteredTasks: TaskResponseDto[] = [];

  searchTerm = '';
  filterPriority: TaskPriority | 'all' = 'all';
  filterStatus: 'all' | 'open' | 'completed' = 'all';

  isLoading = false;

  // TODO CONFIRM: real backend doesn't expose an IsCompleted flag or statusCode.
  // This is the ONLY place "completed" is defined — update this set once you
  // confirm the actual statusName value(s) (or switch to a statusId check).
  private readonly COMPLETED_STATUS_NAMES = new Set(['done', 'completed']);

  private sub?: Subscription;

  constructor(
    private taskService: TaskService,
    private router: Router,
    public account: AccountService,
    private toastr: ToastrService,
    private spinner: SpinnerLoadingService,
    private cdr: ChangeDetectorRef   // added — used to force a view update after async work
  ) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
    this.spinner.forceHide();
  }

  // ============================================================
  // LOAD
  // ============================================================
  loadTasks(): void {
    this.isLoading = true;
    this.spinner.show('Loading tasks...');

    this.sub?.unsubscribe();

    let obs$;
    switch (this.scope) {
      case 'my':
      case 'assigned':
        obs$ = this.taskService.getMyTasks();
        break;
      case 'team':
      case 'all':
      default:
        obs$ = this.taskService.getAll();
        break;
    }

    this.sub = obs$
      .pipe(
        finalize(() => {
          this.isLoading = false;
          this.spinner.forceHide();
          this.cdr.markForCheck(); // ensure the loading-row toggle is picked up too
        })
      )
      .subscribe({
        next: (data) => {
          this.tasks = Array.isArray(data) ? data : [];
          this.applyFilters();
          this.cdr.markForCheck(); // force Angular to re-render with the new array
        },
        error: (err) => {
          console.error(err);
          this.tasks = [];
          this.filteredTasks = [];
          this.toastr.error('Failed to load tasks.', 'Error');
          this.cdr.markForCheck();
        }
      });
  }

  // ============================================================
  // FILTERS
  // ============================================================
  applyFilters(): void {
    const term = this.searchTerm.trim().toLowerCase();

    this.filteredTasks = this.tasks.filter((t) => {
      const matchesSearch =
        !term ||
        t.title?.toLowerCase().includes(term) ||
        t.description?.toLowerCase().includes(term) ||
        t.assigneeName?.toLowerCase().includes(term);

      const matchesPriority =
        this.filterPriority === 'all' || t.priority === this.filterPriority;

      const completed = this.isCompleted(t);

      const matchesStatus =
        this.filterStatus === 'all' ||
        (this.filterStatus === 'open' && !completed) ||
        (this.filterStatus === 'completed' && completed);

      return matchesSearch && matchesPriority && matchesStatus;
    });
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.filterPriority = 'all';
    this.filterStatus = 'all';
    this.applyFilters();
    this.cdr.markForCheck();
  }

  // ============================================================
  // NAVIGATION
  // ============================================================
  createTask(): void {
    this.router.navigate([this.basePath(), 'tasks', 'create']);
  }

  viewTask(id: number): void {
    this.router.navigate([this.basePath(), 'tasks', id]);
  }

  editTask(id: number): void {
    this.router.navigate([this.basePath(), 'tasks', 'edit', id]);
  }

  assignTask(id: number): void {
    this.router.navigate(['/manager/assign'], { queryParams: { taskId: id } });
  }

  private basePath(): string {
    if (this.account.isAdmin()) return '/admin';
    if (this.account.isManager()) return '/manager';
    return '/user';
  }

  // ============================================================
  // ROW ACTIONS
  // ============================================================
  deleteTask(task: TaskResponseDto): void {
    if (!confirm(`Delete task "${task.title}"?`)) return;

    this.taskService.delete(task.id).subscribe({
      next: () => {
        this.tasks = this.tasks.filter(t => t.id !== task.id);
        this.applyFilters();
        this.toastr.success('Task deleted.', 'Success');
        this.cdr.markForCheck();
      },
      error: () => this.toastr.error('Failed to delete task.', 'Error')
    });
  }

  // ============================================================
  // HELPERS
  // ============================================================
  isCompleted(task: TaskResponseDto): boolean {
    return this.COMPLETED_STATUS_NAMES.has((task.statusName ?? '').trim().toLowerCase());
  }

  priorityBadgeClass(priority: TaskPriority): string {
    switch ((priority ?? '').toUpperCase()) {
      case 'CRITICAL': return 'badge-light-danger';
      case 'HIGH':     return 'badge-light-warning';
      case 'MEDIUM':   return 'badge-light-primary';
      case 'LOW':      return 'badge-light-success';
      default:         return 'badge-light-secondary';
    }
  }

  isOverdue(task: TaskResponseDto): boolean {
    if (!task.dueDate || this.isCompleted(task)) return false;
    return new Date(task.dueDate).getTime() < Date.now();
  }

  trackByTaskId(index: number, task: TaskResponseDto): number {
    return task.id;
  }
}