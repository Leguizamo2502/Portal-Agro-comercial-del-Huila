import { Injectable, signal } from '@angular/core';
import { Subject, Observable, EMPTY, tap, finalize } from 'rxjs';
import { ProductSelectModel } from '../../models/product/product.model';
import { FavoriteService } from './favorite.service';

@Injectable({
  providedIn: 'root',
})
export class FavoriteFacadeService {
  private _inFlight = signal<Set<number>>(new Set<number>());
  private _changes = new Subject<{ id: number; isFavorite: boolean }>();
  changes$ = this._changes.asObservable();

  constructor(private api: FavoriteService) {}

  isToggling(id?: number): boolean {
    return !!id && this._inFlight().has(id);
  }

  toggle(product: ProductSelectModel): Observable<boolean> {
    if (!product?.id) return EMPTY;
    if (this.isToggling(product.id)) return EMPTY;

    const original = !!product.isFavorite;
    product.isFavorite = !original;           // optimista
    this.mark(product.id, true);

    return this.api.toggle(product.id, original).pipe(
      tap({
        next: (newState) => {
          product.isFavorite = newState;
          this._changes.next({ id: product.id, isFavorite: newState });
        },
        error: () => { product.isFavorite = original; }, // rollback
      }),
      finalize(() => this.mark(product.id, false))
    );
  }

  private mark(id: number, on: boolean) {
    const s = new Set(this._inFlight());
    on ? s.add(id) : s.delete(id);
    this._inFlight.set(s);
  }
}
