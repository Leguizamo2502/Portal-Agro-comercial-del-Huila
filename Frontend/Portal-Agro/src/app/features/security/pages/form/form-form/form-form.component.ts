import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { formRegisterModel, formSelectModel } from '../../../models/form/form.model';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from "@angular/material/input";
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-form-form',
  imports: [[MatFormFieldModule, ReactiveFormsModule, MatInputModule, MatButtonModule, RouterLink, MatIconModule, CommonModule]],
  templateUrl: './form-form.component.html',
  styleUrl: './form-form.component.css'
})
export class FormFormComponent implements OnInit{
  
  formBuilder = inject(FormBuilder);
  
  @Input({ required: true })
  title!: string;
  
  @Input()
  model?: formSelectModel;
  
  @Output()
  posteoForm = new EventEmitter<formRegisterModel>()
  
  
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
  gurdarCambios() {
    let form = this.form.value as formRegisterModel;
    this.posteoForm.emit(form)
  }

}
