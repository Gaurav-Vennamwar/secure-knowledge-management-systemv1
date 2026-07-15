import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth-service';
import { map } from 'rxjs';

export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService)
  const router = inject(Router)
  const authorise = (user: ReturnType<typeof authService.user>) => {
    if (!user || !user.Roles.includes('Writter')) {
      return router.createUrlTree(['/login']);
    }
    return true;
  };

  const existingUser = authService.user();
  if (existingUser) {
    return authorise(existingUser);
  }

  return authService.loadUser().pipe(
    map(user => {
      authService.user.set(user);
      return authorise(user);
    }),
  );

};
