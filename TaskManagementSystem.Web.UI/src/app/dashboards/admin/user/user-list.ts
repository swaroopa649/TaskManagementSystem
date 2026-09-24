import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

import { UserService } from '../../../services/user.service';
import { UserResponseDto } from '../../../models/user.model';
import { SpinnerLoadingService } from '../../../services/spinner-loading-service';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './user-list.html',
  styleUrls: ['./user-list.css']
})
export class UserListComponent implements OnInit, OnDestroy {

  // Data
  users: UserResponseDto[] = [];
  filteredUsers: UserResponseDto[] = [];

  // Filters
  searchTerm = '';
  filterRole: number | 'all' = 'all';
  filterStatus: 'all' | 'active' | 'inactive' = 'all';

  // UI state
  isLoading = false;

  private sub?: Subscription;

  constructor(
    private userService: UserService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: SpinnerLoadingService
  ) {}

  // ============================================================
  // LIFECYCLE
  // ============================================================
  ngOnInit(): void {
    this.loadUsers();
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
    this.spinner.forceHide();
  }

  // ============================================================
  // LOAD
  // ============================================================
  loadUsers(): void {
    console.log('🟡 loadUsers() START');

    this.isLoading = true;
    this.spinner.show('Loading users...');

    this.sub?.unsubscribe();

    this.sub = this.userService.getAll().subscribe({
      next: (data) => {
        console.log('🟢 Data received:', data);

        // Handle plain array OR .NET $values wrapper
        const arr: UserResponseDto[] = this.normalizeArray(data);

        console.log('🟢 Normalized length:', arr.length);

        this.users = arr;
        this.applyFilters();

        this.isLoading = false;
        this.spinner.forceHide();
      },
      error: (err) => {
        console.error('🔴 Load error:', err);

        this.users = [];
        this.filteredUsers = [];
        this.isLoading = false;
        this.spinner.forceHide();

        this.toastr.error(
          err?.error?.message || err?.message || 'Failed to load users.',
          'Error'
        );
      },
      complete: () => {
        console.log('🔵 Complete');
        // Safety net — never leave the spinner stuck
        this.isLoading = false;
        this.spinner.forceHide();
      }
    });
  }

  // ============================================================
  // FILTER
  // ============================================================
  applyFilters(): void {
    const term = this.searchTerm.trim().toLowerCase();

    this.filteredUsers = this.users.filter((u) => {
      const matchesSearch =
        !term ||
        u.firstName?.toLowerCase().includes(term) ||
        u.lastName?.toLowerCase().includes(term) ||
        u.email?.toLowerCase().includes(term) ||
        u.phone?.toLowerCase().includes(term);

      const matchesRole =
        this.filterRole === 'all' || u.roleId === this.filterRole;

      const matchesStatus =
        this.filterStatus === 'all' ||
        (this.filterStatus === 'active' && u.isActive) ||
        (this.filterStatus === 'inactive' && !u.isActive);

      return matchesSearch && matchesRole && matchesStatus;
    });
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.filterRole = 'all';
    this.filterStatus = 'all';
    this.applyFilters();
  }

  // ============================================================
  // ROW ACTIONS
  // ============================================================
  edit(id: number): void {
    this.router.navigate(['/admin/users/edit', id]);
  }

  toggleStatus(user: UserResponseDto): void {
    const newStatus = !user.isActive;
    const verb = newStatus ? 'activate' : 'deactivate';

    if (!confirm(`Are you sure you want to ${verb} ${user.firstName} ${user.lastName}?`)) {
      return;
    }

    this.userService.updateStatus(user.id, newStatus).subscribe({
      next: () => {
        user.isActive = newStatus;
        this.applyFilters();
        this.toastr.success(`User ${verb}d successfully.`, 'Success');
      },
      error: (err) => {
        console.error('Status update failed:', err);
        this.toastr.error(`Failed to ${verb} user.`, 'Error');
      }
    });
  }

  deleteUser(user: UserResponseDto): void {
    if (!confirm(`Delete ${user.firstName} ${user.lastName}? This cannot be undone.`)) {
      return;
    }

    this.userService.delete(user.id).subscribe({
      next: () => {
        this.users = this.users.filter((u) => u.id !== user.id);
        this.applyFilters();
        this.toastr.success('User deleted successfully.', 'Success');
      },
      error: (err) => {
        console.error('Delete failed:', err);
        this.toastr.error('Failed to delete user.', 'Error');
      }
    });
  }

  // ============================================================
  // TEMPLATE HELPERS
  // ============================================================
  fullName(user: UserResponseDto): string {
    return `${user.firstName ?? ''} ${user.lastName ?? ''}`.trim();
  }

  initials(user: UserResponseDto): string {
    const f = user.firstName?.charAt(0) ?? '';
    const l = user.lastName?.charAt(0) ?? '';
    return (f + l).toUpperCase() || '?';
  }

  getRoleBadgeClass(roleCode?: string | null): string {
    switch ((roleCode ?? '').toUpperCase()) {
      case 'ADMIN':   return 'badge-light-danger';
      case 'MANAGER': return 'badge-light-warning';
      case 'USER':    return 'badge-light-success';
      default:        return 'badge-light-secondary';
    }
  }

  trackByUserId(index: number, user: UserResponseDto): number {
    return user.id;
  }

  // ============================================================
  // PRIVATE — normalize API shape
  // ============================================================
  private normalizeArray(data: any): UserResponseDto[] {
    if (!data) return [];
    if (Array.isArray(data)) return data as UserResponseDto[];

    // .NET ReferenceHandler.Preserve wrapper
    if (Array.isArray(data.$values)) return data.$values as UserResponseDto[];

    // Any object with numeric-indexed user entries
    if (typeof data === 'object') {
      const vals = Object.values(data).filter(
        (v) => v && typeof v === 'object' && 'id' in (v as any)
      ) as UserResponseDto[];
      if (vals.length > 0) return vals;
    }

    console.warn('Unexpected /User response shape:', data);
    return [];
  }
}