import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../../core/services/user.service';
import { TaskService } from '../../../core/services/task.service';
import { CommonModule } from '@angular/common';
import { Subject, takeUntil } from 'rxjs';
import { User } from '../../../core/models/user.model';
import { ToastService } from '../../../shared/services/toast.service';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-task-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './task-create.component.html'
})
export class TaskCreateComponent implements OnInit, OnDestroy {

  users: User[] = [];
  private destroy$ = new Subject<void>();

  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private taskService: TaskService,
    private toastService: ToastService,
    private router: Router
  ) {
    this.form = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      userId: [null, Validators.required],

      priority: ['Medium'],
      estimatedFinishDate: [''],
      tags: [''],
      metadata: ['']
    });
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res) => this.users = res,
        error: () => this.toastService.showError('Error cargando usuarios.')
      });
  }

  private buildExtraDataJson(): string | null {

    const priority = this.form.value.priority;
    const estimatedFinishDate = this.form.value.estimatedFinishDate;
    const tagsRaw = this.form.value.tags;
    const metadata = this.form.value.metadata;

    const tags = tagsRaw
      ? tagsRaw.split(',').map((x: string) => x.trim()).filter((x: string) => x.length > 0)
      : [];

    const extraDataObj: any = {
      priority: priority,
      estimatedFinishDate: estimatedFinishDate || null,
      tags: tags,
      metadata: metadata || null
    };
    
    const hasAnyValue =
      priority ||
      estimatedFinishDate ||
      tags.length > 0 ||
      metadata;

    if (!hasAnyValue) return null;

    return JSON.stringify(extraDataObj);
  }

  save() {
    if (this.form.invalid) {
      this.toastService.showError("Debe completar el título y seleccionar un usuario.");
      return;
    }

    const payload = {
      title: this.form.value.title,
      description: this.form.value.description,
      userId: Number(this.form.value.userId),
      extraData: this.buildExtraDataJson()
    };

    this.taskService.createTask(payload)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.toastService.showSuccess("Tarea creada correctamente.");
          this.form.reset({
            priority: 'Medium'
          });
          this.router.navigate(['/tasks']);
        },
        error: (err) => {
          this.toastService.showError(err?.error?.message || "Error creando tarea.");
        }
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
