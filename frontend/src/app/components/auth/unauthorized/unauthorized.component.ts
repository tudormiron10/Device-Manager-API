import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-unauthorized',
  standalone: true,
  imports: [CommonModule, RouterLink, MatIconModule, MatButtonModule],
  templateUrl: './unauthorized.component.html'
})
export class UnauthorizedComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  logoutAndReturn(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
