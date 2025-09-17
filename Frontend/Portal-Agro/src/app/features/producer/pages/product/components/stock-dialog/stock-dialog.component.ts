// stock-dialog.component.ts
import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

export interface StockDialogData {
  productId: number;
  currentStock: number;
  productName?: string;
}

@Component({
  selector: 'app-stock-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  template: `
  <h2 mat-dialog-title>Actualizar Stock</h2>
  <div mat-dialog-content>
    <p *ngIf="data.productName">Producto: <strong>{{ data.productName }}</strong></p>
    <form [formGroup]="form" (ngSubmit)="onSubmit()">
      <mat-form-field appearance="outline" class="w-100">
        <mat-label>Nuevo stock</mat-label>
        <input matInput type="number" formControlName="newStock" min="0" />
        <mat-error *ngIf="form.get('newStock')?.hasError('required')">Requerido</mat-error>
        <mat-error *ngIf="form.get('newStock')?.hasError('min')">No puede ser negativo</mat-error>
      </mat-form-field>
    </form>
  </div>
  <div mat-dialog-actions align="end">
    <button mat-stroked-button (click)="dialogRef.close()">Cancelar</button>
    <button mat-flat-button color="primary" [disabled]="form.invalid || loading" (click)="onSubmit()">
      Guardar
    </button>
  </div>
  `
})
export class StockDialogComponent {
  loading = false;
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<StockDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: StockDialogData
  ) {
    this.form = this.fb.group({
      newStock: [data.currentStock, [Validators.required, Validators.min(0)]]
    });
  }

  onSubmit() {
    if (this.form.invalid) return;
    this.dialogRef.close({
      productId: this.data.productId,
      newStock: this.form.value.newStock as number
    });
  }
}
