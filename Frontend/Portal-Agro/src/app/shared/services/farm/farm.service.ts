import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { FarmSelectModel } from '../../models/farm/farm.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FarmService {
  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + 'Farm';

  public getFarms():Observable<FarmSelectModel[]>{
    return this.http.get<FarmSelectModel[]>(this.urlBase)
  }
  constructor() { }
}
