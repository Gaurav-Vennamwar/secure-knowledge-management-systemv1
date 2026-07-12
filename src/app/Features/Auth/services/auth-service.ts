import {
  HttpClient,
  httpResource,
  HttpResourceRef,
  HttpResourceRequest,
} from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { catchError, Observable, of, tap } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { LoginResponse, User } from '../models/auth.model';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  http = inject(HttpClient);
  user = signal<User | null>(null);

  router = inject(Router);

  //this is the method which we will call our endpoint me
  // loadUser(): HttpResourceRef<User | undefined> {
  //   return httpResource<User>(() => {
  //     const request: HttpResourceRequest = {
  //       url: `${environment.apiBaseUrl}/api/auth/me`,
  //       withCredentials: true,
  //     };
  //     return request;
  //   });
  // }
  loadUser(): Observable<User | null> {
  return this.http.get<User>(
    `${environment.apiBaseUrl}/api/auth/me`,
    { withCredentials: true }
  ).pipe(
    catchError(() => of (null)) // ← 401 returns null, no crash
  );
}

  login(email: string, password: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(
        `${environment.apiBaseUrl}/api/auth/login`,
        {
          email: email,
          password: password,
        },
        {
          withCredentials: true,
        },
      )
      .pipe(
        tap({
          next: (response) => {
            this.user.set(response);
            localStorage.setItem(
              'user',
              JSON.stringify({
                Email: response.Email,
                Roles: response.Roles,
              }),
            );
          },
        }),
      );
  }

  logout() {
    //api:auth/logout calling the api
    this.http
      .post<void>(
        `${environment.apiBaseUrl}/api/auth/logout`,
        {},
        {
          withCredentials: true,
        },
      )
      .subscribe({
        next: () => {
          //clear out the user signal
          this.user.set(null);
          //redirect to the home page bro
          this.router.navigate(['']);
        },
      });
  }
  register(email: string, password: string): Observable<void> {
    return this.http.post<void>(`${environment.apiBaseUrl}/api/auth/register`, {
      email,
      password,
    });

  }
  refreshToken(): Observable<void> {
  return this.http.post<void>(
    `${environment.apiBaseUrl}/api/auth/refresh`,
    {},
    { withCredentials: true }
    // ← withCredentials needed here so refresh_token 
    //    cookie is sent to backend
  );
}
}
