import { Component, effect, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Navbar } from "./Core/Components/navbar/navbar";
import { ImageSelector } from './Shared/Components/image-selector/image-selector';
import { AuthService } from './Features/Auth/services/auth-service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Navbar,ImageSelector],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('SecureKnowledgeManagement-v1');
  authService = inject(AuthService);

  constructor() {
    // load user on app start
    this.authService.loadUser().subscribe({
      next: (user) => {
        if (user) {
          this.authService.user.set(user);
        }
      }
    });
  }
}