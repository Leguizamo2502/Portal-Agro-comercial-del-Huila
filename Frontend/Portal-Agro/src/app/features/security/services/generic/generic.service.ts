import { Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// @Injectable({
//   providedIn: 'root'
// })
export class GenericService<TList,TCreate> {

  protected apiUrl = environment.apiUrl;
  protected endpoint: string;

  constructor(protected http: HttpClient, endpointName: string) {
    this.endpoint = `${this.apiUrl}${endpointName}`;
  }

  public getAll(): Observable<TList[]> {
    return this.http.get<TList[]>(this.endpoint);
  }

  public getById(id: number): Observable<TList> {
    return this.http.get<TList>(`${this.endpoint}/${id}`);
  }

  public create(item: TCreate) {
    return this.http.post(this.endpoint, item);
  }

  public update(id: number, item: TCreate) {
    return this.http.put(`${this.endpoint}/${id}`, item);
  }

  public delete(id: number): Observable<any> {
    return this.http.delete(`${this.endpoint}/${id}`);
  }

  public deleteLogic(id: number): Observable<any> {
    return this.http.patch(`${this.endpoint}/${id}`,[]);
  }
}
