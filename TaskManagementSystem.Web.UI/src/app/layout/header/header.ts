import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './header.html',
  styleUrls: ['./header.css']
})
export class HeaderComponent implements OnInit {

  userName = '';
  userRole = '';
  userInitials = '';
  avatarColor = '';

  // Simple notification counter (replace with real data later)
  notificationCount = 5;

  constructor(
    public account: AccountService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadUserInfo();
  }

  private loadUserInfo(): void {
    this.userName = this.account.getDisplayName();
    this.userRole = this.account.getRoleDisplayName();
    this.userInitials = this.account.getUserInitials();
    this.avatarColor = this.account.getAvatarColor();
  }

  // ============================================================
  // SIDEBAR TOGGLE (hamburger)
  // ============================================================
  toggleSidebar(): void {
    // flat-able uses a class on <body> called "sidebar-collapsed"
    // When present, the sidebar shrinks and the body content shifts.
    document.body.classList.toggle('sidebar-collapsed');

    // Also toggle the small-screen "mobile menu" class
    document.body.classList.toggle('mobile-menu-active');

    // Re-init pcoded so its internal layout recalculates
    this.reinitPcoded();
  }

  /**
   * Toggle mobile menu (used by the "more vertical" icon on small screens)
   */
  toggleMobileMenu(event: Event): void {
    event.preventDefault();
    const body = document.body;
    body.classList.toggle('mob-sidebar-active');
    this.reinitPcoded();
  }

  private reinitPcoded(): void {
    setTimeout(() => {
      const pcoded = (window as any).pcoded;
      if (pcoded && typeof pcoded.init === 'function') {
        try {
          pcoded.init();
        } catch (e) {
          // ignore — pcoded sometimes throws on partial re-init
        }
      }
    }, 50);
  }

  // ============================================================
  // SEARCH
  // ============================================================
  toggleSearch(event: Event): void {
    event.preventDefault();
    document.body.classList.toggle('search-bar-active');
  }

  onSearch(term: string): void {
    if (!term) return;
    console.log('Searching for:', term);
    // TODO: route to a search page or filter list
    this.router.navigate(['/search'], { queryParams: { q: term } });
  }

  // ============================================================
  // LOGOUT
  // ============================================================
  logout(event?: Event): void {
    event?.preventDefault();

    this.account.logout();
    this.toastr.success('You have been signed out.', 'Goodbye');

    this.router.navigate(['/login']).then(() => {
      // Ensure sidebar/body classes are cleaned up
      document.body.classList.remove('sidebar-collapsed', 'mobile-menu-active', 'mob-sidebar-active');
    });
  }
}