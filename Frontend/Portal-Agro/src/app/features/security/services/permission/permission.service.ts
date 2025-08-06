import { Injectable } from '@angular/core';
import { GenericService } from '../generic/generic.service';
import { PermissionRegisterModel, PermissionSelectModel } from '../../models/permission/permission.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PermissionService extends GenericService<PermissionSelectModel,PermissionRegisterModel>{

  constructor(http:HttpClient) {
    super(http,'permission')
   }
}
