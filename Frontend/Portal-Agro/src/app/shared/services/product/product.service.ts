import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ProductRegisterModel, ProductSelectModel, ProductUpdateModel } from './../../models/product/product.model';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  /** Base URL del API (apiUrl + /Product) */
  private readonly urlBase = `${environment.apiUrl}Product`;

  constructor(private http: HttpClient) {}

  /** --------------------------------------------------  CRUD  ----------------------------------------------------- */
  /** Obtener todos los productos  */
  getAll(): Observable<ProductSelectModel[]> {
    return this.http.get<ProductSelectModel[]>(this.urlBase);
  }
   /** Obtener productos por productor (requiere endpoint GET /Product/by-producer */
  getByProducerId(): Observable<ProductSelectModel[]> {
    return this.http.get<ProductSelectModel[]>(this.urlBase + '/by-producer');
  }

  /** Obtener un producto por ID */
  getById(id: number): Observable<ProductSelectModel> {
    return this.http.get<ProductSelectModel>(`${this.urlBase}/${id}`);
  }

  /** Obtener productos por productor (requiere endpoint GET /Product/by-producer/{producerId:int}) */
  // getByProducer(producerId: number): Observable<ProductSelectModel[]> {
  //   return this.http.get<ProductSelectModel[]>(
  //     `${this.urlBase}/by-producer/${producerId}`
  //   );
  // }

  /** Eliminar un producto por ID */
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.urlBase}/${id}`);
  }

  /** ------------------------  CREATE  ------------------------- */
  /** POST /Product/register/product  (FromForm ProductCreateDto) */
  create(dto: ProductRegisterModel): Observable<ProductSelectModel> {
    const fd = this.buildFormData(dto);
    return this.http.post<ProductSelectModel>(
      `${this.urlBase}/register/product`,
      fd
    );
  }

  /** ------------------------  UPDATE  ------------------------- */
  /** PUT /Product/{id:int} (FromForm ProductUpdateDto) */
  update(dto: ProductUpdateModel): Observable<ProductSelectModel> {
    if (!dto.id) throw new Error('ID del producto es obligatorio');

    const fd = this.buildFormData(dto);
    return this.http.put<ProductSelectModel>(`${this.urlBase}/${dto.id}`, fd);
  }

  /**
   * *FormData* de multipart/form-data
   *
   * Se encarga de montar el objeto que el *ASP.NET Core* espera.
   * <br><br>
   * <b>Campos que envía</b>
   * | Campo         | Tipo      | Comentario                                   |
   * |---------------|-----------|-----------------------------------------------|
   * | id            | number    | (solo en Update)                              |
   * | name          | string    | obligatorio                                   |
   * | description   | string    | obligatorio                                   |
   * | price         | number    | obligatorio                                   |
   * | unit          | string    | obligatorio                                   |
   * | production    | string    | obligatorio                                   |
   * | stock         | number    | obligatorio                                   |
   * | status        | boolean   | obligatorio                                   |
   * | categoryId    | number    | obligatorio                                   |
   * | farmId        | number    | obligatorio                                   |
   * | images        | File[]    | nuevos archivos (Create y Update)             |
   * | imagesToDelete| string[]  | PublicId a borrar (solo en Update)            |
   *
   * • En *CREATE* y *UPDATE* los archivos se envían en la clave `images` (coincide con tu DTO).<br>
   * • En *UPDATE* además se envían `imagesToDelete` como claves repetidas para List<string>.
   */
  private buildFormData(
    dto: ProductRegisterModel | ProductUpdateModel
  ): FormData {
    const data = new FormData();

    /* ---------------------------------  Campos básicos  -------------------------------- */
    if ('id' in dto && dto.id !== undefined) {
      data.append('id', String(dto.id));
    }

    data.append('name', dto.name);
    data.append('description', dto.description);
    data.append('price', String(dto.price));
    data.append('unit', dto.unit);
    data.append('production', dto.production);
    data.append('stock', String(dto.stock));
    data.append('status', String(dto.status));
    data.append('categoryId', String(dto.categoryId));
    data.append('farmId', String(dto.farmId));

    /* ---------------------------------  Imágenes nuevas  -------------------------------- */
    if (dto.images?.length) {
      dto.images.forEach((file) => data.append('images', file, file.name));
    }

    /* -------  Lista de publicId a borrar (solo en Update; List<string> en ASP.NET Core)  ------- */
    if ('imagesToDelete' in dto && dto.imagesToDelete?.length) {
      // Enviar como claves repetidas permite el binding directo a List<string>
      dto.imagesToDelete.forEach((pubId) =>
        data.append('imagesToDelete', pubId)
      );

      // Si prefieres JSON, cambia la línea anterior por:
      // data.append('imagesToDelete', JSON.stringify(dto.imagesToDelete));
      // y deserializa manualmente en el backend.
    }

    return data;
  }
}
