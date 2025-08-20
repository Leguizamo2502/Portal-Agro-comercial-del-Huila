import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from "@angular/material/input";
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from '@angular/material/button';
import { NgIf, NgFor } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CityRegisterModel, CitySelectModel } from '../../../models/city/city.model';
import { DepartmentSelectModel } from '../../../models/department/department.model';
import { MatSelectModule } from '@angular/material/select';
import { ButtonComponent } from "../../../../../shared/components/button/button.component";

@Component({
  selector: 'app-city-form',
  standalone: true,
  imports: [
    MatFormFieldModule, ReactiveFormsModule, MatInputModule, MatButtonModule,
    MatIconModule, MatSelectModule,
    NgIf, NgFor,
    ButtonComponent
  ],
  templateUrl: './city-form.component.html',
  styleUrls: ['./city-form.component.css']
})
export class CityFormComponent implements OnInit {
  private formBuilder = inject(FormBuilder);
  
  @Input({ required: true }) title!: string;

  // Lista de departamentos que vendrá del padre
  @Input() departments: DepartmentSelectModel[] = [];
  
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
  
  @Output() posteoForm = new EventEmitter<CityRegisterModel>();
  
  form = this.formBuilder.group({
    name: ['', Validators.required],
    departmentId: [null, Validators.required],
  });
  
  ngOnInit(): void {
    if (this.model !== undefined) {
      this.form.patchValue(this.model);
    }
  }

  save() {
    const form = this.form.value as CityRegisterModel;
    this.posteoForm.emit(form);
  }
}
