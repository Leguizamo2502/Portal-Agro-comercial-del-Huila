// MODELOS / TIPOS

export interface OrderCreateModel {
  productId: number;
  quantityRequested: number;

  // Entrega
  recipientName: string;
  contactPhone: string;
  addressLine1: string;
  addressLine2?: string | null;
  cityId: number;

  // Opcional
  additionalNotes?: string | null;
}

export interface CreateOrderResponse {
  isSuccess: boolean;
  message?: string;
  orderId?: number;
}

export type OrderStatus =
  | 'PendingReview'
  | 'AcceptedAwaitingPayment'
  | 'PaymentSubmitted'
  | 'Preparing'
  | 'Dispatched'
  | 'DeliveredPendingBuyerConfirm'
  | 'Completed'
  | 'Disputed'
  | 'Rejected'
  | 'CancelledByUser'
  | 'Expired'
  | string;

export type UserReceivedAnswer = 'None' | 'Yes' | 'No';

export interface OrderListItemModel {
  id: number;
  code: string;
  productName: string;
  quantityRequested: number;
  subtotal: number;
  total: number;
  status: OrderStatus;

  // OJO: el backend envía "CreateAt" => JSON "createAt"
  createAt: string; // ISO
  paymentImageUrl?: string | null;
  
}

export interface OrderDetailModel {
  id: number;
  code: string;
  // Producto / snapshots
  productId: number;
  productName: string;
  unitPrice: number;

  // Cantidades / totales
  quantityRequested: number;
  subtotal: number;
  total: number;

  // Estado
  status: OrderStatus;
  userReceivedAnswer?: UserReceivedAnswer; // "None" | "Yes" | "No"

  // Comprobante
  paymentImageUrl?: string | null;
  paymentUploadedAt?: string | null;

  // Decisión productor
  producerDecisionAt?: string | null;
  producerDecisionReason?: string | null;
  producerNotes?: string | null;

  // Entrega
  recipientName: string;
  contactPhone: string;
  addressLine1: string;
  addressLine2?: string | null;
  cityId: number;
  cityName?: string | null;
  departmentName?: string | null;

  // Cliente
  userReceivedAt?: string | null;

  // Metadatos
  createAt: string;          // ISO (ojo con el nombre)
  rowVersion: string;        // Base64 para concurrencia
}

// REQUESTS

export interface OrderAcceptRequest {
  notes?: string;
  rowVersion: string;
}

export interface OrderRejectRequest {
  reason: string;
  rowVersion: string;
}

export interface OrderConfirmRequest {
  // el backend acepta "Yes"/"No" (case-insensitive)
  answer: 'yes' | 'no';
  rowVersion: string;
}

export interface RowVersionOnly {
  rowVersion: string;
}

export interface UploadPaymentRequest {
  rowVersion: string;
  paymentImage: File;
}
