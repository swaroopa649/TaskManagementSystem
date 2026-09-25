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

// Teams feature
import { TeamListComponent } from './dashboards/admin/team/team-list';
import { AddTeamComponent } from './dashboards/admin/team/add-team';
import { AddTeamMemberComponent } from './dashboards/admin/team/add-team-member';
import { TaskListComponent } from './dashboards/admin/task/task-list';
import { AddTaskComponent } from './dashboards/admin/task/add-task';
import { TaskDetailsComponent } from './dashboards/admin/task/task-details';
import { AssignTaskComponent } from './dashboards/admin/task/assign-task';


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
      {
        path: 'tasks',
        component: TaskListComponent,
        data: { scope: 'all', title: 'All Tasks' }
      },
      { path: 'tasks/create',       component: AddTaskComponent },
      { path: 'tasks/edit/:id',     component: AddTaskComponent },
      { path: 'tasks/:id',          component: TaskDetailsComponent },

      // ----- Team Management -----
      { path: 'teams',                     component: TeamListComponent },
      { path: 'teams/create',              component: AddTeamComponent },
      { path: 'teams/edit/:id',            component: AddTeamComponent },
      { path: 'teams/:id/members/add',     component: AddTeamMemberComponent },
      { path: 'teams/:id',                 component: PlaceholderComponent },
      { path: 'members',                   component: PlaceholderComponent },

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
      {
        path: 'tasks',
        component: TaskListComponent,
        data: { scope: 'team', title: 'Team Tasks' }
      },
      { path: 'tasks/create',       component: AddTaskComponent },
      { path: 'tasks/edit/:id',     component: AddTaskComponent },
      { path: 'tasks/:id',          component: TaskDetailsComponent },

      // ----- Team Management -----
      { path: 'team',                     component: TeamListComponent },
      { path: 'team/:id/members/add',     component: AddTeamMemberComponent },

      // ----- Assign Task -----
      { path: 'assign',             component: AssignTaskComponent }
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
      {
        path: 'tasks',
        component: TaskListComponent,
        data: { scope: 'my', title: 'My Tasks' }
      },
      {
        path: 'tasks/assigned',
        component: TaskListComponent,
        data: { scope: 'assigned', title: 'Assigned to Me' }
      },
      { path: 'tasks/:id',          component: TaskDetailsComponent }
    ]
  },

  // ============================================================
  // ROOT & FALLBACK
  // ============================================================
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' }
];