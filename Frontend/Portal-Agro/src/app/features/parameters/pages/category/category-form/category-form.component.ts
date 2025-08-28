// category-form.component.ts
import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatOption } from '@angular/material/core';
import { ButtonComponent } from '../../../../../shared/components/button/button.component';
import { CategoryRegistertModel, CategorySelectModel } from '../../../models/category/category.model';

// ====== Validadores utilitarios (alineados con backend) ======
const noWhitespace = (label: string): ValidatorFn =>
  (c: AbstractControl): ValidationErrors | null => {
    const v = (c.value ?? '') as string;
    return typeof v === 'string' && v.trim().length === 0
      ? { whitespace: `${label} no puede estar en blanco.` }
      : null;
  };

/** Valida longitud sobre el valor TRIMMEADO (ej. 5–100) */
const trimmedLength = (min: number, max: number): ValidatorFn =>
  (c: AbstractControl): ValidationErrors | null => {
    const v = (c.value ?? '') as string;
    const len = (v ?? '').trim().length;
    return len === 0 ? null : len < min || len > max ? { trimLength: { min, max } } : null;
  };

/** null es válido; si viene número debe ser entero > 0 */
const positiveIntOrNull: ValidatorFn = (c: AbstractControl): ValidationErrors | null => {
  const val = c.value;
  if (val === null || val === undefined || val === '') return null;
  const n = Number(val);
  return Number.isInteger(n) && n > 0 ? null : { positiveIntOrNull: true };
};

// Payload sin 'id' (para create/update por URL)
type CategoryPayload = Omit<CategoryRegistertModel, 'id'>;

@Component({
  selector: 'app-category-form',
  standalone: true,
  imports: [ButtonComponent, MatInputModule, CommonModule, MatSelectModule, ReactiveFormsModule, MatOption],
  templateUrl: './category-form.component.html',
  styleUrl: './category-form.component.css',
})
export class CategoryFormComponent implements OnInit {
  private fb = inject(FormBuilder);

  @Input({ required: true }) title!: string;
  @Input() parents: CategorySelectModel[] = [];

  private _model?: CategorySelectModel;
  @Input() set model(value: CategorySelectModel | undefined) {
    this._model = value;
    if (value) {
      this.form.patchValue({
        name: value.name,
        parentCategoryId: value.parentCategoryId != null ? Number(value.parentCategoryId) : null,
      });
    }
  }
  get model() {
    return this._model;
  }

  @Output() posteoForm = new EventEmitter<CategoryPayload>();

  form = this.fb.group({
    name: ['', [Validators.required, noWhitespace('El nombre'), trimmedLength(5, 100)]],
    parentCategoryId: [null as number | null, [positiveIntOrNull]],
  });

  ngOnInit(): void {
    // Si es edición, ya parcheamos en el setter
    if (this.model !== undefined) {
      // Evitar que pueda escogerse a sí misma como padre (defensa adicional)
      this.parents = this.parents.filter((p) => p.id !== this.model!.id);
    }
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.value as { name: string; parentCategoryId: number | null };

    const payload: CategoryPayload = {
      name: (raw.name ?? '').trim(),
      parentCategoryId: raw.parentCategoryId ?? null,
    };

    this.posteoForm.emit(payload);
  }
}
