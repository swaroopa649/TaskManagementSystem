import { Routes } from '@angular/router';

import { LoginComponent } from './auth/login/login';
import { MainLayoutComponent } from './layout/main-layout/main-layout';

import { RoleIds } from './models/role-ids';
import { AuthGuard } from './guard/auth-guard';
import { RoleGuard } from './guard/role.guard';

// Dashboards
import { AdminDashboardComponent } from './dashboards/admin/admin-dashboard';
import { ManagerDashboardComponent } from './dashboards/manager/manager-dashboard';
import { UserDashboardComponent } from './dashboards/user/user-dashboard';

// Shared placeholder
import { PlaceholderComponent } from './dashboards/shared/placeholder/placeholder';

// Users feature
import { UserListComponent } from './dashboards/admin/user/user-list';
import { AddUserComponent } from './dashboards/admin/user/add-user';
import { AddTeamComponent } from './dashboards/admin/team/add-team';
import { TeamListComponent } from './dashboards/admin/team/team-list';
import { AddTeamMemberComponent } from './dashboards/admin/team/add-team-member';


export const routes: Routes = [

  // ============================================================
  // PUBLIC
  // ============================================================
  { path: 'login',           component: LoginComponent },
  { path: 'forgot-password', component: PlaceholderComponent },

  // ============================================================
  // SHARED — all authenticated users (no role restriction)
  // ============================================================
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'tasks/board',     component: PlaceholderComponent },
      { path: 'comments',        component: PlaceholderComponent },
      { path: 'notifications',   component: PlaceholderComponent },
      { path: 'profile',         component: PlaceholderComponent },
      { path: 'change-password', component: PlaceholderComponent }
    ]
  },

  // ============================================================
  // ADMIN
  // ============================================================
  {
    path: 'admin',
    component: MainLayoutComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [RoleIds.ADMIN] },
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

      // Dashboard
      { path: 'dashboard',          component: AdminDashboardComponent },

      // ----- Task Management -----
      { path: 'tasks',              component: PlaceholderComponent },
      { path: 'tasks/create',       component: PlaceholderComponent },
      { path: 'tasks/edit/:id',     component: PlaceholderComponent },
      { path: 'tasks/:id',          component: PlaceholderComponent },

      // ----- Team Management -----
      { path: 'teams',                            component: TeamListComponent },
      { path: 'teams/create',                     component: AddTeamComponent },
      { path: 'teams/edit/:id',                   component: AddTeamComponent },
      { path: 'teams/:id/members/add',            component: AddTeamMemberComponent },
      { path: 'teams/:id',                        component: PlaceholderComponent },
      { path: 'members',                          component: PlaceholderComponent },

      // ----- Users -----
      { path: 'users',              component: UserListComponent },
      { path: 'users/create',       component: AddUserComponent },
  

      // ----- Roles & Reports -----
      { path: 'roles',              component: PlaceholderComponent },
      { path: 'reports',            component: PlaceholderComponent },

      // ----- Settings -----
      { path: 'settings',           component: PlaceholderComponent }
    ]
  },

  // ============================================================
  // MANAGER
  // ============================================================
  {
    path: 'manager',
    component: MainLayoutComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [RoleIds.MANAGER] },
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

      // Dashboard
      { path: 'dashboard',          component: ManagerDashboardComponent },

      // ----- Task Management -----
      { path: 'tasks',              component: PlaceholderComponent },
      { path: 'tasks/create',       component: PlaceholderComponent },
      { path: 'tasks/edit/:id',     component: PlaceholderComponent },
      { path: 'tasks/:id',          component: PlaceholderComponent },

      // ----- Team Management -----
      { path: 'team',                             component: TeamListComponent },
      { path: 'team/:id/members/add',             component: AddTeamMemberComponent },
      { path: 'assign',                           component: PlaceholderComponent }
    ]
  },

  // ============================================================
  // USER
  // ============================================================
  {
    path: 'user',
    component: MainLayoutComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [RoleIds.USER] },
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

      // Dashboard
      { path: 'dashboard',          component: UserDashboardComponent },

      // ----- Task Management -----
      { path: 'tasks',              component: PlaceholderComponent },
      { path: 'tasks/assigned',     component: PlaceholderComponent },
      { path: 'tasks/:id',          component: PlaceholderComponent }
    ]
  },

  // ============================================================
  // ROOT & FALLBACK
  // ============================================================
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' }
];