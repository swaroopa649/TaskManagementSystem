import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from '../services/api.service';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const api = inject(ApiService);
  const router = inject(Router);
  const toastr = inject(ToastrService);

  const token = api.getToken();

  let authReq = req;
  if (token) {
    authReq = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      const isLoginCall = req.url.toLowerCase().includes('/account/login');

      if (error.status === 401 && !isLoginCall) {
        console.warn('Unauthorized — clearing session');
        api.clearAuthData();
        toastr.error('Your session has expired. Please login again.', 'Session Expired');
        router.navigate(['/login']);
      } else if (error.status === 403) {
        toastr.error('You do not have permission to perform this action.', 'Access Denied');
      } else if (error.status === 500) {
        toastr.error('An internal server error occurred. Please try again later.', 'Server Error');
      }

      return throwError(() => error);
    })
  );
};