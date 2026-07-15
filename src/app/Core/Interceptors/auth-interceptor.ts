import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, finalize, Observable, shareReplay, switchMap, throwError } from 'rxjs';
import { AuthService } from '../../Features/Auth/services/auth-service';

let refreshRequest$: Observable<void> | null = null;

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const authReq = req.clone({ withCredentials: true });

  return next(authReq).pipe(
    catchError((error) => {
      const canRefresh = error instanceof HttpErrorResponse &&
        error.status === 401 &&
        !req.url.includes('/api/auth/refresh') &&
        !req.url.includes('/api/auth/login') &&
        !req.url.includes('/api/auth/me') &&
        !req.url.includes('/api/blogpost');

      if (!canRefresh) {
        return throwError(() => error);
      }

      if (!refreshRequest$) {
        refreshRequest$ = authService.refreshToken().pipe(
          finalize(() => refreshRequest$ = null),
          shareReplay(1),
        );
      }

      return refreshRequest$.pipe(
        switchMap(() => next(authReq)),
        catchError((refreshError) => {
          authService.logout();
          return throwError(() => refreshError);
        }),
      );
    }),
  );
};
