import { Injectable } from '@angular/core';
import { GenericService } from '../generic/generic.service';
import { RolFormPermissionRegisterModel, RolFormPermissionSelectModel } from '../../models/rolFormPermission/rolFormPermission.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RolFormPermissionService extends GenericService<RolFormPermissionSelectModel,RolFormPermissionRegisterModel> {

  constructor(http:HttpClient) {
    super(http,'rolFormPermission')
   }
}
