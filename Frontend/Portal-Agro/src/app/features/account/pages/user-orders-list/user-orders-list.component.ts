import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { firstValueFrom, take } from 'rxjs';
import Swal from 'sweetalert2';
import {
  OrderListItemModel,
  OrderDetailModel,
  OrderConfirmRequest,
  UploadPaymentRequest,
} from '../../../products/models/order/order.model';
import { OrderService } from '../../../products/services/order/order.service';
import { CommonModule } from '@angular/common';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { StatusTranslatePipe } from '../../../../shared/pipes/StatusTranslatePipe/status-translate-pipe.pipe';

@Component({
  selector: 'app-user-orders-list',
  imports: [CommonModule, ButtonComponent, StatusTranslatePipe],
  templateUrl: './user-orders-list.component.html',
  styleUrl: './user-orders-list.component.css',
})
export class UserOrdersListComponent implements OnInit {
  private ordersSrv = inject(OrderService);
  private router = inject(Router);

  private numberOrders : number = 0;

  loading = true;
  items: OrderListItemModel[] = [];

  // Límite de 6MB para comprobantes
  readonly MAX_FILE_MB = 6;
  readonly MAX_FILE_BYTES = this.MAX_FILE_MB * 1024 * 1024;

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.ordersSrv
      .getMine()
      .pipe(take(1))
      .subscribe({
        next: (list) => {
          this.items = list ?? [];

          // prueba cantidad de pedidos
          this.numberOrders = this.items.length;

          this.loading = false;
        },
        error: (err) => {
          this.loading = false;
          Swal.fire(
            'Error',
            err?.error?.message ?? 'No se pudo cargar tus pedidos.',
            'error'
          );
        },
      });
  }

  view(code : string): void {
    this.router.navigate(['/account/orders', code]);
  }

  /* ======= Guards por estado ======= */
  canUploadPayment(status: string): boolean {
    return status === 'AcceptedAwaitingPayment';
  }

  canCancel(status: string): boolean {
    return status === 'PendingReview';
  }

  canConfirm(status: string): boolean {
    return status === 'DeliveredPendingBuyerConfirm';
  }

  /* Chip CSS según estado */
  chipClass(status: string): 'pending' | 'accepted' | 'completed' | 'rejected' | 'disputed' {
    const s = (status || '').toLowerCase();

    // Pendiente del productor o pendiente de confirmación del comprador
    if (s === 'pendingreview' || s === 'deliveredpendingbuyerconfirm') return 'pending';

    // Flujo intermedio (aceptado/esperando pago o en proceso logístico)
    if (
      s === 'acceptedawaitingpayment' ||
      s === 'paymentsubmitted' ||
      s === 'preparing' ||
      s === 'dispatched'
    ) return 'accepted';

    if (s === 'completed') return 'completed';
    if (s === 'rejected' || s === 'expired' || s === 'cancelledbyuser') return 'rejected';
    if (s === 'disputed') return 'disputed';

    return 'accepted';
  }

  /* ======= Confirmar recepción (Sí/No) ======= */
  async confirm(code : string): Promise<void> {
    let detail: OrderDetailModel;
    try {
      detail = await firstValueFrom(this.ordersSrv.getDetailForUser(code));
    } catch (err: any) {
      Swal.fire(
        'Error',
        err?.error?.message ?? 'No se pudo cargar el pedido.',
        'error'
      );
      return;
    }

    const res = await Swal.fire({
      title: '¿Recibiste el pedido?',
      text: 'Confirma si ya lo recibiste correctamente.',
      icon: 'question',
      showDenyButton: true,
      confirmButtonText: 'Sí, recibido',
      denyButtonText: 'No, hubo problema',
      showCancelButton: true,
    });
    if (res.isDismissed) return;

    const body: OrderConfirmRequest = {
      answer: res.isConfirmed ? 'yes' : 'no',
      rowVersion: detail.rowVersion,
    };

    this.ordersSrv.confirmReceived(code, body).subscribe({
      next: async () => {
        await Swal.fire(
          'Hecho',
          res.isConfirmed ? '¡Gracias por confirmar!' : 'Hemos registrado tu reporte.',
          'success'
        );
        this.load();
      },
      error: (err) =>
        Swal.fire(
          'Error',
          err?.error?.message ?? 'No se pudo registrar la confirmación.',
          'error'
        ),
    });
  }

  /* ======= Cancelar pedido (PendingReview) ======= */
  async cancel(code : string): Promise<void> {
    let detail: OrderDetailModel;
    try {
      detail = await firstValueFrom(this.ordersSrv.getDetailForUser(code));
    } catch (err: any) {
      Swal.fire(
        'Error',
        err?.error?.message ?? 'No se pudo cargar el pedido.',
        'error'
      );
      return;
    }

    const ask = await Swal.fire({
      title: 'Cancelar pedido',
      text: '¿Seguro que deseas cancelar este pedido?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, cancelar',
      cancelButtonText: 'No',
    });
    if (!ask.isConfirmed) return;

    this.ordersSrv.cancelByUser(code, detail.rowVersion).subscribe({
      next: async () => {
        await Swal.fire('Hecho', 'Pedido cancelado.', 'success');
        this.load();
      },
      error: (err) =>
        Swal.fire(
          'Error',
          err?.error?.message ?? 'No se pudo cancelar el pedido.',
          'error'
        ),
    });
  }

  /* ======= Subir comprobante (AcceptedAwaitingPayment) ======= */
  triggerFile(el: HTMLInputElement) {
    el.click();
  }

  async onPickPaymentFile(code : string, ev: Event) {
    const input = ev.target as HTMLInputElement;
    const file = input.files?.[0];
    input.value = ''; 
    if (!file) return;

    if (!file.type.startsWith('image/')) {
      Swal.fire('Atención', 'El comprobante debe ser una imagen (JPG/PNG/WEBP).', 'warning');
      return;
    }
    if (file.size > this.MAX_FILE_BYTES) {
      Swal.fire('Atención', `La imagen excede ${this.MAX_FILE_MB} MB.`, 'warning');
      return;
    }

    let detail: OrderDetailModel;
    try {
      detail = await firstValueFrom(this.ordersSrv.getDetailForUser(code));
    } catch (err: any) {
      Swal.fire('Error', err?.error?.message ?? 'No se pudo cargar el pedido.', 'error');
      return;
    }

    const req: UploadPaymentRequest = { rowVersion: detail.rowVersion, paymentImage: file };

    Swal.fire({
      title: 'Subiendo comprobante…',
      allowOutsideClick: false,
      didOpen: () => Swal.showLoading(),
    });

    this.ordersSrv.uploadPayment(code, req).subscribe({
      next: async () => {
        Swal.close();
        await Swal.fire('Hecho', 'Comprobante subido.', 'success');
        this.load();
      },
      error: async (err) => {
        Swal.close();
        await Swal.fire(
          'Error',
          err?.error?.message ?? 'No se pudo subir el comprobante.',
          'error'
        );
      },
    });
  }

  trackById = (_: number, it: OrderListItemModel) => it.id;
}
