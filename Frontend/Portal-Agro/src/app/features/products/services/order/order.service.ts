import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment.development';
import { Observable } from 'rxjs';
import { OrderCreateModel, CreateOrderResponse, OrderAcceptRequest, OrderConfirmRequest, OrderDetailModel, OrderListItemModel, OrderRejectRequest } from '../../models/order/order.model';

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

  // Producer
  getProducerOrders(): Observable<OrderListItemModel[]> {
    return this.http.get<OrderListItemModel[]>(this.urlBase);
  }
  getProducerPendingOrders(): Observable<OrderListItemModel[]> {
    return this.http.get<OrderListItemModel[]>(`${this.urlBase}/pending`);
  }
  getDetailForProducer(id: number): Observable<OrderDetailModel> {
    return this.http.get<OrderDetailModel>(`${this.urlBase}/${id}/for-producer`);
  }

  // User
  getMine(): Observable<OrderListItemModel[]> {
    return this.http.get<OrderListItemModel[]>(`${this.urlBase}/mine`);
  }
  getDetailForUser(id: number): Observable<OrderDetailModel> {
    return this.http.get<OrderDetailModel>(`${this.urlBase}/${id}/for-user`);
  }
  confirmReceived(id: number, body: OrderConfirmRequest): Observable<any> {
    return this.http.post<any>(`${this.urlBase}/${id}/confirm-received`, body);
  }

  // Producer actions
  acceptOrder(id: number, dto: OrderAcceptRequest): Observable<void> {
    return this.http.post<void>(`${this.urlBase}/${id}/accept`, dto);
  }
  rejectOrder(id: number, dto: OrderRejectRequest): Observable<void> {
    return this.http.post<void>(`${this.urlBase}/${id}/reject`, dto);
  }
}
