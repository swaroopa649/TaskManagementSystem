import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { UserService } from '../../../services/user.service';
import { CreateUserDto } from '../../../models/user.model';
import { SpinnerLoadingService } from '../../../services/spinner-loading-service';

@Component({
  selector: 'app-add-user',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './add-user.html',
  styleUrls: ['./add-user.css']
})
export class AddUserComponent {

  model: CreateUserDto = {
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    roleId: null,
    password: '',
    isActive: true
  };

  confirmPassword = '';
  showPassword = false;
  isLoading = false;

  constructor(
    private userService: UserService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: SpinnerLoadingService
  ) {}

  // ============================================================
  // SUBMIT
  // ============================================================
  onSubmit(form: NgForm): void {
    // Basic checks beyond template validation
    if (this.model.password !== this.confirmPassword) {
      this.toastr.error('Passwords do not match.', 'Validation');
      return;
    }

    if (!this.model.roleId) {
      this.toastr.error('Please select a role.', 'Validation');
      return;
    }

    if (this.isLoading) return;
    this.isLoading = true;
    this.spinner.show('Creating user...');

    this.userService.create(this.model).subscribe({
      next: (created) => {
        this.isLoading = false;
        this.spinner.hide();
        this.toastr.success(`User "${created.firstName} ${created.lastName}" created.`, 'Success');
        this.router.navigate(['/admin/users']);
      },
      error: (err) => {
        this.isLoading = false;
        this.spinner.hide();

        let msg = 'Failed to create user.';
        if (err?.status === 409) {
          msg = err.error || 'A user with that email already exists.';
        } else if (err?.status === 400) {
          msg = err.error?.message || 'Invalid input. Please check the form.';
        }
        this.toastr.error(msg, 'Error');
      }
    });
  }

  // ============================================================
  // UI HELPERS
  // ============================================================
  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  cancel(): void {
    this.router.navigate(['/admin/users']);
  }
}