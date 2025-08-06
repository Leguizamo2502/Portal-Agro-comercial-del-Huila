import { Injectable } from '@angular/core';
import { GenericService } from '../generic/generic.service';
import { formRegisterModel, formSelectModel } from '../../models/form/form.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class FormService extends GenericService<formSelectModel,formRegisterModel> {

   constructor(http: HttpClient) {
    super(http, 'form');
    
  }
}
