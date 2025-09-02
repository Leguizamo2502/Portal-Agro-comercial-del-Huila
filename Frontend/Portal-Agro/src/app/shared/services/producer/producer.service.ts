import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { ProducerSelectModel } from '../../models/producer/producer.model';

@Injectable({
  providedIn: 'root',
})
export class ProducerService {
  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + 'Producer';

  /** Obtener productor por código (lanza error si 404) */
  getByCodeProducer(codeProducer: string): Observable<ProducerSelectModel> {
    return this.http.get<ProducerSelectModel>(
      `${this.urlBase}/by-code/${codeProducer}`
    );
  }
}
