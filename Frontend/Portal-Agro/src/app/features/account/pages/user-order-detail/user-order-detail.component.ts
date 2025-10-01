import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import Swal from 'sweetalert2';
import {
  OrderDetailModel,
  OrderStatus,
  OrderConfirmRequest,
  UploadPaymentRequest,
} from '../../../products/models/order/order.model';
import { OrderService } from '../../../products/services/order/order.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';

@Component({
  selector: 'app-user-order-detail',
  imports: [CommonModule, ButtonComponent],
  templateUrl: './user-order-detail.component.html',
  styleUrl: './user-order-detail.component.css',
})
export class UserOrderDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private ordersSrv = inject(OrderService);

  code!: string;
  loading = true;
  confirming = false;
  detail?: OrderDetailModel;

  // zoom imagen
  showImage = false;

  // límite de archivo para comprobante
  readonly MAX_FILE_MB = 6;
  readonly MAX_FILE_BYTES = this.MAX_FILE_MB * 1024 * 1024;

  ngOnInit(): void {
    this.code = String(this.route.snapshot.paramMap.get('code'));
    if (!this.code) {
      this.router.navigateByUrl('/account/orders');
      return;
    }
    this.load();
  }

  async load(): Promise<void> {
    this.loading = true;
    try {
      this.detail = await firstValueFrom(this.ordersSrv.getDetailForUser(this.code));
    } catch (err: any) {
      Swal.fire('Error', err?.error?.message ?? 'No se pudo cargar el pedido.', 'error');
      this.detail = undefined;
    } finally {
      this.loading = false;
    }
  }

  /* ======= Guards por estado ======= */
  get canCancel(): boolean {
    return this.detail?.status === 'PendingReview';
  }

  get canUploadPayment(): boolean {
    return this.detail?.status === 'AcceptedAwaitingPayment';
  }

  get canConfirm(): boolean {
    return this.detail?.status === 'DeliveredPendingBuyerConfirm';
  }

  /* ======= Chip de estado (texto + clase) ======= */
  get statusChip(): { text: string; cls: string } {
    const s = (this.detail?.status || '') as OrderStatus;
    switch (s) {
      case 'PendingReview':
        return { text: 'Pendiente de revisión', cls: 'chip info' };

      case 'AcceptedAwaitingPayment':
        return { text: 'Aceptado (esperando pago)', cls: 'chip warning' };

      case 'PaymentSubmitted':
        return { text: 'Pago enviado (en revisión)', cls: 'chip info' };

      case 'Preparing':
        return { text: 'Preparando', cls: 'chip info' };

      case 'Dispatched':
        return { text: 'Despachado', cls: 'chip info' };

      case 'DeliveredPendingBuyerConfirm':
        return { text: 'Entregado (pendiente de confirmación)', cls: 'chip warning' };

      case 'Completed':
        return { text: 'Completado', cls: 'chip success' };

      case 'Rejected':
        return { text: 'Rechazado', cls: 'chip danger' };

      case 'Disputed':
        return { text: 'En disputa', cls: 'chip danger' };

      case 'Expired':
        return { text: 'Expirado', cls: 'chip danger' };

      case 'CancelledByUser':
        return { text: 'Cancelado por el usuario', cls: 'chip danger' };

      default:
        return { text: s, cls: 'chip neutral' };
    }
  }

  /* ======= Acciones ======= */
  async confirm(answer: 'yes' | 'no'): Promise<void> {
    if (!this.detail) return;

    const dialog = await Swal.fire({
      title: answer === 'yes' ? '¿Confirmar recepción?' : '¿Reportar problema?',
      text:
        answer === 'yes'
          ? 'Se dará por completado el pedido.'
          : 'Se marcará como “En disputa”.',
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: answer === 'yes' ? 'Sí, recibido' : 'Reportar',
      cancelButtonText: 'Cancelar',
      reverseButtons: true,
    });

    if (!dialog.isConfirmed) return;

    this.confirming = true;
    const body: OrderConfirmRequest = { answer, rowVersion: this.detail.rowVersion };

    this.ordersSrv.confirmReceived(this.code, body).subscribe({
      next: async () => {
        await Swal.fire(
          'Hecho',
          answer === 'yes' ? '¡Gracias por confirmar!' : 'Se registró tu reporte.',
          'success'
        );
        this.load();
      },
      error: (err) => {
        Swal.fire(
          'Error',
          err?.error?.message ?? 'No se pudo registrar la confirmación.',
          'error'
        );
      },
      complete: () => (this.confirming = false),
    });
  }

  async cancel(): Promise<void> {
    if (!this.detail) return;

    const ask = await Swal.fire({
      title: 'Cancelar pedido',
      text: '¿Seguro que deseas cancelar este pedido?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, cancelar',
      cancelButtonText: 'No',
      reverseButtons: true,
    });
    if (!ask.isConfirmed) return;

    this.ordersSrv.cancelByUser(this.code, this.detail.rowVersion).subscribe({
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

  triggerFile(el: HTMLInputElement) {
    el.click();
  }

  async onPickPaymentFile(ev: Event) {
    const input = ev.target as HTMLInputElement;
    const file = input.files?.[0];
    input.value = ''; // limpia para permitir re-seleccionar luego
    if (!file || !this.detail) return;

    if (!file.type.startsWith('image/')) {
      Swal.fire('Atención', 'El comprobante debe ser una imagen (JPG/PNG/WEBP).', 'warning');
      return;
    }
    if (file.size > this.MAX_FILE_BYTES) {
      Swal.fire('Atención', `La imagen excede ${this.MAX_FILE_MB} MB.`, 'warning');
      return;
    }

    const req: UploadPaymentRequest = {
      rowVersion: this.detail.rowVersion,
      paymentImage: file,
    };

    Swal.fire({
      title: 'Subiendo comprobante…',
      allowOutsideClick: false,
      didOpen: () => Swal.showLoading(),
    });

    this.ordersSrv.uploadPayment(this.code, req).subscribe({
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
}
