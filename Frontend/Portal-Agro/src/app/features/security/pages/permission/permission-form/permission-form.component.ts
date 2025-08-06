import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { PermissionRegisterModel, PermissionSelectModel } from '../../../models/permission/permission.model';
import { MatInputModule } from "@angular/material/input";
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-permission-form',
  imports: [MatFormFieldModule, ReactiveFormsModule, MatInputModule, MatButtonModule, RouterLink, MatIconModule, CommonModule],
  templateUrl: './permission-form.component.html',
  styleUrl: './permission-form.component.css'
})
export class PermissionFormComponent implements OnInit{
  formBuilder = inject(FormBuilder);
  
  @Input({ required: true })
  title!: string;
  
  private _model?: PermissionSelectModel;

  @Input()
  set model(value: PermissionSelectModel | undefined) {
    this._model = value;
    if (value) {
      this.form.patchValue(value);
    }
  }

  get model() {
    return this._model;
  }
  
  @Output()
  posteoForm = new EventEmitter<PermissionRegisterModel>()
  
  
  form = this.formBuilder.group({
    name: ['',Validators.required],
    description: ['',Validators.required],

  })
  
  ngOnInit(): void {
    if(this.model !== undefined) {
      this.form.patchValue(this.model)
    }
  }
  save() {
    let form = this.form.value as PermissionRegisterModel;
    this.posteoForm.emit(form)
  }
}
