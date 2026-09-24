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

  constructor(
    public account: AccountService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userName = this.account.getDisplayName();
    this.userInitials = this.account.getUserInitials();
    this.userRole = this.account.getRoleDisplayName();
    this.avatarColor = this.account.getAvatarColor();
  }

  goToDashboard(): void {
    this.router.navigate([this.account.getRedirectPath()]);
  }

  logout(): void {
    this.account.logout();
    this.router.navigate(['/login']);
  }
}