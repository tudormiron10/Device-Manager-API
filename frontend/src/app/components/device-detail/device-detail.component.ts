import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { DeviceDto } from '../../models/api.models';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-device-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, MatButtonModule, MatIconModule, MatProgressSpinnerModule, MatDividerModule, MatSnackBarModule],
  templateUrl: './device-detail.component.html'
})
export class DeviceDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private apiService = inject(ApiService);
  private snackBar = inject(MatSnackBar);
  authService = inject(AuthService);

  device?: DeviceDto;
  loading: boolean = true;
  error: boolean = false;
  isActionLoading: boolean = false;

  ngOnInit(): void {
    this.loadDevice();
  }

  loadDevice(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.apiService.getDeviceById(id).subscribe({
        next: (data) => {
          this.device = data;
          this.loading = false;
        },
        error: () => {
          this.error = true;
          this.loading = false;
        }
      });
    } else {
      this.error = true;
      this.loading = false;
    }
  }

  assignToMe(): void {
    if (!this.device || this.isActionLoading) return;
    this.isActionLoading = true;
    this.apiService.assignDevice(this.device.id).subscribe({
      next: (updated) => {
        this.device = updated;
        this.isActionLoading = false;
        this.snackBar.open('Device successfully assigned to your profile.', 'Close', { duration: 3000 });
      },
      error: (err) => {
        this.isActionLoading = false;
        this.snackBar.open(err.error?.Message || 'Failed to assign device.', 'Close', { duration: 5000 });
      }
    });
  }

  unassign(): void {
    if (!this.device || this.isActionLoading) return;
    this.isActionLoading = true;
    this.apiService.unassignDevice(this.device.id).subscribe({
      next: (updated) => {
        this.device = updated;
        this.isActionLoading = false;
        this.snackBar.open('Device successfully unassigned.', 'Close', { duration: 3000 });
      },
      error: (err) => {
        this.isActionLoading = false;
        this.snackBar.open(err.error?.Message || 'Failed to unassign device.', 'Close', { duration: 5000 });
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/devices']);
  }
}
