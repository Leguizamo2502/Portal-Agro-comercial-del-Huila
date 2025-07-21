import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { RegisterUserModel } from '../Models/registeruser.model';
import { Observable } from 'rxjs';
import { LoginModel, LoginResponseModel } from '../Models/login.model';

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

  Login(Objeto:LoginModel):Observable<any>{
    return this.http.post<any>(this.urlBase, Objeto);
  }

  GetMe(): Observable<LoginResponseModel>{
    return this.http.get<LoginResponseModel>(this.urlBase + 'me');
  }

}
