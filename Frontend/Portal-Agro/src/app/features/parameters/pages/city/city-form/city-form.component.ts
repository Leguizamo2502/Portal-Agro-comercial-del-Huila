import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from "@angular/material/input";
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CityRegisterModel, CitySelectModel } from '../../../models/city/city.model';

@Component({
  selector: 'app-permission-form',
  imports: [MatFormFieldModule, ReactiveFormsModule, MatInputModule, MatButtonModule, RouterLink, MatIconModule, CommonModule],
  templateUrl: './city-form.component.html',
  styleUrl: './city-form.component.css'
})
export class CityFormComponent implements OnInit{
  formBuilder = inject(FormBuilder);
  
  @Input({ required: true })
  title!: string;
  
  private _model?: CitySelectModel;

  @Input()
  set model(value: CitySelectModel | undefined) {
    this._model = value;
    if (value) {
      this.form.patchValue(value);
    }
  }

  get model() {
    return this._model;
  }
  
  @Output()
  posteoForm = new EventEmitter<CityRegisterModel>()
  
  
  form = this.formBuilder.group({
    name: ['',Validators.required],
    nameDpto: ['',Validators.required],

  })
  
  ngOnInit(): void {
    if(this.model !== undefined) {
      this.form.patchValue(this.model)
    }
  }
  save() {
    let form = this.form.value as CityRegisterModel;
    this.posteoForm.emit(form)
  }
}
