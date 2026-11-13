import { Component, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
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
  isSubmitting = signal(false);
  submitError = signal<string | null>(null);

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
    this.submitError.set(null);
    if (this.loginFormGroup.invalid) {
      this.loginFormGroup.markAllAsTouched();
      return;
    }

    const formRawValue = this.loginFormGroup.getRawValue();
    this.isSubmitting.set(true);
    this.authService.login(formRawValue.email, formRawValue.password).subscribe({
      next: (response) => {
        this.router.navigate([''])//navigate to home page
      },
      error: (error) => {
        this.isSubmitting.set(false);
        this.submitError.set(error?.error?.title || 'We could not sign you in. Check your email and password, then try again.');
      },
    });
  }
}
