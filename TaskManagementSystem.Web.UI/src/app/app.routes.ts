import { Routes } from '@angular/router';
import { BodyComponent } from './layout/body/body';
import { LoginComponent } from './auth/login/login';
import { MainLayoutComponent } from './layout/main-layout/main-layout';
import { AuthGuard } from './guard/auth-guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },

  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [AuthGuard],           // ← THIS IS THE KEY
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: BodyComponent },
    ]
  },

  { path: '**', redirectTo: 'login' }
];