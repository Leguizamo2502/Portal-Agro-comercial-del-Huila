import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { LoginModel, LoginResponseModel } from '../../Models/login.model';
import { RegisterUserModel } from '../../Models/registeruser.model';
import { UserSelectModel } from '../../Models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + 'Auth/';
  constructor() {}

  Register(Objeto: RegisterUserModel): Observable<any> {
    return this.http.post<any>(this.urlBase + 'Register', Objeto);
  }

  Login(Objeto: LoginModel): Observable<any> {
    return this.http.post<any>(this.urlBase + 'login', Objeto);
  }

  LogOut():Observable<any>{
    return this.http.post<any>(this.urlBase+"logout",[])
  }

  GetMe(): Observable<LoginResponseModel> {
    return this.http.get<LoginResponseModel>(this.urlBase + 'me');
  }

  GetDataBasic():Observable<UserSelectModel>{
    return this.http.get<UserSelectModel>(this.urlBase+"DataBasic")
  }
}
