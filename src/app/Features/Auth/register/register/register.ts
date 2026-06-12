import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth-service';

@Component({
  selector: 'app-register',
  imports: [RouterLink,ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  authService = inject(AuthService);
  router = inject(Router);

  registerFormGroup = new FormGroup({
    email: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.email],
    }),
    password: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.minLength(6)],
    }),
    confirmPassword: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required],
    }),
  });

  get emailFormControl() { return this.registerFormGroup.controls.email; }
  get passwordFormControl() { return this.registerFormGroup.controls.password; }
  get confirmPasswordFormControl() { return this.registerFormGroup.controls.confirmPassword; }


  onSubmit() {
    if (this.registerFormGroup.invalid) return;

    const { email, password, confirmPassword } = this.registerFormGroup.getRawValue();

    // check passwords match
    if (password !== confirmPassword) {
      alert('Passwords do not match!');
      return;
    }

    this.authService.register(email, password).subscribe({
      next: () => {
        this.router.navigate(['/login']); // ← redirect to login after register
      },
      error: (error) => {
        console.error(error);
      }
    });
  }
}
  

