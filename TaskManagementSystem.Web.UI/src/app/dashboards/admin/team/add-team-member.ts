import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { TeamService } from '../../../services/team.service';
import { UserService } from '../../../services/user.service';
import { TeamResponseDto, AssignTeamMemberDto } from '../../../models/team.model';
import { UserResponseDto } from '../../../models/user.model';
import { SpinnerLoadingService } from '../../../services/spinner-loading-service';

@Component({
  selector: 'app-add-team-member',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-team-member.html',
  styleUrls: ['./add-team-member.css']
})
export class AddTeamMemberComponent implements OnInit {

  teamId = 0;
  team: TeamResponseDto | null = null;

  availableUsers: UserResponseDto[] = [];

  model: AssignTeamMemberDto = {
    teamId: 0,
    userId: 0
  };

  searchTerm = '';
  isLoading = false;
  loadingUsers = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private teamService: TeamService,
    private userService: UserService,
    private toastr: ToastrService,
    private spinner: SpinnerLoadingService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (!idParam) {
      this.toastr.error('Invalid team id.', 'Error');
      this.router.navigate(['/admin/teams']);
      return;
    }

    this.teamId = +idParam;
    this.model.teamId = this.teamId;

    this.loadTeam();
    this.loadAvailableUsers();
  }

  // ============================================================
  // LOAD TEAM DETAILS
  // ============================================================
  private loadTeam(): void {
    this.teamService.getById(this.teamId).subscribe({
      next: (t) => (this.team = t),
      error: (err) => {
        console.error(err);
        this.toastr.error('Failed to load team.', 'Error');
        this.router.navigate(['/admin/teams']);
      }
    });
  }

  // ============================================================
  // LOAD ALL USERS, EXCLUDE THOSE ALREADY IN THE TEAM
  // ============================================================
  private loadAvailableUsers(): void {
    this.loadingUsers = true;

    this.userService.getAll().subscribe({
      next: (users) => {
        const existingIds = new Set((this.team?.members ?? []).map(m => m.userId));
        this.availableUsers = (users ?? []).filter(
          u => u.isActive && !existingIds.has(u.id)
        );
        this.loadingUsers = false;
      },
      error: (err) => {
        console.error(err);
        this.loadingUsers = false;
        this.toastr.error('Failed to load users.', 'Error');
      }
    });
  }

  // ============================================================
  // FILTERED USERS (for dropdown)
  // ============================================================
  get filteredUsers(): UserResponseDto[] {
    const term = this.searchTerm.trim().toLowerCase();
    if (!term) return this.availableUsers;

    return this.availableUsers.filter(u =>
      u.firstName?.toLowerCase().includes(term) ||
      u.lastName?.toLowerCase().includes(term) ||
      u.email?.toLowerCase().includes(term)
    );
  }

  // ============================================================
  // SUBMIT
  // ============================================================
  onSubmit(form: NgForm): void {
    if (!this.model.userId) {
      this.toastr.error('Please select a user.', 'Validation');
      return;
    }

    this.isLoading = true;
    this.spinner.show('Adding member...');

    this.teamService.assignMember(this.model).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.spinner.forceHide();
        this.toastr.success(res?.message || 'Member added.', 'Success');
        this.router.navigate(['/admin/teams']);
      },
      error: (err) => {
        this.isLoading = false;
        this.spinner.forceHide();
        this.toastr.error(
          err?.error?.message || 'Failed to add member.',
          'Error'
        );
      }
    });
  }

  // ============================================================
  // UI HELPERS
  // ============================================================
  fullName(u: UserResponseDto): string {
    return `${u.firstName ?? ''} ${u.lastName ?? ''}`.trim();
  }

  initials(u: UserResponseDto): string {
    const f = u.firstName?.charAt(0) ?? '';
    const l = u.lastName?.charAt(0) ?? '';
    return (f + l).toUpperCase() || '?';
  }

  cancel(): void {
    this.router.navigate(['/admin/teams']);
  }
}