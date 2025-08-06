import { Injectable } from '@angular/core';
import { GenericService } from '../generic/generic.service';
import { ModuleSelectModel, ModuleRegisterModel } from '../../models/module/module.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ModuleService extends GenericService<ModuleSelectModel, ModuleRegisterModel> {

   constructor(http: HttpClient) {
    super(http, 'module');
    
  }
}
