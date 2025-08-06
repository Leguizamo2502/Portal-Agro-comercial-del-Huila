import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from "@angular/material/input";
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { ModuleRegisterModel, ModuleSelectModel } from '../../../models/module/module.model';

@Component({
  selector: 'app-form-form',
  imports: [[MatFormFieldModule, ReactiveFormsModule, MatInputModule, MatButtonModule, RouterLink, MatIconModule, CommonModule]],
  templateUrl: '../module-module/module-module.component.html',
  styleUrl: '../module-module/module-module.component.css'
})
export class ModuleModuleComponent implements OnInit{
  
  formBuilder = inject(FormBuilder);
  
  @Input({ required: true })
  title!: string;
  
  private _model?: ModuleSelectModel;

  @Input()
  set model(value: ModuleSelectModel | undefined) {
    this._model = value;
    if (value) {
      this.form.patchValue(value);
    }
  }

  get model() {
    return this._model;
  }
  
  @Output()
  posteoModule = new EventEmitter<ModuleRegisterModel>()
  
  
  form = this.formBuilder.group({
    name: ['',Validators.required],
    description: ['',Validators.required],
  })
  
  ngOnInit(): void {
    if(this.model !== undefined) {
      this.form.patchValue(this.model)
    }
  }
  gurdarCambios() {
    let form = this.form.value as ModuleRegisterModel;
    this.posteoModule.emit(form)
  }

}
