import { Component, OnDestroy, OnInit } from '@angular/core';
import { UserService } from '../../../core/services/user.service';
import { Subject, takeUntil } from 'rxjs';
import { CommonModule } from '@angular/common';
import { User } from '../../../core/models/user.model';
import { ToastService } from '../../../shared/services/toast.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './user-list.component.html'
})
export class UserListComponent implements OnInit, OnDestroy {

  users: User[] = [];

  page = 1;
  pageSize = 5;

  private destroy$ = new Subject<void>();

  constructor(
    private userService: UserService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res) => this.users = res,
        error: () => this.toastService.showError("Error cargando usuarios.")
      });
  }

  get paginatedUsers(): User[] {
    const start = (this.page - 1) * this.pageSize;
    return this.users.slice(start, start + this.pageSize);
  }

  totalPages(): number {
    return Math.ceil(this.users.length / this.pageSize);
  }

  pagesArray(): number[] {
    return Array.from({ length: this.totalPages() }, (_, i) => i + 1);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages()) {
      this.page = page;
    }
  }

  trackById(index: number, item: User) {
    return item.id;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
