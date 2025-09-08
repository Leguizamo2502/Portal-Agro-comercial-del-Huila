export interface OrderCreateModel {
  productId: number;
  quantityRequested: number;
  paymentImage: File;

  recipientName: string;
  contactPhone: string;
  addressLine1: string;
  addressLine2?: string | null;
  cityId: number;
  additionalNotes?: string | null;
}

export interface CreateOrderResponse {
  isSuccess: boolean;
  message: string;
  orderId: number;
}

export interface OrderListItemModel {
 id: number;
  productName: string;
  quantityRequested: number;
  subtotal: number;
  total: number;
  status: string;          // viene como string del backend
  createdAt: string;        // ISO
  paymentImageUrl?: string;
}

export type OrderStatus =
  | 'PendingReview'
  | 'AcceptedAwaitingUser'
  | 'Rejected'
  | 'Completed'
  | 'Disputed'
  | string;

export interface OrderDetailModel {
  id: number;
  productName: string;
  unitPrice: number;
  quantityRequested: number;
  subtotal: number;
  total: number;
  status: OrderStatus;

  // evidencia de pago
  paymentImageUrl?: string | null;
  paymentUploadedAt?: string | null;

  // entrega
  recipientName: string;
  contactPhone: string;
  addressLine1: string;
  addressLine2?: string | null;
  cityId: number;
  cityName?: string | null;

  // metadatos
  createdAt: string;
  producerDecisionAt?: string | null;
  producerDecisionReason?: string | null;
  producerNotes?: string | null;

  // concurrencia (Base64)
  rowVersion: string;
}

/* Requests */
export interface OrderAcceptRequest {
  notes?: string;
  rowVersion: string;
}
export interface OrderRejectRequest {
  reason: string;
  rowVersion: string;
}
export interface OrderConfirmRequest {
  answer: 'yes' | 'no';
  rowVersion: string;
}

