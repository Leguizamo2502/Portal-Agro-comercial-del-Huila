import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment';
import { Observable } from 'rxjs';
import { UserSelectModel } from '../../../../Core/Models/user.model';

@Injectable({
  providedIn: 'root'
})


export class UserService {
  private http = inject(HttpClient);
  private urlBase = environment.apiUrl +'User'
  constructor() { }

  getUser():Observable<UserSelectModel[]>{
    return this.http.get<UserSelectModel[]>(this.urlBase)
  }

  public deleteLogic(id: number): Observable<any> {
    return this.http.patch(`${this.urlBase}/${id}`,[]);
  }
}
