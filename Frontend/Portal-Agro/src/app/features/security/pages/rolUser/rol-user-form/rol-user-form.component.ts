// rol-user-form.component.ts
import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ButtonComponent } from '../../../../../shared/components/button/button.component';

import { RolSelectModel } from '../../../models/rol/rol.model';
import { UserSelectModel } from '../../../../../Core/Models/user.model';

export interface RolUserSelectModel {
  id: number;
  rolId: number;
  rolName: string;
  userId: number;
  userName: string;
}
export interface RolUserRegisterModel {
  rolId: number;
  userId: number;
}

@Component({
  selector: 'app-rol-user-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatInputModule, MatSelectModule, ButtonComponent],
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
      this.form.patchValue({ userId: value.userId, rolId: value.rolId });
    }
  }
  get model() { return this._model; }

  @Output() posteoForm = new EventEmitter<RolUserRegisterModel>();

  // Non-nullable + min(1) => > 0
  form = this.fb.nonNullable.group({
    userId: [0, [Validators.required, Validators.min(1)]],
    rolId:  [0, [Validators.required, Validators.min(1)]],
  });

  ngOnInit(): void {
    if (this._model) {
      this.form.patchValue({ userId: this._model.userId, rolId: this._model.rolId });
    }
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const payload: RolUserRegisterModel = this.form.getRawValue();
    this.posteoForm.emit(payload);
  }
}
