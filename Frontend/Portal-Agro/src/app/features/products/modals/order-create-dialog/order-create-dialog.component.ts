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

const minLetters = (min: number, label: string): ValidatorFn => (c: AbstractControl): ValidationErrors | null => {
  const raw = (c.value ?? '').toString();
  // Extrae letras Unicode (incluye tildes y ñ)
  const letters = raw.match(/\p{L}/gu) ?? [];
  if (letters.length < min) return { minLetters: `${label} debe tener al menos ${min} letras.` };
  return null;
};


const minAlnum = (min: number, label: string): ValidatorFn => (c: AbstractControl): ValidationErrors | null => {
  const raw = (c.value ?? '').toString();
  const alnum = raw.match(/[\p{L}\p{N}]/gu) ?? [];
  if (alnum.length < min) return { minAlnum: `${label} debe tener al menos ${min} caracteres válidos.` };
  return null;
};


const phoneCoMobile = (label: string): ValidatorFn => (c: AbstractControl): ValidationErrors | null => {
  const raw = (c.value ?? '').toString();
  const digits = raw.replace(/\D+/g, ''); // deja solo dígitos
  if (digits.length === 0) return { required: `${label} es obligatorio.` };
  if (digits.length !== 10) return { phoneCo: `${label} debe tener 10 dígitos.` };
  if (!digits.startsWith('3')) return { phoneCo: `${label} debe iniciar con 3.` };
  return null;
};

@Component({
  selector: 'app-order-create-dialog',
  imports: [MatInputModule, MatSelectModule, CommonModule, ReactiveFormsModule, ButtonComponent, MatIconModule, MatStep, MatStepperModule],
  templateUrl: './order-create-dialog.component.html',
  styleUrl: './order-create-dialog.component.css'
})
export class OrderCreateDialogComponent {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<OrderCreateDialogComponent>);
  private locationSrv = inject(LocationService);
  private orderSrv = inject(OrderService);

  constructor(@Inject(MAT_DIALOG_DATA) public data: OrderCreateDialogData) {}

  // Step groups (ya sin payment)
  productGroup!: FormGroup;
  deliveryGroup!: FormGroup;

  // Catálogos
  departments: DepartmentModel[] = [];
  cities: CityModel[] = [];

  // Estado UI
  isSubmitting = false;

  ngOnInit(): void {
    this.initForms();
    this.loadDepartments();
  }

  // ---------- Init Forms ----------
 private initForms(): void {
  this.productGroup = this.fb.group({
    quantityRequested: [1, [positiveInt('Cantidad'), maxInt(this.data.stock, 'Cantidad')]],
  });

  this.deliveryGroup = this.fb.group({
    recipientName: [
      '',
      [
        requiredTrimmed('Nombre del destinatario'),
        minLetters(3, 'Nombre del destinatario'),
      ],
    ],
    contactPhone: [
      '',
      [
        phoneCoMobile('Teléfono de contacto'), 
      ],
    ],
    departmentId: [null, [Validators.required]],
    cityId: [null, [Validators.required, positiveInt('Ciudad')]],
    addressLine1: [
      '',
      [
        requiredTrimmed('Dirección'),
        minAlnum(5, 'Dirección'), 
      ],
    ],
    addressLine2: [''], 
    additionalNotes: [''], 
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

  // ---------- Submit ----------
  submit(): void {
    if (this.isSubmitting) return;

    this.productGroup.markAllAsTouched();
    this.deliveryGroup.markAllAsTouched();

    if (this.productGroup.invalid || this.deliveryGroup.invalid) return;

    const qty = Number(this.productGroup.value.quantityRequested);
    const d = this.deliveryGroup.value;

    const dto: OrderCreateModel = {
      productId: this.data.productId,
      quantityRequested: qty,
      recipientName: (d.recipientName ?? '').trim(),
      contactPhone: (d.contactPhone ?? '').trim(),
      addressLine1: (d.addressLine1 ?? '').trim(),
      addressLine2: (d.addressLine2 ?? '').trim() || undefined,
      cityId: Number(d.cityId),
      additionalNotes: (d.additionalNotes ?? '').trim() || undefined,
    };

    this.isSubmitting = true;

    Swal.fire({ title: 'Creando pedido...', allowOutsideClick: false, didOpen: () => Swal.showLoading() });

    this.orderSrv.create(dto)
  .pipe(
    take(1),
    catchError((err) => {
      const msg = err?.error?.message || err?.message || 'No se pudo crear el pedido.';
      // Mostrar error aquí
      this.toast(msg, 'error');
      // Además, propagar un objeto de respuesta falso al padre si quieres que también avise
      const fallback: CreateOrderResponse = { isSuccess: false, message: msg, };
      // Cerramos para que el padre pueda decidir si muestra algo más
      this.dialogRef.close(fallback);
      return of(null);
    }),
    finalize(() => (this.isSubmitting = false))
  )
  .subscribe((resp) => {
    Swal.close();
    if (!resp) return;

    if (resp.isSuccess) {
      Swal.fire({
        icon: 'success',
        title: 'Pedido creado',
        text: `Tu pedido #${resp.orderId} fue creado. Te enviaremos instrucciones por correo cuando el productor lo revise.`,
      });
      this.dialogRef.close(resp);
    } else {
      // Muestra el mensaje del backend y cierra devolviendo resp
      Swal.fire({
        icon: 'error',
        title: 'No se pudo crear el pedido',
        text: resp.message || 'Ocurrió un error al crear el pedido.',
      });
      this.dialogRef.close(resp);
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
