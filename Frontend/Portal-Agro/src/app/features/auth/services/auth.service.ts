import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { LoginModel, LoginResponseModel } from '../Models/login.model';
import { RegisterUserModel } from '../Models/registeruser.model';

@Injectable({
  providedIn: 'root',
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

  GetMe(): Observable<LoginResponseModel> {
    return this.http.get<LoginResponseModel>(this.urlBase + 'me');
  }

  // 🔹 Implementación de logout
  LogOut(): Observable<any> {
    // si tu backend tiene endpoint de logout:
    // return this.http.post(this.urlBase + 'logout', {});

    // si NO tiene endpoint: simplemente limpia el token local
    localStorage.removeItem('token');
    return of({ success: true });
  }
}
