import { Component, EventEmitter, Input, Output, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ButtonComponent } from '../../../../../shared/components/button/button.component';

import { FormSelectModel } from '../../../models/form/form.model';
import {
  FormModuleSelectModel,
  FormModuleRegisterModel,
} from '../../../models/formModule/formModule.model';
import { ModuleSelectModel } from '../../../models/module/module.model';

@Component({
  selector: 'app-form-module-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    ButtonComponent,
  ],
  templateUrl: './form-module-form.component.html',
  styleUrl: './form-module-form.component.css',
})
export class FormModuleFormComponent implements OnInit {
  private fb = inject(FormBuilder);

  @Input({ required: true }) title!: string;
  @Input() forms: FormSelectModel[] = [];
  @Input() modules: ModuleSelectModel[] = [];

  private _model?: FormModuleSelectModel;
  @Input() set model(value: FormModuleSelectModel | undefined) {
    this._model = value;
    if (value) {
      // Parchar SOLO las claves que existen en el form
      this.form.patchValue({ formId: value.formId, moduleId: value.moduleId });
    }
  }
  get model() { return this._model; }

  @Output() posteoForm = new EventEmitter<FormModuleRegisterModel>();

  // Non-nullable: números por defecto 0, luego validamos > 0
  form = this.fb.nonNullable.group({
    formId:    [0, [Validators.required, Validators.min(1)]],
    moduleId:  [0, [Validators.required, Validators.min(1)]],
  });

  ngOnInit(): void {
    if (this._model) {
      this.form.patchValue({
        formId: this._model.formId,
        moduleId: this._model.moduleId,
      });
    }
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    // Emitimos exactamente el DTO que espera el backend
    const payload: FormModuleRegisterModel = {
      formId: this.form.getRawValue().formId,
      moduleId: this.form.getRawValue().moduleId,
    };

    this.posteoForm.emit(payload);
  }
}
