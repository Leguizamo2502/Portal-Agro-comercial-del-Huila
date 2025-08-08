import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from "@angular/material/input";
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { DepartmentRegisterModel, DepartmentSelectModel } from '../../../models/department/department.model';

@Component({
  selector: 'app-deparment-form',
  imports: [MatFormFieldModule, ReactiveFormsModule, MatInputModule, MatButtonModule, RouterLink, MatIconModule, CommonModule],
  templateUrl: './deparment-form.component.html',
  styleUrl: './deparment-form.component.css'
})
export class DepartmentFormComponent implements OnInit{
  formBuilder = inject(FormBuilder);
  
  @Input({ required: true })
  title!: string;
  
  private _model?: DepartmentSelectModel;

  @Input()
  set model(value: DepartmentSelectModel | undefined) {
    this._model = value;
    if (value) {
      this.form.patchValue(value);
    }
  }

  get model() {
    return this._model;
  }
  
  @Output()
  posteoForm = new EventEmitter<DepartmentRegisterModel>()
  
  
  form = this.formBuilder.group({
    name: ['',Validators.required],
  })
  
  ngOnInit(): void {
    if(this.model !== undefined) {
      this.form.patchValue(this.model)
    }
  }
  save() {
    let form = this.form.value as DepartmentRegisterModel;
    this.posteoForm.emit(form)
  }
}
