// rol-form-permission-form.component.ts
import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';

import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ButtonComponent } from '../../../../../shared/components/button/button.component';

import { PermissionSelectModel } from '../../../models/permission/permission.model';
import { FormSelectModel } from '../../../models/form/form.model';
import { RolSelectModel } from '../../../models/rol/rol.model';
import { RolFormPermissionRegisterModel, RolFormPermissionSelectModel } from '../../../models/rolFormPermission/rolFormPermission.model';

@Component({
  selector: 'app-rol-form-permission-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatInputModule, MatSelectModule, ButtonComponent],
  templateUrl: './rol-form-permission-form.component.html',
  styleUrl: './rol-form-permission-form.component.css'
})
export class RolFormPermissionFormComponent implements OnInit {
  private fb = inject(FormBuilder);

  @Input({ required: true }) title!: string;
  @Input() forms: FormSelectModel[] = [];
  @Input() rols: RolSelectModel[] = [];
  @Input() permissions: PermissionSelectModel[] = [];

  private _model?: RolFormPermissionSelectModel;
  @Input() set model(value: RolFormPermissionSelectModel | undefined) {
    this._model = value;
    if (value) {
      this.form.patchValue({
        rolId: value.rolId,
        formId: value.formId,
        permissionId: value.permissionId,
      });
    }
  }
  get model() { return this._model; }

  @Output() posteoForm = new EventEmitter<RolFormPermissionRegisterModel>();

  // Non-nullable y con min(1) => > 0
  form = this.fb.nonNullable.group({
    rolId:        [0, [Validators.required, Validators.min(1)]],
    formId:       [0, [Validators.required, Validators.min(1)]],
    permissionId: [0, [Validators.required, Validators.min(1)]],
  });

  ngOnInit(): void {
    if (this._model) {
      this.form.patchValue({
        rolId: this._model.rolId,
        formId: this._model.formId,
        permissionId: this._model.permissionId,
      });
    }
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const payload: RolFormPermissionRegisterModel = this.form.getRawValue();
    this.posteoForm.emit(payload);
  }
}
