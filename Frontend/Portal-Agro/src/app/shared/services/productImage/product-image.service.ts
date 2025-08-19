import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ProductImageSelectModel } from '../../models/product/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductImageService {
  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + 'ProductImage'
  constructor() { }

  /** Obtener imágenes de un producto */
  getImagesByProductId(id: number): Observable<ProductImageSelectModel[]> {
    return this.http.get<ProductImageSelectModel[]>(`${this.urlBase}/${id}`);
  }

  /** Eliminar varias imágenes a la vez mediante sus publicId */
  deleteImagesByPublicIds(publicIds: string[]): Observable<void> {
    return this.http.delete<void>(`${this.urlBase}/multiple`, { body: publicIds });
  }

  /** Eliminar lógicamente una imagen por su publicId */
  logicalDeleteImage(publicId: string): Observable<void> {
    return this.http.patch<void>(`${this.urlBase}/logical-delete`, null, { params: { publicId } });
  }
}
