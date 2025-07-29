import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { LoginResponseModel } from '../../../features/auth/Models/login.model';

@Injectable({
  providedIn: 'root'
})
export class MeService {

  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + 'Auth/';
  constructor() {}

  GetMe(): Observable<LoginResponseModel> {
    return this.http.get<LoginResponseModel>(this.urlBase + 'me');
  }
}
