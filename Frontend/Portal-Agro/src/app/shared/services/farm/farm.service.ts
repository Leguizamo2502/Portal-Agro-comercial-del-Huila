import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import {
  FarmRegisterModel,
  FarmSelectModel,
  FarmUpdateModel,
  FarmWithProducerRegisterModel,
} from '../../models/farm/farm.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FarmService {
  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + 'Farm';

  /** Obtener todas las fincas */
  public getAll(): Observable<FarmSelectModel[]> {
    return this.http.get<FarmSelectModel[]>(this.urlBase);
  }

  /** Obtener fincas de prodcutor por code */
  public getFarmByCodeProducer(codeProducer: string): Observable<FarmSelectModel[]> {
    return this.http.get<FarmSelectModel[]>(
      `${this.urlBase}/by-producerCode/${codeProducer}`
    );
  }

  /** Obtener una finca por ID */
  public getById(id: number): Observable<FarmSelectModel> {
    return this.http.get<FarmSelectModel>(`${this.urlBase}/${id}`);
  }

  /** Obtener fincas por productor autenticado */
  public getByProducer(): Observable<FarmSelectModel[]> {
    return this.http.get<FarmSelectModel[]>(`${this.urlBase}/by-producer`);
  }

  /** Crear finca junto con productor */
  public createWithProducer(
    dto: FarmWithProducerRegisterModel
  ): Observable<any> {
    const fd = this.buildFormData(dto);
    return this.http.post<any>(`${this.urlBase}/registrar/producer`, fd);
  }

  /** Crear finca (productor ya existente) */
  public create(dto: FarmRegisterModel): Observable<any> {
    const fd = this.buildFormData(dto);
    return this.http.post<any>(`${this.urlBase}/register/farm`, fd);
  }

  /** Actualizar finca */
  public update(dto: FarmUpdateModel): Observable<FarmSelectModel> {
    if (!dto.id) throw new Error('ID de la finca es obligatorio');

    const fd = this.buildFormData(dto);
    return this.http.put<FarmSelectModel>(`${this.urlBase}/${dto.id}`, fd);
  }

  /** Eliminar finca */
  public delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.urlBase}/${id}`);
  }

  /**
   * Construcción de FormData compatible con ASP.NET Core
   */
  private buildFormData(
    dto: FarmRegisterModel | FarmWithProducerRegisterModel | FarmUpdateModel
  ): FormData {
    const data = new FormData();

    if ('id' in dto && dto.id !== undefined) {
      data.append('id', String(dto.id));
    }

    data.append('name', dto.name);
    data.append('hectares', String(dto.hectares));
    data.append('altitude', String(dto.altitude));
    data.append('latitude', String(dto.latitude));
    data.append('longitude', String(dto.longitude));
    data.append('cityId', String(dto.cityId));

    if ('description' in dto && dto.description !== undefined) {
      data.append('description', dto.description);
    }

    if (dto.images?.length) {
      dto.images.forEach((file) => data.append('images', file, file.name));
    }

    if ('imagesToDelete' in dto && dto.imagesToDelete?.length) {
      dto.imagesToDelete.forEach((pubId) =>
        data.append('imagesToDelete', pubId)
      );
    }

    return data;
  }
}
