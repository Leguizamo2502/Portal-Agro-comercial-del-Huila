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
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './stock-dialog.component.html',
  styleUrl: './stock-dialog.component.css'
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
