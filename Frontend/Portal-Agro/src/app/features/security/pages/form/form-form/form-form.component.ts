import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ButtonComponent } from '../../../../../shared/components/button/button.component';
import { FormRegisterModel, FormSelectModel } from '../../../models/form/form.model';

//
// 🔹 Validadores personalizados
//
const noWhitespaceOrInvalidStart = (label: string): ValidatorFn =>
  (c: AbstractControl): ValidationErrors | null => {
    const v = (c.value ?? '') as string;

    if (!/^[A-Z]/.test(v)) {
      return { startsWithUppercase: `${label} debe comenzar con una letra mayúscula.` };
    }
    return null;
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

const noSpaces: ValidatorFn = (c: AbstractControl): ValidationErrors | null => {
  const v = (c.value ?? '') as string;
  return /\s/.test(v) ? { noSpaces: true } : null;
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
  styleUrls: ['./form-form.component.css'],
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

  @Output() posteoForm = new EventEmitter<FormRegisterModel>();

  form: FormGroup = this.fb.group({
    name: [
      '',
      [
        Validators.required,
        Validators.minLength(5),
        Validators.maxLength(100),
        noWhitespaceOrInvalidStart('El nombre'),
      ],
    ],
    description: [
      '',
      [
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(300),
        noWhitespaceOrInvalidStart('La descripción'),
      ],
    ],
    url: [
      '',
      [Validators.required, Validators.maxLength(200), absoluteUrl, noSpaces],
    ],
  });

  ngOnInit(): void {
    if (this.model) this.form.patchValue(this.model);
  }

  // - Elimina espacios iniciales
  // - Fuerza primera letra en mayúscula cuando el usuario escribe el primer carácter
  onInputChange(event: Event, controlName: 'name' | 'description'): void {
    const input = event.target as HTMLInputElement | HTMLTextAreaElement;
    let value = input.value ?? '';

    if (value.startsWith(' ')) {
      value = value.trimStart();
    }

    if (value.length === 1) {
      value = value.toUpperCase();
    }

    this.form.get(controlName)?.setValue(value, { emitEvent: false });
  }

  //Elimina espacios si se pegan desde el portapapeles + previene espacios por teclado
  onUrlInput(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input.value) return;
    const cleaned = input.value.replace(/\s/g, '');
    if (cleaned !== input.value) {
      input.value = cleaned;
      this.form.get('url')?.setValue(cleaned, { emitEvent: false });
    }
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.value as { name: string; description: string; url: string };

    const payload: FormRegisterModel = {
      id: this.model?.id ?? 0,
      name: (raw.name ?? '').trim(),
      description: (raw.description ?? '').trim(),
      url: (raw.url ?? '').trim(),
    };

    this.posteoForm.emit(payload);
  }
}
