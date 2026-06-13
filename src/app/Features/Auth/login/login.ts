import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { email } from '@angular/forms/signals';
import { AuthService } from '../services/auth-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  authService = inject(AuthService);
  router = inject(Router)

  loginFormGroup = new FormGroup({
    email: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required],
    }),
    password: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required],
    }),
  });

  get emailFormControl() {
    return this.loginFormGroup.controls.email;
  }
  get passwordFormControl() {
    return this.loginFormGroup.controls.password;
  }

  onSubmit() {
    const formRawValue = this.loginFormGroup.getRawValue();
    this.authService.login(formRawValue.email, formRawValue.password).subscribe({
      next: (response) => {
        console.log('LOGIN RESPONSE:', response);
        this.router.navigate([''])//navigate to home page
      },
      error: (error) => {
        console.error(error);
      },
    });
  }
}
