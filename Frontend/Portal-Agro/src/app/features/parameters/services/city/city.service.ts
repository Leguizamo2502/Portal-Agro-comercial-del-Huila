import { Injectable } from '@angular/core';
import { GenericService } from '../../../security/services/generic/generic.service';
import { CityRegisterModel, CitySelectModel } from '../../models/city/city.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CityService extends GenericService<CitySelectModel,CityRegisterModel>{

  constructor(http:HttpClient) { 
    super(http,'City')
  }
}
