// form-form.component.ts
import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ButtonComponent } from '../../../../../shared/components/button/button.component';
import { FormRegisterModel, FormSelectModel } from '../../../models/form/form.model';

const noWhitespace = (label: string): ValidatorFn => (c: AbstractControl): ValidationErrors | null => {
  const v = (c.value ?? '') as string;
  return v.trim().length === 0 ? { whitespace: `${label} no puede estar en blanco.` } : null;
};

const absoluteUrl: ValidatorFn = (c: AbstractControl): ValidationErrors | null => {
  const v = (c.value ?? '') as string;
  if (!v) return null; // 'required' cubre vacío
  try {
    const u = new URL(v);
    return u.protocol === 'http:' || u.protocol === 'https:' ? null : { urlInvalid: true };
  } catch {
    return { urlInvalid: true };
  }
};

@Component({
  selector: 'app-form-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    ButtonComponent,
  ],
  templateUrl: './form-form.component.html',
  styleUrl: './form-form.component.css',
})
export class FormFormComponent implements OnInit {
  private fb = inject(FormBuilder);

  @Input({ required: true }) title!: string;

  private _model?: FormSelectModel;
  @Input()
  set model(value: FormSelectModel | undefined) {
    this._model = value;
    if (value) this.form.patchValue(value);
  }
  get model() { return this._model; }

  // ⬅️ Volvemos a emitir FormRegisterModel
  @Output() posteoForm = new EventEmitter<FormRegisterModel>();

  form: FormGroup = this.fb.group({
    name: [
      '',
      [Validators.required, Validators.minLength(5), Validators.maxLength(100), noWhitespace('El nombre')],
    ],
    description: [
      '',
      [Validators.required, Validators.minLength(10), Validators.maxLength(300), noWhitespace('La descripción')],
    ],
    url: [
      '',
      [Validators.required, Validators.maxLength(200), absoluteUrl],
    ],
  });

  ngOnInit(): void {
    if (this.model) this.form.patchValue(this.model);
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.value as { name: string; description: string; url: string };

    // Si hay model, usamos su id (update); si no, id: 0 (create). Ajusta si tu backend prefiere null u omitirlo.
    const payload: FormRegisterModel = {
      id: this.model?.id ?? 0,
      name: (raw.name ?? '').trim(),
      description: (raw.description ?? '').trim(),
      url: (raw.url ?? '').trim(),
    };

    this.posteoForm.emit(payload);
  }
}
