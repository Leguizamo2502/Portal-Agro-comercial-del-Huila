import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { RegisterUserModel } from '../Models/registeruser.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + 'Auth/';
  constructor() { }


  Register(Objeto:RegisterUserModel):Observable<any>{
    return this.http.post<any>(this.urlBase + 'Register', Objeto);
  }
}
