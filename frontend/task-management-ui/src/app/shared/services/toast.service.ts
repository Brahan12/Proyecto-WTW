import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface ToastMessage {
  type: 'success' | 'error' | 'info';
  text: string;
}

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  private toastSubject = new BehaviorSubject<ToastMessage | null>(null);
  toast$ = this.toastSubject.asObservable();

  showSuccess(text: string) {
    this.toastSubject.next({ type: 'success', text });
    this.autoClear();
  }

  showError(text: string) {
    this.toastSubject.next({ type: 'error', text });
    this.autoClear();
  }

  showInfo(text: string) {
    this.toastSubject.next({ type: 'info', text });
    this.autoClear();
  }

  clear() {
    this.toastSubject.next(null);
  }

  private autoClear() {
    setTimeout(() => this.clear(), 3500);
  }
}
