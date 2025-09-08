import { Component, inject, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule, AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { catchError, finalize, of, take } from 'rxjs';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef, MatDialogContent, MatDialogActions } from '@angular/material/dialog';
import { DepartmentModel, CityModel } from '../../../../shared/models/location/location.model';
import { LocationService } from '../../../../shared/services/location/location.service';
import { OrderCreateModel, CreateOrderResponse } from '../../models/order/order.model';
import { OrderService } from '../../services/order/order.service';
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { ButtonComponent } from "../../../../shared/components/button/button.component";
import { MatIconModule } from "@angular/material/icon";
import { MatStep, MatStepperModule } from "@angular/material/stepper";

export interface OrderCreateDialogData {
  productId: number;
  productName: string;
  unitPrice: number;
  stock: number;
  shippingNote: string; // 'Envío gratis' | 'No incluye envío'
}

// ===== Validadores utilitarios (alineados al backend) =====
const positiveInt = (label: string): ValidatorFn => (c: AbstractControl): ValidationErrors | null => {
  const n = Number(c.value);
  if (!Number.isInteger(n) || n <= 0) return { positiveInt: `${label} debe ser mayor a 0.` };
  return null;
};

const maxInt = (max: number, label: string): ValidatorFn => (c: AbstractControl): ValidationErrors | null => {
  const n = Number(c.value);
  if (!Number.isInteger(n)) return { required: `${label} es obligatorio.` };
  if (n > max) return { max: `${label} no puede exceder ${max}.` };
  return null;
};

const requiredTrimmed = (label: string): ValidatorFn => (c: AbstractControl): ValidationErrors | null => {
  const v = (c.value ?? '').toString().trim();
  if (!v) return { required: `${label} es obligatorio.` };
  return null;
};

const phoneBasic = (label: string): ValidatorFn => (c: AbstractControl): ValidationErrors | null => {
  const v = (c.value ?? '').toString().trim();
  if (!v) return { required: `${label} es obligatorio.` };
  // básico: dígitos + símbolos comunes, 7–20 chars
  if (!/^[0-9 +()\-]{7,20}$/.test(v)) return { pattern: `${label} no es válido.` };
  return null;
};

@Component({
  selector: 'app-order-create-dialog',
  imports: [MatInputModule, MatSelectModule, MatDialogContent, CommonModule, ReactiveFormsModule, MatDialogActions, ButtonComponent, MatIconModule, MatStep, MatStepperModule],
  templateUrl: './order-create-dialog.component.html',
  styleUrl: './order-create-dialog.component.css'
})


