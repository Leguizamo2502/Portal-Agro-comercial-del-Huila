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
import { MatColumnDef } from '@angular/material/table';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private http = inject(HttpClient);
  private urlBase = environment.apiUrl + 'Order';

  // ========== CREAR ORDEN (sin comprobante) ==========
  create(dto: OrderCreateModel): Observable<CreateOrderResponse> {
    return this.http.post<CreateOrderResponse>(this.urlBase, dto);
  }

  // ========== SUBIR COMPROBANTE ==========
  uploadPayment(code : string, req: UploadPaymentRequest): Observable<any> {
    const fd = new FormData();
    fd.append('RowVersion', req.rowVersion);
    fd.append('PaymentImage', req.paymentImage, req.paymentImage.name);
    return this.http.post<any>(`${this.urlBase}/${code}/payment`, fd);
  }

  // ========== LISTADOS / DETALLES ==========
  // Productor
  getProducerOrders(): Observable<OrderListItemModel[]> {
    return this.http.get<OrderListItemModel[]>(this.urlBase);
  }

  getProducerPendingOrders(): Observable<OrderListItemModel[]> {
    return this.http.get<OrderListItemModel[]>(`${this.urlBase}/pending`);
  }

  getDetailForProducer(code : string): Observable<OrderDetailModel> {
    return this.http.get<OrderDetailModel>(
      `${this.urlBase}/${code}/for-producer`
    );
  }

  // Cliente
  getMine(): Observable<OrderListItemModel[]> {
    return this.http.get<OrderListItemModel[]>(`${this.urlBase}/mine`);
  }

  getDetailForUser(code: string): Observable<OrderDetailModel> {
    return this.http.get<OrderDetailModel>(`${this.urlBase}/${code}/for-user`);
  }

  // ========== ACCIONES DEL CLIENTE ==========
  confirmReceived(code : string, body: OrderConfirmRequest) {
    return this.http.post<any>(`${this.urlBase}/${code}/confirm-received`, body);
  }

  cancelByUser(code : string, rowVersion: string) {
    return this.http.post<any>(
      `${this.urlBase}/${code}/cancel`,
      JSON.stringify(rowVersion),
      { headers: { 'Content-Type': 'application/json' } }
    );
  }

  // ========== ACCIONES DEL PRODUCTOR ==========
  acceptOrder(code : string, dto: OrderAcceptRequest): Observable<any> {
    return this.http.post<any>(`${this.urlBase}/${code}/accept`, dto);
  }

  rejectOrder(code : string, dto: OrderRejectRequest): Observable<any> {
    return this.http.post<any>(`${this.urlBase}/${code}/reject`, dto);
  }

  markPreparing(code : string, rowVersion: string): Observable<any> {
    return this.http.post<any>(
      `${this.urlBase}/${code}/preparing`,
      JSON.stringify(rowVersion),
      { headers: { 'Content-Type': 'application/json' } }
    );
  }

  markDispatched(code : string, rowVersion: string): Observable<any> {
    return this.http.post<any>(
      `${this.urlBase}/${code}/dispatched`,
      JSON.stringify(rowVersion),
      { headers: { 'Content-Type': 'application/json' } }
    );
  }

  markDelivered(code : string, rowVersion: string): Observable<any> {
    return this.http.post<any>(
      `${this.urlBase}/${code}/delivered`,
      JSON.stringify(rowVersion),
      { headers: { 'Content-Type': 'application/json' } }
    );
  }
}
