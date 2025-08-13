import { Injectable } from '@angular/core';
import { GenericService } from '../generic/generic.service';
import { RolUserRegisterModel, RolUserSelectModel } from '../../models/rolUser/rolUser.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RolUserService extends GenericService<RolUserSelectModel,RolUserRegisterModel>{

  constructor(http:HttpClient) { 
    super(http,'rolUser')
  }
}
