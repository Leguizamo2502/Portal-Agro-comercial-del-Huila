import { Injectable } from '@angular/core';
import { GenericService } from '../generic/generic.service';
import { FormModuleRegisterModel, FormModuleSelectModel } from '../../models/formModule/formModule.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class FormModuleService extends GenericService<FormModuleSelectModel,FormModuleRegisterModel>{

  constructor(http:HttpClient) { 
    super(http,'formModule')
  }
}
