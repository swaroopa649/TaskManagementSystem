import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { TeamService } from '../../../services/team.service';
import { UserService } from '../../../services/user.service';
import { CreateTeamDto } from '../../../models/team.model';
import { UserResponseDto } from '../../../models/user.model';
import { SpinnerLoadingService } from '../../../services/spinner-loading-service';

@Component({
  selector: 'app-add-team',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-team.html',
  styleUrls: ['./add-team.css']
})
export class AddTeamComponent implements OnInit {

  model: CreateTeamDto = {
    name: '',
    description: '',
    managerId: 0
  };

  managers: UserResponseDto[] = [];
  isLoading = false;
  loadingManagers = false;

  constructor(
    private teamService: TeamService,
    private userService: UserService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: SpinnerLoadingService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadManagers();
  }

  // ============================================================
  // LOAD MANAGERS (users with roleId = 2)
  // ============================================================
  private loadManagers(): void {
    this.loadingManagers = true;
    this.userService.getAll().subscribe({
      next: (users) => {
        this.managers = (users ?? []).filter(u => u.roleId === 2 && u.isActive);
        this.loadingManagers = false;
      },
      error: (err) => {
        console.error(err);
        this.loadingManagers = false;
        this.toastr.error('Failed to load managers.', 'Error');
      }
    });
  }

  // ============================================================
  // SUBMIT
  // ============================================================
  onSubmit(form: NgForm): void {
    if (form.invalid || !this.model.managerId) {
      if (!this.model.managerId) {
        this.toastr.error('Please select a manager.', 'Validation');
      }
      return;
    }

    this.isLoading = true;
    this.spinner.show('Creating team...');

    this.teamService.create(this.model).subscribe({
      next: (team) => {
        this.isLoading = false;
        this.spinner.forceHide();
        this.toastr.success(`Team "${team.name}" created.`, 'Success');
        this.router.navigate(['/admin/teams']);
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isLoading = false;
        this.spinner.forceHide();
        this.toastr.error(
          err?.error?.message || 'Failed to create team.',
          'Error'
        );
        this.cdr.detectChanges();
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/admin/teams']);
  }
}