import { Injectable } from '@angular/core';
import { GenericService } from '../../../features/security/services/generic/generic.service';
import { PersonRegisterModel, PersonSelectModel } from '../../models/person/person.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PersonService extends GenericService<PersonSelectModel,PersonRegisterModel> {

  private urlBase = environment.apiUrl + "Person";
  constructor(http : HttpClient) { 
    super(http,'Person')
  }

  getDataBasic():Observable<PersonSelectModel>{
    return this.http.get<PersonSelectModel>(this.urlBase)
  }
}
