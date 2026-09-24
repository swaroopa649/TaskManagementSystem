import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

import { TeamService } from '../../../services/team.service';
import { TeamResponseDto, TeamMemberResponseDto } from '../../../models/team.model';
import { SpinnerLoadingService } from '../../../services/spinner-loading-service';

@Component({
  selector: 'app-team-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './team-list.html',
  styleUrls: ['./team-list.css']
})
export class TeamListComponent implements OnInit, OnDestroy {

  teams: TeamResponseDto[] = [];
  filteredTeams: TeamResponseDto[] = [];

  searchTerm = '';
  filterStatus: 'all' | 'active' | 'inactive' = 'all';

  // Which teams have their member list expanded
  expandedTeamIds = new Set<number>();

  isLoading = false;

  private sub?: Subscription;

  constructor(
    private teamService: TeamService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: SpinnerLoadingService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadTeams();
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
    this.spinner.forceHide();
  }

  // ============================================================
  // LOAD
  // ============================================================
  loadTeams(): void {
    this.isLoading = true;
    this.spinner.show('Loading teams...');

    this.sub?.unsubscribe();
    this.sub = this.teamService.getAll().subscribe({
      next: (data) => {
        this.teams = Array.isArray(data) ? data : [];
        this.applyFilters();
        this.isLoading = false;
        this.spinner.forceHide();
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
        this.teams = [];
        this.filteredTeams = [];
        this.isLoading = false;
        this.spinner.forceHide();
        this.toastr.error('Failed to load teams.', 'Error');
      },
      complete: () => {
        this.isLoading = false;
        this.spinner.forceHide();
      }
    });
  }

  // ============================================================
  // FILTERS
  // ============================================================
  applyFilters(): void {
    const term = this.searchTerm.trim().toLowerCase();

    this.filteredTeams = this.teams.filter((t) => {
      const matchesSearch =
        !term ||
        t.name?.toLowerCase().includes(term) ||
        t.description?.toLowerCase().includes(term) ||
        t.managerName?.toLowerCase().includes(term);

      const matchesStatus =
        this.filterStatus === 'all' ||
        (this.filterStatus === 'active' && t.isActive) ||
        (this.filterStatus === 'inactive' && !t.isActive);

      return matchesSearch && matchesStatus;
    });
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.filterStatus = 'all';
    this.applyFilters();
  }

  // ============================================================
  // EXPAND/COLLAPSE MEMBERS
  // ============================================================
  toggleMembers(teamId: number): void {
    if (this.expandedTeamIds.has(teamId)) {
      this.expandedTeamIds.delete(teamId);
    } else {
      this.expandedTeamIds.add(teamId);
    }
  }

  isExpanded(teamId: number): boolean {
    return this.expandedTeamIds.has(teamId);
  }

  // ============================================================
  // NAVIGATION
  // ============================================================
  addTeam(): void {
    this.router.navigate(['/admin/teams/create']);
  }

  editTeam(id: number): void {
    this.router.navigate(['/admin/teams/edit', id]);
  }

  addMember(teamId: number): void {
    this.router.navigate(['/admin/teams', teamId, 'members', 'add']);
  }

  // ============================================================
  // REMOVE MEMBER
  // ============================================================
  removeMember(team: TeamResponseDto, member: TeamMemberResponseDto): void {
    if (!confirm(`Remove ${member.userName} from "${team.name}"?`)) return;

    this.teamService.removeMember(team.id, member.userId).subscribe({
      next: () => {
        team.members = team.members.filter((m) => m.userId !== member.userId);
        this.applyFilters();
        this.toastr.success('Member removed.', 'Success');
      },
      error: (err) => {
        console.error(err);
        this.toastr.error('Failed to remove member.', 'Error');
      }
    });
  }

  // ============================================================
  // HELPERS
  // ============================================================
  memberInitials(m: TeamMemberResponseDto): string {
    const parts = (m.userName ?? '').trim().split(/\s+/);
    const f = parts[0]?.charAt(0) ?? '';
    const l = parts[1]?.charAt(0) ?? '';
    return (f + l).toUpperCase() || '?';
  }

  trackByTeamId(index: number, team: TeamResponseDto): number {
    return team.id;
  }

  trackByMemberId(index: number, member: TeamMemberResponseDto): number {
    return member.id;
  }
}