import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators, AbstractControl, ValidatorFn, ValidationErrors } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ButtonComponent } from '../../../../../shared/components/button/button.component';
import { PermissionRegisterModel, PermissionSelectModel } from '../../../models/permission/permission.model';

// ✅ Validador combinado:
// - No puede quedar en blanco (solo espacios)
// - No puede iniciar con espacio
// - Debe iniciar con mayúscula
const noWhitespaceOrInvalidStart = (label: string): ValidatorFn =>
  (c: AbstractControl): ValidationErrors | null => {
    const v = (c.value ?? '') as string;

    if (/^\s/.test(v)) {
      return { startsWithSpace: `${label} no puede comenzar con un espacio.` };
    }

    return null;
  };

@Component({
  selector: 'app-permission-form',
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
  templateUrl: './permission-form.component.html',
  styleUrls: ['./permission-form.component.css']
})
export class PermissionFormComponent implements OnInit {
  private fb = inject(FormBuilder);

  @Input({ required: true }) title!: string;

  private _model?: PermissionSelectModel;
  @Input()
  set model(value: PermissionSelectModel | undefined) {
    this._model = value;
    if (value) this.form.patchValue(value);
  }
  get model() { return this._model; }

  @Output() posteoForm = new EventEmitter<PermissionRegisterModel>();

  form = this.fb.group({
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
  });

  ngOnInit(): void {
    if (this.model) this.form.patchValue(this.model);
  }

  // 🔧 Autocorrección de entrada (UX)
  // - Elimina espacios iniciales
  // - Fuerza primera letra en mayúscula cuando el usuario escribe el primer carácter
  onInputChange(event: Event, controlName: string): void {
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

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.value as { name: string; description: string };

    const payload: PermissionRegisterModel = {
      name: (raw.name ?? '').trim(),
      description: (raw.description ?? '').trim(),
    };

    this.posteoForm.emit(payload);
  }
}
