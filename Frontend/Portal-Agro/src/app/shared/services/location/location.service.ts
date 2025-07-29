import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { CityModel, DepartmentModel } from '../../models/location/location.model';

@Injectable({
  providedIn: 'root'
})
export class LocationService {
  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + 'location';
  constructor() { }

  getDepartment():Observable<DepartmentModel[]>{
    return this.http.get<DepartmentModel[]>(this.urlBase+'/Department');
  }

  getCity(id:number):Observable<CityModel[]>{
    return this.http.get<CityModel[]>(`${this.urlBase}/Department/City/${id}`);
  }

}
