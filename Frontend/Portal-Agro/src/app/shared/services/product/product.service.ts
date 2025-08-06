import { ProductSelectModel } from './../../models/product/product.model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + 'Product';

  public getProduct():Observable<ProductSelectModel[]>{
    return this.http.get<ProductSelectModel[]>(this.urlBase)
  }

}
