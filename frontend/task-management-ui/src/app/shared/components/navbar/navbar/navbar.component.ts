import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../../core/services/auth.service';
import { ToastService } from '../../../services/toast.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html'
})
export class NavbarComponent {

  constructor(
    public authService: AuthService,
    private router: Router,
    private toastService: ToastService
  ) {}

  logout() {
    this.authService.logout();
    this.toastService.showInfo("Sesión cerrada correctamente.");
    this.router.navigate(['/auth']);
  }
}
