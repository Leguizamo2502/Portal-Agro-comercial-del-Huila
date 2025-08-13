import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RolSelectModel } from '../../../models/rol/rol.model';
import { UserSelectModel } from '../../../../../Core/Models/user.model';
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { ButtonComponent } from "../../../../../shared/components/button/button.component";
import { CommonModule } from '@angular/common';

export interface RolUserSelectModel{
  id: number;
  rolId: number;
  rolName: string;
  userId: number;
  userName: string;
}

export interface RolUserRegisterModel{
  rolId: number;
  userId: number;
}

@Component({
  selector: 'app-rol-user-form',
  imports: [MatInputModule, MatSelectModule, ButtonComponent,ReactiveFormsModule,CommonModule],
  templateUrl: './rol-user-form.component.html',
  styleUrl: './rol-user-form.component.css'
})
export class RolUserFormComponent {
  private fb = inject(FormBuilder);

  @Input({ required: true }) title!: string;
  @Input() rols: RolSelectModel[] = [];
  @Input() users: UserSelectModel[] = [];

  private _model?: RolUserSelectModel;
  @Input() set model(value: RolUserSelectModel | undefined) {
    this._model = value;
    if (value) {
      // Parchar SOLO las claves que existen en el form
      this.form.patchValue({ userId: value.userId, rolId: value.rolId });
    }
  }
  get model() { return this._model; }

  @Output() posteoForm = new EventEmitter<RolUserRegisterModel>();

  // Controles no anulables: el form espera números, no null
  form = this.fb.nonNullable.group({
    userId: [0, Validators.required],
    rolId:  [0, Validators.required],
  });

  ngOnInit(): void {
    if (this._model) {
      this.form.patchValue({ userId: this._model.userId, rolId: this._model.rolId });
    }
  }

  save() {
    const form = this.form.value as RolUserSelectModel;
    this.posteoForm.emit(form);
  }
}
