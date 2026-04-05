import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatDialogModule, MatIconModule],
  template: `
    <h2 mat-dialog-title class="!text-xl !font-black !uppercase !tracking-widest !text-primary !flex !items-center !border-b-[3px] !border-gray-100 !pb-4 !m-0 !pt-6 !px-6">
      <mat-icon class="mr-3 text-red-700">warning</mat-icon> {{ data.title }}
    </h2>
    <mat-dialog-content class="!py-8 !px-6">
      <p class="text-base font-medium text-gray-700">{{ data.message }}</p>
    </mat-dialog-content>
    <mat-dialog-actions align="end" class="!border-t-[3px] !border-gray-100 !pt-4 !pb-4 !px-6 bg-gray-50/80">
      <button mat-button mat-dialog-close class="!font-bold !uppercase !tracking-wider !text-gray-600">Cancel</button>
      <button mat-raised-button color="warn" [mat-dialog-close]="true" class="!font-bold !uppercase !tracking-wider">Confirm Delete</button>
    </mat-dialog-actions>
  `
})
export class ConfirmDialogComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: { title: string, message: string }) {}
}
