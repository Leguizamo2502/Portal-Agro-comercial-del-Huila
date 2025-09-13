import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  ProductRegisterModel,
  ProductSelectModel,
  ProductUpdateModel,
  ApiOk,
} from './../../models/product/product.model';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private readonly urlBase = `${environment.apiUrl}Product`;

  constructor(private http: HttpClient) {}

  /** --------------------------------------------------  Favorites  ----------------------------------------------------- */
  getAllHome(): Observable<ProductSelectModel[]> {
    return this.http.get<ProductSelectModel[]>(this.urlBase + '/home');
  }

  getFavorites(): Observable<ProductSelectModel[]> {
    return this.http.get<ProductSelectModel[]>(this.urlBase + '/favorites');
  }

  /** ------------------------  FILTER BY CATEGORY  ------------------------- */
  /**
   * Productos de la categoría indicada y todas sus subcategorías.
   * Endpoint: GET /api/v1/categories/{categoryId}/products
   */
  getByCategory(categoryId: number): Observable<ProductSelectModel[]> {
    if (!categoryId || categoryId <= 0) throw new Error('categoryId inválido');
    const url = `${this.urlBase}/categories/${categoryId}/products`;
    return this.http.get<ProductSelectModel[]>(url);
  }

  /** --------------------------------------------------  CRUD  ----------------------------------------------------- */
  getAll(): Observable<ProductSelectModel[]> {
    return this.http.get<ProductSelectModel[]>(this.urlBase);
  }

  getByProducerId(): Observable<ProductSelectModel[]> {
    return this.http.get<ProductSelectModel[]>(this.urlBase + '/by-producer');
  }

  /** Obtener fincas de prodcutor por code */
  public getProductByCodeProducer(
    codeProducer: string
  ): Observable<ProductSelectModel[]> {
    return this.http.get<ProductSelectModel[]>(
      `${this.urlBase}/by-producerCode/${codeProducer}`
    );
  }

  getById(id: number): Observable<ProductSelectModel> {
    return this.http.get<ProductSelectModel>(`${this.urlBase}/${id}`);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.urlBase}/${id}`);
  }

  /** ------------------------  CREATE  ------------------------- */
  create(dto: ProductRegisterModel): Observable<ApiOk> {
    const fd = this.buildFormData(dto);
    return this.http.post<ApiOk>(`${this.urlBase}/register/product`, fd);
  }

  /** ------------------------  UPDATE  ------------------------- */
  update(dto: ProductUpdateModel): Observable<ApiOk> {
    if (!dto.id) throw new Error('ID del producto es obligatorio');
    const fd = this.buildFormData(dto);
    return this.http.put<ApiOk>(`${this.urlBase}/${dto.id}`, fd);
  }

  /**
   * Multipart FormData que el backend espera.
   *
   * Campos:
   * - id             (solo en Update)
   * - name
   * - description
   * - price
   * - unit
   * - production
   * - stock
   * - status
   * - categoryId
   * - FarmIds        (varias claves repetidas)
   * - images         (File[])
   * - imagesToDelete (varias claves repetidas)
   */
  private buildFormData(
    dto: ProductRegisterModel | ProductUpdateModel
  ): FormData {
    const data = new FormData();

    if ('id' in dto && dto.id !== undefined) data.append('id', String(dto.id));

    data.append('name', dto.name);
    data.append('description', dto.description);
    data.append('price', String(dto.price));
    data.append('unit', dto.unit);
    data.append('production', dto.production);
    data.append('stock', String(dto.stock));
    data.append('status', String(dto.status));
    data.append('categoryId', String(dto.categoryId));

    // << NUEVO: múltiples fincas
    (dto.farmIds ?? []).forEach((fid) => data.append('FarmIds', String(fid)));
    // Alternativa válida: data.append('FarmIds[0]', '1'), etc. (el binder es case-insensitive)

    // Imágenes nuevas
    if (dto.images?.length) {
      dto.images.forEach((file) => data.append('images', file, file.name));
    }

    // Imágenes a borrar (PublicId)
    if ('imagesToDelete' in dto && dto.imagesToDelete?.length) {
      dto.imagesToDelete.forEach((pubId) =>
        data.append('imagesToDelete', pubId)
      );
    }

    return data;
  }
}
