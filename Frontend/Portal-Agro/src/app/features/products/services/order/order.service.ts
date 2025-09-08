import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment.development';
import { Observable } from 'rxjs';
import { OrderCreateModel, CreateOrderResponse } from '../../models/order/order.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

   private http = inject(HttpClient);
  private urlBase = environment.apiUrl + 'Order'; 

  create(dto: OrderCreateModel): Observable<CreateOrderResponse> {
    const fd = new FormData();
    fd.append('ProductId', String(dto.productId));
    fd.append('QuantityRequested', String(dto.quantityRequested));
    fd.append('CityId', String(dto.cityId));
    fd.append('PaymentImage', dto.paymentImage, dto.paymentImage.name);
    fd.append('RecipientName', dto.recipientName.trim());
    fd.append('ContactPhone', dto.contactPhone.trim());
    fd.append('AddressLine1', dto.addressLine1.trim());
    if (dto.addressLine2)    fd.append('AddressLine2', dto.addressLine2.trim());
    if (dto.additionalNotes) fd.append('AdditionalNotes', dto.additionalNotes.trim());
    return this.http.post<CreateOrderResponse>(this.urlBase, fd);
  }
}
