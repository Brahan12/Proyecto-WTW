import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../../core/services/user.service';
import { CommonModule } from '@angular/common';
import { ToastService } from '../../../shared/services/toast.service';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-user-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './user-create.component.html'
})
export class UserCreateComponent {

  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private toastService: ToastService,
    private router: Router,
  ) {
    this.form = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  save() {
    if (this.form.invalid) {
      this.toastService.showError("Complete los campos obligatorios correctamente.");
      return;
    }

    this.userService.createUser(this.form.value as any).subscribe({
      next: () => {
        this.toastService.showSuccess("Usuario creado correctamente.");
        this.form.reset();
        this.router.navigate(['/users']);
      },
      error: (err) => {
        this.toastService.showError(err?.error?.message || "Error creando usuario.");
      }
    });
  }
}
