import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { AuthResponse, ApplicationUser } from '../models/auth.response';
import { ApiService } from './api.service';

// --- Request DTOs ---
export interface ForgotPasswordRequest {
  email: string;
}

export interface ResetPasswordRequest {
  token: string;
  newPassword: string;
  confirmPassword?: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}

// --- Role IDs (must match backend enum) ---
export const RoleIds = {
  ADMIN: 1,
  MANAGER: 2,
  USER: 3
} as const;

export type RoleId = typeof RoleIds[keyof typeof RoleIds];

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  // Auth state (boolean)
  private authState = new BehaviorSubject<boolean>(false);
  authState$ = this.authState.asObservable();

  // Current user
  private currentUserSubject = new BehaviorSubject<ApplicationUser | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(private apiService: ApiService) {
    this.checkAndUpdateAuthState();
  }

  // ==================== AUTHENTICATION ====================

  /**
   * Authenticate user with credentials.
   */
  authenticateAsync(credentials: { userName: string; password: string }): Observable<AuthResponse> {
    return this.apiService
      .send<AuthResponse>('POST', 'Account/Authenticate', credentials)
      .pipe(
        tap((response: AuthResponse) => {
          if (response?.isValidUser && response?.isValidPassword && response?.jwtToken) {
            this.apiService.setAuthData(response);
            this.authState.next(true);
            this.currentUserSubject.next(response.applicationUser);
          } else {
            this.authState.next(false);
            this.currentUserSubject.next(null);
          }
        }),
        catchError((error) => {
          this.authState.next(false);
          this.currentUserSubject.next(null);
          console.error('Authentication failed:', error);
          return throwError(() => error);
        })
      );
  }

  /**
   * Synchronous auth check (reads localStorage).
   */
  isAuthenticated(): boolean {
    const user = this.getCurrentUser();
    const isValid = !!user && !!this.getToken();
    this.authState.next(isValid);
    return isValid;
  }

  /**
   * Get auth state synchronously.
   */
  get isAuthenticatedSync(): boolean {
    return this.authState.value;
  }

  /**
   * Logout — clear storage and notify subscribers.
   */
  logout(): void {
    this.apiService.clearAuthData();
    this.authState.next(false);
    this.currentUserSubject.next(null);
    console.log('User logged out');
  }

  // ==================== STORAGE ACCESS ====================

  /**
   * Get the full auth response from storage.
   */
  getAuthResponse(): AuthResponse | null {
    const raw = localStorage.getItem('AuthenticatedUserResponse');
    if (!raw) return null;
    try {
      return JSON.parse(raw) as AuthResponse;
    } catch (error) {
      console.error('Failed to parse auth response:', error);
      return null;
    }
  }

  /**
   * Get the current user (ApplicationUser) from storage.
   */
  getCurrentUser(): ApplicationUser | null {
    return this.getAuthResponse()?.applicationUser ?? null;
  }

  /**
   * Get the JWT token.
   */
  getToken(): string | null {
    return this.getAuthResponse()?.jwtToken ?? null;
  }

  // ==================== USER INFO ====================

  getUserId(): number | null {
    return this.getCurrentUser()?.userId ?? null;
  }

  getFirstName(): string {
    return this.getCurrentUser()?.firstName ?? '';
  }

  getLastName(): string {
    return this.getCurrentUser()?.lastName ?? '';
  }

  getUserFullName(): string {
    const first = this.getFirstName();
    const last = this.getLastName();
    if (first && last) return `${first} ${last}`;
    return first || last || '';
  }

  getUserEmail(): string {
    return this.getCurrentUser()?.email ?? '';
  }

  getUserPhone(): string {
    return this.getCurrentUser()?.phone ?? '';
  }

  getUserRoleId(): number | null {
    return this.getCurrentUser()?.roleId ?? null;
  }

  getUserRoleName(): string {
    return this.getCurrentUser()?.roleInfo?.name ?? '';
  }

  getUserRoleCode(): string {
    return this.getCurrentUser()?.roleInfo?.code ?? '';
  }

  // ==================== ROLE CHECKS (ID-BASED) ====================

  hasRole(roleId: number): boolean {
    return this.getUserRoleId() === roleId;
  }

  isAdmin(): boolean {
    return this.hasRole(RoleIds.ADMIN);
  }

  isManager(): boolean {
    return this.hasRole(RoleIds.MANAGER);
  }

  isUser(): boolean {
    return this.hasRole(RoleIds.USER);
  }

  /**
   * Legacy fallback: match by role name (in case role IDs shift).
   */
  hasRoleByName(roleName: string): boolean {
    const current = this.getUserRoleName().toLowerCase();
    return current === roleName.toLowerCase();
  }

  hasAnyRole(roleIds: number[]): boolean {
    const id = this.getUserRoleId();
    return id !== null && roleIds.includes(id);
  }

  hasAdminOrManagerAccess(): boolean {
    return this.hasAnyRole([RoleIds.ADMIN, RoleIds.MANAGER]);
  }

  // ==================== ACCESS LEVEL ====================

  getAccessLevel(): 'admin' | 'manager' | 'user' | 'none' {
    if (this.isAdmin()) return 'admin';
    if (this.isManager()) return 'manager';
    if (this.isUser()) return 'user';
    return 'none';
  }

  // ==================== NAVIGATION HELPERS ====================

  getBasePath(): string {
    if (this.isAdmin()) return '/admin';
    if (this.isManager()) return '/manager';
    if (this.isUser()) return '/user';
    return '';
  }

  getDashboardPath(): string {
    const base = this.getBasePath();
    return base ? `${base}/dashboard` : '/login';
  }

  getRedirectPath(): string {
    return this.getDashboardPath();
  }

  isValidUser(): boolean {
    return this.isAuthenticatedSync && this.getAccessLevel() !== 'none';
  }

  // ==================== DISPLAY HELPERS ====================

  getDisplayName(): string {
    const full = this.getUserFullName();
    if (full) return full;
    return this.getUserEmail() || 'User';
  }

  getUserInitials(): string {
    const f = this.getFirstName().charAt(0);
    const l = this.getLastName().charAt(0);
    return (f + l).toUpperCase() || 'U';
  }

  getRoleDisplayName(): string {
    switch (this.getUserRoleId()) {
      case RoleIds.ADMIN:   return 'Administrator';
      case RoleIds.MANAGER: return 'Manager';
      case RoleIds.USER:    return 'User';
      default:              return 'User';
    }
  }

  getRoleBadgeClass(): string {
    switch (this.getUserRoleId()) {
      case RoleIds.ADMIN:   return 'bg-danger text-white';
      case RoleIds.MANAGER: return 'bg-warning text-dark';
      case RoleIds.USER:    return 'bg-success text-white';
      default:              return 'bg-secondary text-white';
    }
  }

  getAvatarColor(): string {
    switch (this.getUserRoleId()) {
      case RoleIds.ADMIN:   return 'linear-gradient(135deg, #dc3545, #c82333)';
      case RoleIds.MANAGER: return 'linear-gradient(135deg, #ffc107, #e0a800)';
      case RoleIds.USER:    return 'linear-gradient(135deg, #0d9488, #0e7490)';
      default:              return 'linear-gradient(135deg, #6c757d, #5a6268)';
    }
  }

  // ==================== PASSWORD FLOWS ====================

  forgotPassword(email: string): Observable<any> {
    const request: ForgotPasswordRequest = { email };
    return this.apiService.send<any>('POST', 'Account/forgot-password', request).pipe(
      tap(() => console.log('Forgot password request sent')),
      catchError((error) => {
        console.error('Forgot password failed:', error);
        return throwError(() => error);
      })
    );
  }

  resetPassword(payload: ResetPasswordRequest): Observable<any> {
    return this.apiService.send<any>('POST', 'Account/reset-password', payload);
  }

  validateResetToken(token: string): Observable<any> {
    if (!token) {
      return throwError(() => new Error('Token is required'));
    }
    return this.apiService.send<any>('POST', 'Account/validate-reset-token', { token });
  }

  changePassword(currentPassword: string, newPassword: string): Observable<any> {
    if (!currentPassword || !newPassword) {
      return throwError(() => new Error('Current and new password are required'));
    }
    const request: ChangePasswordRequest = { currentPassword, newPassword };
    return this.apiService.send<any>('POST', 'Account/change-password', request);
  }

  // ==================== INTERNAL ====================

  private checkAndUpdateAuthState(): void {
    const user = this.getCurrentUser();
    const valid = !!user && !!this.getToken();
    this.authState.next(valid);
    this.currentUserSubject.next(valid ? user : null);
  }

  
}