// permission-form.component.ts
import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators, AbstractControl, ValidatorFn, ValidationErrors } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ButtonComponent } from '../../../../../shared/components/button/button.component';
import { PermissionRegisterModel, PermissionSelectModel } from '../../../models/permission/permission.model';

const noWhitespace = (label: string): ValidatorFn =>
  (c: AbstractControl): ValidationErrors | null => {
    const v = (c.value ?? '') as string;
    return v.trim().length === 0 ? { whitespace: `${label} no puede estar en blanco.` } : null;
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
  styleUrl: './permission-form.component.css'
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
      [Validators.required, Validators.minLength(5), Validators.maxLength(100), noWhitespace('El nombre')],
    ],
    description: [
      '',
      [Validators.required, Validators.minLength(10), Validators.maxLength(300), noWhitespace('La descripción')],
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

    const raw = this.form.value as { name: string; description: string };

    const payload: PermissionRegisterModel = {
      // si tu PermissionRegisterModel NO tiene 'id', perfecto; si lo tiene opcional, puedes añadir: id: this.model?.id
      name: (raw.name ?? '').trim(),
      description: (raw.description ?? '').trim(),
    };

    this.posteoForm.emit(payload);
  }
}
