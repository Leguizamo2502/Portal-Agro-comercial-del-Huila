import { inject, Injectable } from '@angular/core';
import { FarmImageSelectModel } from '../../models/farm/farm.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class FarmImageService {
  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + 'FarmImage'
  constructor() { }

  /** Obtener imágenes de un finca */
  getImagesByFarmId(id: number): Observable<FarmImageSelectModel[]> {
    return this.http.get<FarmImageSelectModel[]>(`${this.urlBase}/${id}`);
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
