import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

import { AccountService } from '../../services/account.service';
import { AuthResponse } from '../../models/auth.response';
import { SpinnerLoadingService } from '../../services/spinner-loading-service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent implements OnInit, OnDestroy {

  // Form model — username + password
  userName = '';
  password = '';
  rememberMe = false;

  // UI state
  isLoading = false;
  showPassword = false;
  errorMessage = '';
  userNameError = '';
  passwordError = '';

  private loginSubscription?: Subscription;

  constructor(
    private accountService: AccountService,
    private router: Router,
    private toastr: ToastrService,
    private spinnerService: SpinnerLoadingService
  ) {}

  ngOnInit(): void {
    // If already authenticated, send the user straight to their dashboard
    if (this.accountService.isAuthenticated()) {
      this.redirectBasedOnRole();
      return;
    }

    // Restore remembered username
    const savedUsername = localStorage.getItem('savedUsername');
    if (savedUsername) {
      this.userName = savedUsername;
      this.rememberMe = true;
    }
  }

  ngOnDestroy(): void {
    this.loginSubscription?.unsubscribe();
    this.spinnerService.hide();
  }

  // ============================================================
  // SUBMIT
  // ============================================================
  signin(): void {
    this.clearErrors();

    const userName = this.userName?.trim() ?? '';
    const password = this.password?.trim() ?? '';

    if (!userName) {
      this.showError('userNameError', 'Please enter your username.');
      return;
    }

    if (userName.length < 3) {
      this.showError('userNameError', 'Username must be at least 3 characters.');
      return;
    }

    if (!password || password.length < 6) {
      this.showError('passwordError', 'Password must be at least 6 characters.');
      return;
    }

    if (this.isLoading) return;

    this.isLoading = true;
    this.spinnerService.show('Signing you in...');

    const credentials = { userName, password };

    this.loginSubscription?.unsubscribe();
    this.loginSubscription = this.accountService.authenticateAsync(credentials).subscribe({
      next: (res: AuthResponse) => {
        this.isLoading = false;
        this.spinnerService.hide();

        if (res?.isValidUser && res?.isValidPassword && res?.jwtToken) {
          if (this.rememberMe) {
            localStorage.setItem('savedUsername', userName);
          } else {
            localStorage.removeItem('savedUsername');
          }

          this.toastr.success('Login successful!', 'Success');
          this.redirectBasedOnRole();
        } else {
          const msg = res?.message || 'Invalid username or password.';
          this.errorMessage = msg;
          this.toastr.error(msg, 'Login Failed');
        }
      },
      error: (err: any) => {
        this.isLoading = false;
        this.spinnerService.hide();
        console.error('Login failed:', err);

        let errorMsg = 'Login failed. Please try again.';
        if (err?.status === 0) {
          errorMsg = 'Unable to reach the server. Check your network.';
        } else if (err?.status === 401) {
          errorMsg = 'Invalid username or password.';
        } else if (err?.status === 403) {
          errorMsg = 'Access forbidden. Contact support.';
        } else if (err?.error?.message) {
          errorMsg = err.error.message;
        }

        this.errorMessage = errorMsg;
        this.toastr.error(errorMsg, 'Login Failed');
      }
    });
  }

  // ============================================================
  // ROLE-BASED REDIRECT  (roleId: 1=Admin, 2=Manager, 3=User)
  // ============================================================
  private redirectBasedOnRole(): void {
    const roleId = this.accountService.getUserRoleId();
    const path = this.accountService.getRedirectPath(); // /admin/dashboard etc.

    switch (roleId) {
      case 1:
        this.toastr.info('Welcome, Administrator', 'Signed In');
        break;
      case 2:
        this.toastr.info('Welcome, Manager', 'Signed In');
        break;
      case 3:
        this.toastr.info('Welcome', 'Signed In');
        break;
      default:
        this.toastr.warning('Unknown role. Redirecting to home.', 'Notice');
        this.router.navigate(['/']);
        return;
    }

    this.router.navigate([path]).catch(err => {
      console.error('Navigation failed:', err);
      window.location.href = path;
    });
  }

  // ============================================================
  // UI HELPERS
  // ============================================================
  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  onForgotPassword(event: Event): void {
    event.preventDefault();
    this.router.navigate(['/forgot-password']).catch(err => {
      console.error('Navigation error:', err);
    });
  }

  private clearErrors(): void {
    this.userNameError = '';
    this.passwordError = '';
    this.errorMessage = '';
  }

  private showError(
    field: 'userNameError' | 'passwordError' | 'errorMessage',
    message: string
  ): void {
    this[field] = message;

    setTimeout(() => {
      if (this[field] === message) {
        this[field] = '';
      }
    }, 5000);
  }
}