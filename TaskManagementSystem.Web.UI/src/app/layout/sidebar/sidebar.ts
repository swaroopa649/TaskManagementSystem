import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './sidebar.html',
  styleUrls: ['./sidebar.css']
})
export class SidebarComponent implements OnInit {

  userInitials = 'U';
  userName = '';
  userRole = '';
  avatarColor = '';

  openMenus = new Set<string>();

  constructor(
    public account: AccountService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userName = this.account.getDisplayName();
    this.userInitials = this.account.getUserInitials();
    this.userRole = this.account.getRoleDisplayName();
    this.avatarColor = this.account.getAvatarColor();
    this.autoExpand();
  }

  // ============================================================
  // DASHBOARD
  // ============================================================
  goToDashboard(): void {
    this.router.navigate([this.account.getRedirectPath()]);
  }

  isOnDashboard(): boolean {
    return this.router.url === this.account.getRedirectPath();
  }

  // ============================================================
  // PARENT MENU TOGGLE
  // IMPORTANT: do NOT preventDefault — the parent <a> has routerLink
  // so navigation must be allowed to complete.
  // ============================================================
  toggleMenu(name: string, event?: Event): void {
    if (this.openMenus.has(name)) {
      this.openMenus.delete(name);
    } else {
      this.openMenus.add(name);
    }
  }

  isOpen(name: string): boolean {
    return this.openMenus.has(name);
  }

  private autoExpand(): void {
    const url = this.router.url;

    if (url.startsWith('/admin/teams') || url.startsWith('/manager/team')) {
      this.openMenus.add('teams');
    }
    if (url.startsWith('/admin/users') || url.startsWith('/admin/roles')) {
      this.openMenus.add('users');
    }
  }

  // ============================================================
  // LOGOUT
  // ============================================================
  logout(): void {
    this.account.logout();
    this.router.navigate(['/login']);
  }
}