import { Component, OnDestroy, OnInit } from '@angular/core';
import { TaskService } from '../../../core/services/task.service';
import { UserService } from '../../../core/services/user.service';
import { CommonModule } from '@angular/common';
import { Subject, takeUntil } from 'rxjs';
import { TaskItem } from '../../../core/models/task.model';
import { ConfirmModalComponent } from '../../../shared/components/confirm-modal/confirm-modal.component';
import { ToastService } from '../../../shared/services/toast.service';
import { User } from '../../../core/models/user.model';
import { RouterLink } from '@angular/router';
import { TaskDetailModalComponent } from '../../../shared/components/task-detail-modal/task-detail-modal.component';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, ConfirmModalComponent, RouterLink, TaskDetailModalComponent],
  templateUrl: './task-list.component.html'
})
export class TaskListComponent implements OnInit, OnDestroy {

  tasks: TaskItem[] = [];
  users: User[] = [];

  userMap: { [key: number]: User } = {};

  statusFilter: string = '';
  priorityFilter: string = '';

  page = 1;
  pageSize = 5;

  private destroy$ = new Subject<void>();

  // Modal
  showModal = false;
  selectedTask: TaskItem | null = null;
  selectedStatus: string = '';
  showDetailModal = false;
  selectedTaskDetail: any = null;
  selectedUserLabel = '';

  constructor(
    private taskService: TaskService,
    private userService: UserService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loadUsers();
    this.loadTasks();
  }

  loadUsers() {
    this.userService.getUsers()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res) => {
          this.users = res;

          this.userMap = {};
          res.forEach(u => this.userMap[u.id] = u);
        },
        error: () => this.toastService.showError("Error cargando usuarios.")
      });
  }

  loadTasks() {
    this.taskService.getTasks(this.statusFilter, this.priorityFilter)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res) => this.tasks = res,
        error: (err) => this.toastService.showError(err?.error?.message || 'Error cargando tareas.')
      });
  }

  getUserLabel(userId: number): string {
  const user = this.userMap[userId];
  return user ? `${user.fullName} (${user.email})` : `Usuario #${userId}`;
}

  changeFilter(value: string) {
    this.statusFilter = value;
    this.page = 1;
    this.loadTasks();
  }

  changePriority(value: string) {
    this.priorityFilter = value;
    this.page = 1;
    this.loadTasks();
  }

  onFilterChange(event: Event) {
  const value = (event.target as HTMLSelectElement).value;
  this.changeFilter(value);
}

  openConfirmModal(task: TaskItem, newStatus: string) {
    this.selectedTask = task;
    this.selectedStatus = newStatus;
    this.showModal = true;
  }

  confirmChangeStatus() {
    if (!this.selectedTask) return;

    this.taskService.updateStatus(this.selectedTask.id, this.selectedStatus)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.toastService.showSuccess("Estado actualizado correctamente.");
          this.loadTasks();
          this.closeModal();
        },
        error: (err) => {
          this.toastService.showError(err?.error?.message || "Error actualizando estado.");
          this.closeModal();
        }
      });
  }

  closeModal() {
    this.showModal = false;
    this.selectedTask = null;
    this.selectedStatus = '';
  }

  openDetailModal(task: any) {
    this.selectedTaskDetail = task;
    this.selectedUserLabel = this.getUserLabel(task.userId);
    this.showDetailModal = true;
  }

  closeDetailModal() {
    this.showDetailModal = false;
    this.selectedTaskDetail = null;
    this.selectedUserLabel = '';
  }

  get paginatedTasks(): TaskItem[] {
    const start = (this.page - 1) * this.pageSize;
    return this.tasks.slice(start, start + this.pageSize);
  }

  onStatusChange(event: Event) {
    const value = (event.target as HTMLSelectElement).value;
    this.changeFilter(value);
  }

  onPriorityChange(event: Event) {
    const value = (event.target as HTMLSelectElement).value;
    this.changePriority(value);
  }

  totalPages(): number {
    return Math.ceil(this.tasks.length / this.pageSize);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages()) {
      this.page = page;
    }
  }

  pagesArray(): number[] {
    return Array.from({ length: this.totalPages() }, (_, i) => i + 1);
  }

  trackById(index: number, item: TaskItem) {
    return item.id;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
