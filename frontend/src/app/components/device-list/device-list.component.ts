import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { DeviceDto } from '../../models/api.models';
import { ConfirmDialogComponent } from '../shared/confirm-dialog.component';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [
    CommonModule, 
    RouterLink, 
    MatTableModule, 
    MatButtonModule, 
    MatIconModule, 
    MatSnackBarModule, 
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule
  ],
  templateUrl: './device-list.component.html'
})
export class DeviceListComponent implements OnInit {
  private apiService = inject(ApiService);
  private snackBar = inject(MatSnackBar);
  private dialog = inject(MatDialog);
  authService = inject(AuthService);

  devices: DeviceDto[] = [];
  displayedColumns: string[] = ['name', 'manufacturer', 'type', 'assignedUserName', 'actions'];
  
  searchControl = new FormControl('');

  ngOnInit(): void {
    this.loadDevices();
    
    // Setup debounced search for Bonus Phase (Free-text Search & Ranking)
    this.searchControl.valueChanges.pipe(
      debounceTime(400),
      distinctUntilChanged()
    ).subscribe(query => {
      this.performSearch(query || '');
    });
  }

  loadDevices(): void {
    this.apiService.getDevices().subscribe({
      next: (data) => this.devices = data,
      error: (err) => this.snackBar.open('Error loading devices...', 'Close', { duration: 3000 })
    });
  }

  performSearch(query: string): void {
    if (!query) {
      this.loadDevices();
      return;
    }

    this.apiService.searchDevices(query).subscribe({
      next: (data) => this.devices = data,
      error: (err) => this.snackBar.open('Error performing search...', 'Close', { duration: 3000 })
    });
  }

  deleteDevice(id: string): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '450px',
      data: {
        title: 'Delete Hardware Asset',
        message: 'Are you sure you want to permanently delete this device? This action cannot be undone and will erase all data regarding this inventory item.'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        this.apiService.deleteDevice(id).subscribe({
          next: () => {
            this.snackBar.open('Device deleted successfully', 'Close', { duration: 3000 });
            this.loadDevices();
          },
          error: (err) => {
            const message = err.error?.message || 'Error deleting device';
            this.snackBar.open(message, 'Close', { duration: 5000 });
          }
        });
      }
    });
  }
}
