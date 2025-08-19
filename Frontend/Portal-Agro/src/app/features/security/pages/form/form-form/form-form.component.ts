import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from "@angular/material/input";
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { ButtonComponent } from "../../../../../shared/components/button/button.component";
import { FormRegisterModel, FormSelectModel } from '../../../models/form/form.model';

@Component({
  selector: 'app-form-form',
  imports: [MatFormFieldModule, ReactiveFormsModule, MatInputModule, MatButtonModule, MatIconModule, CommonModule, ButtonComponent],
  templateUrl: './form-form.component.html',
  styleUrl: './form-form.component.css'
})
export class FormFormComponent implements OnInit{
  
  formBuilder = inject(FormBuilder);
  
  @Input({ required: true })
  title!: string;
  
  private _model?: FormSelectModel;

  @Input()
  set model(value: FormSelectModel | undefined) {
    this._model = value;
    if (value) {
      this.form.patchValue(value);
    }
  }

  get model() {
    return this._model;
  }
  
  @Output()
  posteoForm = new EventEmitter<FormRegisterModel>()
  
  
  form = this.formBuilder.group({
    name: ['',Validators.required],
    description: ['',Validators.required],
    url: ['',Validators.required]

  })
  
  ngOnInit(): void {
    if(this.model !== undefined) {
      this.form.patchValue(this.model)
    }
  }
  save() {
    let form = this.form.value as FormRegisterModel;
    this.posteoForm.emit(form)
  }

}
