import { HttpClient, HttpContext } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, of, switchMap } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { LoginModel, LoginResponseModel, UserMeDto } from '../../Models/login.model';
import { RegisterUserModel } from '../../Models/registeruser.model';
import { PersonUpdateModel, UserSelectModel } from '../../Models/user.model';
import { ChangePasswordModel, RecoverPasswordConfirmModel, RecoverPasswordModel } from '../../Models/changePassword.model';
import { OPTIONAL_AUTH } from '../../interceptors/auth/auth-optional.token';

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

  ChangePassword(Objeto: ChangePasswordModel): Observable<any> {
    return this.http.put<any>(this.urlBase + 'ChangePassword', Objeto);
  }

  GetMe(): Observable<UserMeDto> {
    return this.http.get<UserMeDto>(this.urlBase + 'me');
  }

  GetMeOptional(): Observable<UserMeDto> {
    const context = new HttpContext().set(OPTIONAL_AUTH, true);
    return this.http.get<UserMeDto>(this.urlBase + 'me', { context });
  }

  GetDataBasic():Observable<UserSelectModel>{
    return this.http.get<UserSelectModel>(this.urlBase+"DataBasic")
  }

  LogOut(): Observable<any> {
    return this.http.post<any>(this.urlBase + 'logout', []);
  }

  RefreshToken(): Observable<UserMeDto> {
    return this.http.post<any>(this.urlBase + 'refresh', {}, { withCredentials: true }).pipe(
      switchMap(() => this.GetMe())
    );
  }

  UpdatePerson(objeto:PersonUpdateModel):Observable<any>{
    return this.http.put<any>(this.urlBase+"UpdatePerson",objeto)
  }

  RequestRecoverPassword(objeto: RecoverPasswordModel): Observable<any> {
    return this.http.post<any>(this.urlBase + 'recover/send-code', objeto);
  }

  ConfirmRecoverPassword(objeto: RecoverPasswordConfirmModel): Observable<any> {
    return this.http.post<any>(this.urlBase + 'recover/confirm', objeto);
  }

  
}
