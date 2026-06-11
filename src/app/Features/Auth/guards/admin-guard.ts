import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth-service';

export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService)
  const router = inject(Router)
  const user = authService.user();

  //if user is not loggen in
  if(!user){
    //navigate back to login page
    router.navigate(['/login']);
    return false;
  }

  //if the upper if skips then the user is logged in 
  //then check the role of the user
  const isWritter = user.Roles.includes("writter");

  //if its not a writter
  if(!isWritter){
    authService.logout();
    return false;
  }

  //cuz now we know the user is logged in and is a writter
  return true;

};
