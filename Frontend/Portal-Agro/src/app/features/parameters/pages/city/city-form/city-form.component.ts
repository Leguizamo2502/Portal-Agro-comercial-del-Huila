// city-form.component.ts
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
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { ButtonComponent } from '../../../../../shared/components/button/button.component';

import { CityRegisterModel, CitySelectModel } from '../../../models/city/city.model';
import { DepartmentSelectModel } from '../../../models/department/department.model';

// ===== Validadores alineados con backend =====
const noWhitespace = (label: string): ValidatorFn =>
  (c: AbstractControl): ValidationErrors | null => {
    const v = (c.value ?? '') as string;
    return typeof v === 'string' && v.trim().length === 0
      ? { whitespace: `${label} no puede estar en blanco.` }
      : null;
  };

const trimmedLength = (min: number, max: number): ValidatorFn =>
  (c: AbstractControl): ValidationErrors | null => {
    const v = (c.value ?? '') as string;
    const len = (v ?? '').trim().length;
    return len === 0 ? null : (len < min || len > max) ? { trimLength: { min, max } } : null;
  };

const positiveInt: ValidatorFn =
  (c: AbstractControl): ValidationErrors | null => {
    const n = Number(c.value);
    return Number.isInteger(n) && n > 0 ? null : { positiveInt: true };
  };

// Payload que espera el backend (sin 'id')
type CityPayload = { name: string; DepartmentId: number };

@Component({
  selector: 'app-city-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    ButtonComponent
  ],
  templateUrl: './city-form.component.html',
  styleUrls: ['./city-form.component.css']
})
export class CityFormComponent implements OnInit {
  private fb = inject(FormBuilder);

  @Input({ required: true }) title!: string;
  @Input() departments: DepartmentSelectModel[] = [];

  private _model?: CitySelectModel;
  @Input() set model(value: CitySelectModel | undefined) {
    this._model = value;
    if (value) {
      // Mapear del modelo (PascalCase) al form (camelCase)
      this.form.patchValue({
        name: value.name,
        departmentId: value.DepartmentId
      });
    }
  }
  get model() { return this._model; }

  @Output() posteoForm = new EventEmitter<CityPayload>();

  form = this.fb.group({
    name: ['', [Validators.required, noWhitespace('El nombre'), trimmedLength(3, 100)]],
    // El form usa camelCase 'departmentId'
    departmentId: [null as number | null, [Validators.required, positiveInt]],
  });

  ngOnInit(): void {
    if (this.model) {
      // Reforzar el mapeo en init por si llega después
      this.form.patchValue({
        name: this.model.name,
        departmentId: this.model.DepartmentId
      });
    }
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    // Tomamos camelCase del form...
    const raw = this.form.value as { name: string; departmentId: number };

    // ...y emitimos PascalCase que espera el backend.
    const payload: CityPayload = {
      name: (raw.name ?? '').trim(),
      DepartmentId: Number(raw.departmentId),
    };
    this.posteoForm.emit(payload);
  }
}
