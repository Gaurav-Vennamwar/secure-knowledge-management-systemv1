import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../Features/Auth/services/auth-service';

@Component({
  selector: 'app-navbar',
  imports: [RouterModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  authService = inject(AuthService);

  get userDisplayName(): string {
    const email = this.authService.user()?.Email ?? '';

    const username = email.split('@')[0];

    return username.charAt(0).toUpperCase() + username.slice(1);
  }

  get userName(): string {
    return this.authService.user()?.Email?.split('@')[0] ?? 'User';
  }

  onLogout() {
    this.authService.logout();
  }
}
