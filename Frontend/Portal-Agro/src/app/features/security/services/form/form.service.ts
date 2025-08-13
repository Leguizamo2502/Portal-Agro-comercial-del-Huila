import { Injectable } from '@angular/core';
import { GenericService } from '../generic/generic.service';
import { HttpClient } from '@angular/common/http';
import { FormSelectModel, FormRegisterModel } from '../../models/form/form.model';

@Injectable({
  providedIn: 'root'
})
export class FormService extends GenericService<FormSelectModel,FormRegisterModel> {

   constructor(http: HttpClient) {
    super(http, 'form');
    
  }
}