export class OrderCreateDialogComponent {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<OrderCreateDialogComponent>);
  private locationSrv = inject(LocationService);
  private orderSrv = inject(OrderService);

  constructor(@Inject(MAT_DIALOG_DATA) public data: OrderCreateDialogData) {}

  // Step groups
  productGroup!: FormGroup;
  deliveryGroup!: FormGroup;
  paymentGroup!: FormGroup;

  // Catálogos
  departments: DepartmentModel[] = [];
  cities: CityModel[] = [];

  // Estado UI
  isSubmitting = false;
  MAX_FILE_MB = 6;
  MAX_FILE_BYTES = this.MAX_FILE_MB * 1024 * 1024;

  // File
  paymentFile?: File;
  paymentPreview?: string; // base64 preview

  ngOnInit(): void {
    this.initForms();
    this.loadDepartments();
    // Pre-carga ciudades si quieres a partir del depto seleccionado (aquí vacío inicialmente)
  }

  // ---------- Init Forms ----------
  private initForms(): void {
    this.productGroup = this.fb.group({
      quantityRequested: [1, [positiveInt('Cantidad'), maxInt(this.data.stock, 'Cantidad')]],
    });

    this.deliveryGroup = this.fb.group({
      recipientName: ['', [requiredTrimmed('Nombre del destinatario')]],
      contactPhone: ['', [phoneBasic('Teléfono de contacto')]],
      departmentId: [null, [Validators.required]],
      cityId: [null, [Validators.required, positiveInt('Ciudad')]],
      addressLine1: ['', [requiredTrimmed('Dirección')]],
      addressLine2: [''],
      additionalNotes: [''],
    });

    this.paymentGroup = this.fb.group({
      paymentImage: [null, [Validators.required]], // solo marcador para estado del stepper
    });
  }

  // ---------- Catálogos ----------
  private loadDepartments(): void {
    this.locationSrv.getDepartment().pipe(take(1)).subscribe({
      next: (deps) => (this.departments = deps ?? []),
    });
  }

  onDepartmentChange(depId: number): void {
    this.deliveryGroup.patchValue({ cityId: null });
    this.cities = [];
    if (!depId) return;

    this.locationSrv.getCity(depId).pipe(take(1)).subscribe({
      next: (cities) => (this.cities = cities ?? []),
    });
  }

  // ---------- Helpers UI ----------
  get quantity(): number {
    return Number(this.productGroup.value.quantityRequested || 0);
  }

  get subtotal(): number {
    return this.quantity * (this.data.unitPrice ?? 0);
  }

  formatCop(n: number): string {
    return new Intl.NumberFormat('es-CO', { style: 'currency', currency: 'COP', maximumFractionDigits: 0 }).format(n);
  }

  // ---------- File handlers ----------
  onFilePicked(files: FileList | null): void {
    if (!files?.length) return;
    const file = files.item(0)!;

    if (!file.type.startsWith('image/')) {
      this.toast('El comprobante debe ser una imagen (JPG/PNG/WEBP).', 'warning');
      return;
    }
    if (file.size > this.MAX_FILE_BYTES) {
      this.toast(`La imagen excede ${this.MAX_FILE_MB} MB.`, 'warning');
      return;
    }

    this.paymentFile = file;
    this.paymentGroup.get('paymentImage')!.setValue('ok');
    this.readPreview(file);
  }

  removeFile(): void {
    this.paymentFile = undefined;
    this.paymentPreview = undefined;
    this.paymentGroup.get('paymentImage')!.reset();
  }

  private readPreview(file: File): void {
    const reader = new FileReader();
    reader.onload = (ev) => (this.paymentPreview = ev.target?.result as string);
    reader.readAsDataURL(file);
  }

  // ---------- Submit ----------
  submit(): void {
    if (this.isSubmitting) return;

    this.productGroup.markAllAsTouched();
    this.deliveryGroup.markAllAsTouched();
    this.paymentGroup.markAllAsTouched();

    if (this.productGroup.invalid || this.deliveryGroup.invalid || !this.paymentFile) return;

    const qty = Number(this.productGroup.value.quantityRequested);
    const d = this.deliveryGroup.value;

    const dto = {
      productId: this.data.productId,
      quantityRequested: qty,
      recipientName: (d.recipientName ?? '').trim(),
      contactPhone: (d.contactPhone ?? '').trim(),
      addressLine1: (d.addressLine1 ?? '').trim(),
      addressLine2: (d.addressLine2 ?? '').trim() || undefined,
      cityId: Number(d.cityId),
      additionalNotes: (d.additionalNotes ?? '').trim() || undefined,
      paymentImage: this.paymentFile,
    };

    this.isSubmitting = true;

    Swal.fire({ title: 'Creando pedido...', allowOutsideClick: false, didOpen: () => Swal.showLoading() });

    this.orderSrv
      .create(dto)
      .pipe(
        take(1),
        catchError((err) => {
          const msg = err?.error?.message || err?.message || 'No se pudo crear el pedido.';
          this.toast(msg, 'error');
          return of(null);
        }),
        finalize(() => (this.isSubmitting = false))
      )
      .subscribe((resp) => {
        Swal.close();
        if (!resp) return;

        // Esperamos { IsSuccess: true, OrderId: number } desde el backend
        if (resp.isSuccess) {
          // this.toast('Pedido Completado.', 'success');
           Swal.fire({
                    icon: 'success',
                    title: 'Pedido Completado.',
                    text: 'Pedido completado exitosamente.',
                  });
          this.dialogRef.close(resp); // El padre muestra el toast de éxito
        } else {
          this.toast('No se pudo crear el pedido.', 'error');
        }
      });
  }

  close(): void {
    this.dialogRef.close();
  }

  // ---------- Toast helper ----------
  private async toast(title: string, icon: 'success' | 'error' | 'warning' | 'info') {
    await Swal.fire({ toast: true, position: 'top-end', timer: 1800, showConfirmButton: false, icon, title });
  }
}