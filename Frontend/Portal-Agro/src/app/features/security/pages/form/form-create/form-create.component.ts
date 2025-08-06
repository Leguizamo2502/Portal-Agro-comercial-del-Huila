import { Component, inject } from '@angular/core';
import { FormService } from '../../../services/form/form.service';
import { Router } from '@angular/router';
import { formRegisterModel } from '../../../models/form/form.model';
import { FormFormComponent } from "../form-form/form-form.component";

@Component({
  selector: 'app-form-create',
  imports: [FormFormComponent],
  templateUrl: './form-create.component.html',
  styleUrl: './form-create.component.css'
})
export class FormCreateComponent {
  formService = inject(FormService);
  router = inject(Router);

  saveChange(form:formRegisterModel){
    this.formService.create(form).subscribe(()=>{
      this.router.navigate(['security/form']);
      console.log(form);
    })
  }

}
