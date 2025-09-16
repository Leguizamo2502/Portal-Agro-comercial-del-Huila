import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment.development';

import {
  CreateOrderResponse,
  OrderAcceptRequest,
  OrderConfirmRequest,
  OrderDetailModel,
  OrderListItemModel,
  OrderRejectRequest,
  OrderCreateModel,
  RowVersionOnly,
  UploadPaymentRequest,
} from '../../models/order/order.model';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + 'Order';

  // ========== CREAR ORDEN (sin comprobante) ==========
  create(dto: OrderCreateModel): Observable<CreateOrderResponse> {
    return this.http.post<CreateOrderResponse>(this.urlBase, dto);
  }

  // ========== SUBIR COMPROBANTE ==========
  uploadPayment(id: number, req: UploadPaymentRequest): Observable<any> {
    const fd = new FormData();
    fd.append('RowVersion', req.rowVersion);
    fd.append('PaymentImage', req.paymentImage, req.paymentImage.name);
    return this.http.post<any>(`${this.urlBase}/${id}/payment`, fd);
  }

  // ========== LISTADOS / DETALLES ==========
  // Productor
  getProducerOrders(): Observable<OrderListItemModel[]> {
    return this.http.get<OrderListItemModel[]>(this.urlBase);
  }

  getProducerPendingOrders(): Observable<OrderListItemModel[]> {
    return this.http.get<OrderListItemModel[]>(`${this.urlBase}/pending`);
  }

  getDetailForProducer(id: number): Observable<OrderDetailModel> {
    return this.http.get<OrderDetailModel>(
      `${this.urlBase}/${id}/for-producer`
    );
  }

  // Cliente
  getMine(): Observable<OrderListItemModel[]> {
    return this.http.get<OrderListItemModel[]>(`${this.urlBase}/mine`);
  }

  getDetailForUser(id: number): Observable<OrderDetailModel> {
    return this.http.get<OrderDetailModel>(`${this.urlBase}/${id}/for-user`);
  }

  // ========== ACCIONES DEL CLIENTE ==========
  confirmReceived(id: number, body: OrderConfirmRequest) {
    return this.http.post<any>(`${this.urlBase}/${id}/confirm-received`, body);
  }

  cancelByUser(id: number, rowVersion: string) {
    return this.http.post<any>(
      `${this.urlBase}/${id}/cancel`,
      JSON.stringify(rowVersion),
      { headers: { 'Content-Type': 'application/json' } }
    );
  }

  // ========== ACCIONES DEL PRODUCTOR ==========
  acceptOrder(id: number, dto: OrderAcceptRequest): Observable<any> {
    return this.http.post<any>(`${this.urlBase}/${id}/accept`, dto);
  }

  rejectOrder(id: number, dto: OrderRejectRequest): Observable<any> {
    return this.http.post<any>(`${this.urlBase}/${id}/reject`, dto);
  }

  markPreparing(id: number, rowVersion: string): Observable<any> {
    return this.http.post<any>(
      `${this.urlBase}/${id}/preparing`,
      JSON.stringify(rowVersion),
      { headers: { 'Content-Type': 'application/json' } }
    );
  }

  markDispatched(id: number, rowVersion: string): Observable<any> {
    return this.http.post<any>(
      `${this.urlBase}/${id}/dispatched`,
      JSON.stringify(rowVersion),
      { headers: { 'Content-Type': 'application/json' } }
    );
  }

  markDelivered(id: number, rowVersion: string): Observable<any> {
    return this.http.post<any>(
      `${this.urlBase}/${id}/delivered`,
      JSON.stringify(rowVersion),
      { headers: { 'Content-Type': 'application/json' } }
    );
  }
}
