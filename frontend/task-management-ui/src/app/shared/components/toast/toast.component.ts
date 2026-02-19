import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toast.component.html'
})
export class ToastComponent {

  constructor(public toastService: ToastService) {}

  getToastClass(type: string) {
    if (type === 'success') return 'bg-success text-white';
    if (type === 'error') return 'bg-danger text-white';
    return 'bg-primary text-white';
  }
}
