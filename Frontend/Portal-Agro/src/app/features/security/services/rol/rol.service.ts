import { Injectable } from '@angular/core';
import { GenericService } from '../generic/generic.service';
import { RolRegisterModel, RolSelectModel } from '../../models/rol/rol.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RolService extends GenericService<RolSelectModel,RolRegisterModel>{

  constructor(http:HttpClient) { 
    super(http,'rol')
  }
}
