import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';
import { ToastService } from '../../../shared/services/toast.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html'
})
export class LoginComponent {

  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private toastService: ToastService
  ) {
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  login() {
    if (this.form.invalid) {
      this.toastService.showError("Debe ingresar usuario y contraseña.");
      return;
    }

    const username = this.form.value.username;
    const password = this.form.value.password;

    this.authService.login(username, password).subscribe({
      next: () => {
        this.toastService.showSuccess("Bienvenido al sistema.");
        this.router.navigate(['/tasks']);
      },
      error: () => {
        this.toastService.showError("Credenciales incorrectas.");
      }
    });
  }
}
