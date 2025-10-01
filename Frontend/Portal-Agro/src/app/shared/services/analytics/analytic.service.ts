import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { TopProductsResponse } from '../../models/analytics/analytic.model';

@Injectable({
  providedIn: 'root'
})
export class AnalyticService {

  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + 'Analytics'
  constructor() { }

  getTopProducts(limit = 5): Observable<TopProductsResponse> {
    const params = new HttpParams().set('limit', limit);
    return this.http.get<TopProductsResponse>(`${this.urlBase}/top-products`, { params });
  }
}
