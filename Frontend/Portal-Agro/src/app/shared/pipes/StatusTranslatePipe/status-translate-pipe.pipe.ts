import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'statusTranslate',
  standalone: true // ← si lo usas en `imports` de componentes
})
export class StatusTranslatePipe implements PipeTransform {

  private readonly statusMap: Record<string, string> = {
    PendingReview: 'Pendiente de revisión',
    AcceptedAwaitingPayment: 'Aceptado (espera comprobante)',
    PaymentSubmitted: 'Pago enviado',
    Preparing: 'Preparando',
    Dispatched: 'Despachado',
    DeliveredPendingBuyerConfirm: 'Entregado (pendiente confirmación)',
    Completed: 'Completado',
    Disputed: 'En disputa',
    Rejected: 'Rechazado',
    CancelledByUser: 'Cancelado por el cliente',
    Expired: 'Expirada',

    // Compatibilidad con el flujo antiguo (por si llega desde caché/histórico):
    AcceptedAwaitingUser: 'Aceptado (esperando cliente)'
  };

  transform(value: string): string {
    return this.statusMap[value] ?? value;
  }
}