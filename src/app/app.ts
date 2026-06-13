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

  loadUserRef = this.authService.loadUser();
  //when we get the value back it will be signal
  user = this.loadUserRef.value;

  effectRef = effect(() => {
    const userValue = this.user();
    if(userValue){
      this.authService.user.set(userValue)
    }
  })
}
