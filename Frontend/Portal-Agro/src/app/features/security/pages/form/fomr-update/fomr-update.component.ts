import { Component, inject, Input, numberAttribute, OnInit } from '@angular/core';
import { FormService } from '../../../services/form/form.service';
import { Router } from '@angular/router';
import { formRegisterModel, formSelectModel } from '../../../models/form/form.model';
import { FormFormComponent } from "../form-form/form-form.component";

@Component({
  selector: 'app-fomr-update',
  imports: [FormFormComponent],
  templateUrl: './fomr-update.component.html',
  styleUrl: './fomr-update.component.css'
})
export class FomrUpdateComponent implements OnInit{
  @Input({transform: numberAttribute})
  id!: number;
  
  formService = inject(FormService);
  router = inject(Router);
  
  model? : formSelectModel;
  
  ngOnInit(): void {
    this.formService.getById(this.id).subscribe((form)=>{
      this.model = form;
    })
  }

  save(form:formRegisterModel){
    this.formService.update(this.id,form).subscribe(()=>{
      this.router.navigate(['security/form'])
    })
  }



}
