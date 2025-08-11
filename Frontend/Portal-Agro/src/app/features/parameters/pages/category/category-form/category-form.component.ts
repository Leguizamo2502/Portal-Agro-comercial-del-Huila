import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { CategoryRegistertModel, CategorySelectModel } from '../../../models/category/category.model';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonComponent } from "../../../../../shared/components/button/button.component";
import { MatInputModule } from "@angular/material/input";
import { CommonModule } from '@angular/common';
import { MatOption, MatSelectModule } from "@angular/material/select";

@Component({
  selector: 'app-category-form',
  imports: [ButtonComponent, MatInputModule, CommonModule, MatSelectModule,ReactiveFormsModule,MatOption],
  templateUrl: './category-form.component.html',
  styleUrl: './category-form.component.css'
})
export class CategoryFormComponent implements OnInit{
  formBuilder = inject(FormBuilder);
  
  @Input({ required: true })
  title!: string;
  
  private _model?: CategorySelectModel;

  @Input() set model(value: CategorySelectModel | undefined) {
  this._model = value;
  if (value) {
    this.form.patchValue({
      name: value.name,
      parentCategoryId: value.parentCategoryId != null ? Number(value.parentCategoryId) : null

    });
  }
}

  @Input() parents: CategorySelectModel[] = [];

  get model() {
    return this._model;
  }
  
  @Output()
  posteoForm = new EventEmitter<CategoryRegistertModel>()
  
  
  form = this.formBuilder.group({
    name: ['', Validators.required],
    parentCategoryId: [null as number | null],
  

  })
  
  ngOnInit(): void {
    if(this.model !== undefined) {
      this.form.patchValue(this.model)
    }
  }
  save() {
    let form = this.form.value as CategoryRegistertModel;
    this.posteoForm.emit(form)
  }
}
