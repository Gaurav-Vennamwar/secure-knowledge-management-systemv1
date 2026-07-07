import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../../Features/Auth/services/auth-service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  // add withCredentials to every request automatically
  const authReq = req.clone({
    withCredentials: true,
  });

  return next(authReq).pipe(
    catchError((error) => {
      // if 401 and not already a refresh request
      if (
        error instanceof HttpErrorResponse &&
        error.status === 401 &&
        !req.url.includes('/api/auth/refresh') &&
        !req.url.includes('/api/auth/login') &&
        !req.url.includes('/api/blogpost')
      ) {
        // call refresh endpoint
        return authService.refreshToken().pipe(
          switchMap(() => {
            // retry original request after refresh
            return next(authReq);
          }),
          catchError((refreshError) => {
            // refresh failed → logout user
            authService.logout();
            return throwError(() => refreshError);
          }),
        );
      }
      return throwError(() => error);
    }),
  );
};
