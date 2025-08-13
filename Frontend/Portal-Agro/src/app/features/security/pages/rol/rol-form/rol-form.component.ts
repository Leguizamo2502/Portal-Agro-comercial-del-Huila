import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RolSelectModel, RolRegisterModel } from '../../../models/rol/rol.model';
import { MatInputModule } from "@angular/material/input";
import { ButtonComponent } from "../../../../../shared/components/button/button.component";

@Component({
  selector: 'app-rol-form',
  imports: [MatInputModule, ReactiveFormsModule, ButtonComponent],
  templateUrl: './rol-form.component.html',
  styleUrl: './rol-form.component.css'
})
export class RolFormComponent {
  formBuilder = inject(FormBuilder);
  
  @Input({ required: true })
  title!: string;
  
  private _model?: RolSelectModel;

  @Input()
  set model(value: RolSelectModel | undefined) {
    this._model = value;
    if (value) {
      this.form.patchValue(value);
    }
  }

  get model() {
    return this._model;
  }
  
  @Output()
  posteoForm = new EventEmitter<RolRegisterModel>()
  
  
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
    let form = this.form.value as RolRegisterModel;
    this.posteoForm.emit(form)
  }
}
