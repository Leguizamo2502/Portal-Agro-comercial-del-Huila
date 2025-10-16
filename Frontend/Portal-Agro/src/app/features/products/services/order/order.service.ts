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
  UploadPaymentRequest,
} from '../../models/order/order.model';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private http = inject(HttpClient);

  // Bases según tus nuevos controladores/rutas
  private urlUser = environment.apiUrl + 'OrderUser';
  private urlProducer = environment.apiUrl + 'OrderProducer';

  // ========== CREAR ORDEN (Cliente, sin comprobante) ==========
  create(dto: OrderCreateModel): Observable<CreateOrderResponse> {
    return this.http.post<CreateOrderResponse>(this.urlUser, dto);
  }

  // ========== SUBIR COMPROBANTE (Cliente) ==========
  uploadPayment(code: string, req: UploadPaymentRequest): Observable<any> {
    const fd = new FormData();
    fd.append('RowVersion', req.rowVersion);
    fd.append('PaymentImage', req.paymentImage, req.paymentImage.name);
    return this.http.post<any>(`${this.urlUser}/${code}/payment`, fd);
  }

  // ========== LISTADOS / DETALLES ==========
  // Productor
  getProducerOrders(): Observable<OrderListItemModel[]> {
    return this.http.get<OrderListItemModel[]>(this.urlProducer);
  }

  getProducerPendingOrders(): Observable<OrderListItemModel[]> {
    return this.http.get<OrderListItemModel[]>(`${this.urlProducer}/pending`);
  }

  getDetailForProducer(code: string): Observable<OrderDetailModel> {
    return this.http.get<OrderDetailModel>(`${this.urlProducer}/${code}/detail`);
  }

  // Cliente
  getMine(): Observable<OrderListItemModel[]> {
    return this.http.get<OrderListItemModel[]>(`${this.urlUser}/mine`);
  }

  getDetailForUser(code: string): Observable<OrderDetailModel> {
    return this.http.get<OrderDetailModel>(`${this.urlUser}/${code}/detail`);
  }

  // ========== ACCIONES DEL CLIENTE ==========
  confirmReceived(code: string, body: OrderConfirmRequest): Observable<any> {
    return this.http.post<any>(`${this.urlUser}/${code}/confirm-received`, body);
  }

  cancelByUser(code: string, rowVersion: string): Observable<any> {
    return this.http.post<any>(
      `${this.urlUser}/${code}/cancel`,
      JSON.stringify(rowVersion),
      { headers: { 'Content-Type': 'application/json' } }
    );
  }

  // ========== ACCIONES DEL PRODUCTOR ==========
  acceptOrder(code: string, dto: OrderAcceptRequest): Observable<any> {
    return this.http.post<any>(`${this.urlProducer}/${code}/accept`, dto);
  }

  rejectOrder(code: string, dto: OrderRejectRequest): Observable<any> {
    return this.http.post<any>(`${this.urlProducer}/${code}/reject`, dto);
  }

  markPreparing(code: string, rowVersion: string): Observable<any> {
    return this.http.post<any>(
      `${this.urlProducer}/${code}/preparing`,
      JSON.stringify(rowVersion),
      { headers: { 'Content-Type': 'application/json' } }
    );
  }

  markDispatched(code: string, rowVersion: string): Observable<any> {
    return this.http.post<any>(
      `${this.urlProducer}/${code}/dispatched`,
      JSON.stringify(rowVersion),
      { headers: { 'Content-Type': 'application/json' } }
    );
  }

  markDelivered(code: string, rowVersion: string): Observable<any> {
    return this.http.post<any>(
      `${this.urlProducer}/${code}/delivered`,
      JSON.stringify(rowVersion),
      { headers: { 'Content-Type': 'application/json' } }
    );
  }
}
