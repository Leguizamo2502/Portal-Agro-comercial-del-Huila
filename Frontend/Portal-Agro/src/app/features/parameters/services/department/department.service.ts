import { Injectable } from '@angular/core';
import { GenericService } from '../../../security/services/generic/generic.service';
import { DepartmentRegisterModel, DepartmentSelectModel } from '../../models/department/department.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService extends GenericService<DepartmentSelectModel,DepartmentRegisterModel>{

  constructor(http:HttpClient) {
    super(http,'Department')
   }
}
