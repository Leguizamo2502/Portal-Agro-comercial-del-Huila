import { Injectable } from '@angular/core';
import { GenericService } from '../../../security/services/generic/generic.service';
import {
  CategoryNodeModel,
  CategoryRegistertModel,
  CategorySelectModel,
} from '../../models/category/category.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, shareReplay } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CategoryService extends GenericService<
  CategorySelectModel,
  CategoryRegistertModel
> {
  private cache = new Map<string, Observable<CategoryNodeModel[]>>();

  constructor(http: HttpClient) {
    super(http, 'Category');
  }

  /**
   * GET /api/v1/categories/node?parentId=
   * - parentId omitido/null => categorías raíz.
   * - parentId con valor    => hijas de esa categoría.
   */
  public getNodes(parentId?: number | null): Observable<CategoryNodeModel[]> {
    const url = `${this.endpoint}/node`;

    let params = new HttpParams();
    if (parentId !== null && parentId !== undefined) {
      params = params.set('parentId', String(parentId));
    }

    // Cache sencillo por nivel para evitar peticiones repetidas
    const key =
      parentId !== null && parentId !== undefined
        ? `node:${parentId}`
        : 'node:root';
    if (!this.cache.has(key)) {
      const req$ = this.http
        .get<CategoryNodeModel[]>(url, { params })
        .pipe(shareReplay(1));
      this.cache.set(key, req$);
    }
    return this.cache.get(key)!;
  }
}
