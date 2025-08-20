import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { map, Observable } from 'rxjs';
import { FavoriteCreateRequest } from '../../models/product/product.model';

@Injectable({
  providedIn: 'root',
})
export class FavoriteService {
  private readonly urlBase = `${environment.apiUrl}Product`;

  constructor(private http: HttpClient) {}
  /** --------------------------------------------------  Favorite ----------------------------------------------------- */
  addFavorite(productId: number): Observable<boolean> {
    return this.http.post(`${this.urlBase}/register/favorite`, { productId }, { observe: 'response' })
      .pipe(map(r => r.status === 201 || r.status === 204));
  }

  remove(productId: number): Observable<boolean> {
    return this.http.delete(`${this.urlBase}/favorite/${productId}`, { observe: 'response' })
      .pipe(map(r => r.status === 204));
  }

  /** Toggle: devuelve el nuevo estado (true = ahora es favorito) */
  toggle(productId: number, currentlyFavorite: boolean): Observable<boolean> {
    return (currentlyFavorite ? this.remove(productId) : this.addFavorite(productId))
      .pipe(map(ok => ok ? !currentlyFavorite : currentlyFavorite));
  }
}
