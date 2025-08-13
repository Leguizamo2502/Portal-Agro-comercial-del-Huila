import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { PermissionSelectModel } from '../../../models/permission/permission.model';
import { FormSelectModel } from '../../../models/form/form.model';
import { RolSelectModel } from '../../../models/rol/rol.model';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RolFormPermissionRegisterModel, RolFormPermissionSelectModel } from '../../../models/rolFormPermission/rolFormPermission.model';
import { CommonModule } from '@angular/common';
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { ButtonComponent } from "../../../../../shared/components/button/button.component";

@Component({
  selector: 'app-rol-form-permission-form',
  imports: [CommonModule, ReactiveFormsModule, MatInputModule, MatSelectModule, ButtonComponent],
  templateUrl: './rol-form-permission-form.component.html',
  styleUrl: './rol-form-permission-form.component.css'
})
export class RolFormPermissionFormComponent implements OnInit{
  private fb = inject(FormBuilder);

  @Input({ required: true }) title!: string;
  @Input() forms: FormSelectModel[] = [];
  @Input() rols: RolSelectModel[] = [];
  @Input() permissions: PermissionSelectModel[] = [];


  private _model?: RolFormPermissionSelectModel;
  @Input() set model(value: RolFormPermissionSelectModel | undefined) {
    this._model = value;
    if (value) {
      // Parchar SOLO las claves que existen en el form
      this.form.patchValue({ rolId: value.rolId, formId: value.formId, permissionId:value.permissionId});
    }
  }
  get model() {
    return this._model;
  }

  @Output() posteoForm = new EventEmitter<RolFormPermissionRegisterModel>();

  // Contformes no anulables: el form espera números, no null
  form = this.fb.nonNullable.group({
    rolId: [0, Validators.required],
    formId: [0, Validators.required],
    permissionId: [0, Validators.required],

  });

  ngOnInit(): void {
    if (this._model) {
      this.form.patchValue({
        rolId: this._model.rolId,
        formId: this._model.formId,
        permissionId:this._model.permissionId
      });
    }
  }

  save() {
    const form = this.form.value as RolFormPermissionRegisterModel;
    this.posteoForm.emit(form);
  }
}
