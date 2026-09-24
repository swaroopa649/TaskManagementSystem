import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AccountService } from '../services/account.service';


@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
  constructor(private account: AccountService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const allowedRoles = route.data['roles'] as number[] | undefined;

    if (!allowedRoles || allowedRoles.length === 0) {
      return true;
    }

    const userRole = this.account.getUserRoleId();

    if (userRole !== null && allowedRoles.includes(userRole)) {
      return true;
    }

    // Not allowed — send them to their own dashboard
    const fallback = this.account.getRedirectPath();
    this.router.navigate([fallback]);
    return false;
  }
}