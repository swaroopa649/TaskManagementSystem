import { Routes } from '@angular/router';

import { LoginComponent } from './auth/login/login';


import { MainLayoutComponent } from './layout/main-layout/main-layout';


import { RoleIds } from './models/role-ids';
import { AuthGuard } from './guard/auth-guard';
import { RoleGuard } from './guard/role.guard';
import { AdminDashboardComponent } from './dashboards/admin/admin-dashboard';
import { ManagerDashboardComponent } from './dashboards/manager/manager-dashboard';
import { UserDashboardComponent } from './dashboards/user/user-dashboard';

export const routes: Routes = [

  // ---------- Public ----------
  { path: 'login', component: LoginComponent },

  // ---------- Admin ----------
  {
    path: 'admin',
    component: MainLayoutComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [RoleIds.ADMIN] },
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: AdminDashboardComponent }
      // { path: 'users', component: AdminUsersComponent },
      // { path: 'settings', component: AdminSettingsComponent },
    ]
  },

  // ---------- Manager ----------
  {
    path: 'manager',
    component: MainLayoutComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [RoleIds.MANAGER] },
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: ManagerDashboardComponent }
    ]
  },

  // ---------- User ----------
  {
    path: 'user',
    component: MainLayoutComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [RoleIds.USER] },
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: UserDashboardComponent }
    ]
  },

  // ---------- Root & fallback ----------
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' }
];