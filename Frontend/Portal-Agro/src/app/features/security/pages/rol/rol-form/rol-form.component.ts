// rol-form.component.ts
import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ButtonComponent } from '../../../../../shared/components/button/button.component';
import { RolSelectModel, RolRegisterModel } from '../../../models/rol/rol.model';

const noWhitespace = (label: string): ValidatorFn =>
  (c: AbstractControl): ValidationErrors | null => {
    const v = (c.value ?? '') as string;
    return v.trim().length === 0 ? { whitespace: `${label} no puede estar en blanco.` } : null;
  };

@Component({
  selector: 'app-rol-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, ButtonComponent],
  templateUrl: './rol-form.component.html',
  styleUrl: './rol-form.component.css'
})
export class RolFormComponent {
  private fb = inject(FormBuilder);

  @Input({ required: true }) title!: string;

  private _model?: RolSelectModel;
  @Input()
  set model(value: RolSelectModel | undefined) {
    this._model = value;
    if (value) this.form.patchValue(value);
  }
  get model() { return this._model; }

  @Output() posteoForm = new EventEmitter<RolRegisterModel>();

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

    const payload: RolRegisterModel = {
      // si tu RolRegisterModel no tiene 'id', perfecto; si lo tiene opcional, puedes incluir: id: this.model?.id
      name: (raw.name ?? '').trim(),
      description: (raw.description ?? '').trim(),
    };

    this.posteoForm.emit(payload);
  }
}
