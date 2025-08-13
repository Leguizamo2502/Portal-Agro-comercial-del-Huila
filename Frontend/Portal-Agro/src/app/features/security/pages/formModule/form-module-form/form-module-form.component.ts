import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormSelectModel } from '../../../models/form/form.model';
import { FormModuleSelectModel, FormModuleRegisterModel } from '../../../models/formModule/formModule.model';
import { ModuleSelectModel } from '../../../models/module/module.model';
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { ButtonComponent } from "../../../../../shared/components/button/button.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-form-module-form',
  imports: [MatInputModule, MatSelectModule, ReactiveFormsModule, ButtonComponent,CommonModule],
  templateUrl: './form-module-form.component.html',
  styleUrl: './form-module-form.component.css'
})
export class FormModuleFormComponent {
    private fb = inject(FormBuilder);

  @Input({ required: true }) title!: string;
  @Input() forms: FormSelectModel[] = [];
  @Input() modules: ModuleSelectModel[] = [];

  private _model?: FormModuleSelectModel;
  @Input() set model(value: FormModuleSelectModel | undefined) {
    this._model = value;
    if (value) {
      // Parchar SOLO las claves que existen en el form
      this.form.patchValue({ moduleId: value.moduleId, formId: value.formId });
    }
  }
  get model() { return this._model; }

  @Output() posteoForm = new EventEmitter<FormModuleRegisterModel>();

  // Contformes no anulables: el form espera números, no null
  form = this.fb.nonNullable.group({
    moduleId: [0, Validators.required],
    formId:  [0, Validators.required],
  });

  ngOnInit(): void {
    if (this._model) {
      this.form.patchValue({ moduleId: this._model.moduleId, formId: this._model.formId });
    }
  }

  save() {
    const form = this.form.value as FormModuleSelectModel;
    this.posteoForm.emit(form);
  }
}
