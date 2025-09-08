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
