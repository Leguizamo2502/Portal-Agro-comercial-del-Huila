import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs';
import Swal from 'sweetalert2';
import { OrderDetailModel } from '../../../products/models/order/order.model';
import { OrderService } from '../../../products/services/order/order.service';
import { CommonModule } from '@angular/common';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { StatusTranslatePipe } from '../../../../shared/pipes/StatusTranslatePipe/status-translate-pipe.pipe';

@Component({
  selector: 'app-producer-order-detail',
  imports: [CommonModule, ButtonComponent, StatusTranslatePipe],
  templateUrl: './producer-order-detail.component.html',
  styleUrl: './producer-order-detail.component.css',
})
export class ProducerOrderDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private ordersSrv = inject(OrderService);

  code!: string;
  detail?: OrderDetailModel;
  loading = true;

  ngOnInit(): void {
    this.code = String(this.route.snapshot.paramMap.get('code'));
    if (!this.code) {
      this.router.navigateByUrl('/account/producer/orders');
      return;
    }
    this.loadDetail();
  }

  loadDetail(): void {
    this.loading = true;
    this.ordersSrv
      .getDetailForProducer(this.code)
      .pipe(take(1))
      .subscribe({
        next: (d) => {
          this.detail = d;
          this.loading = false;
        },
        error: (err) => {
          this.loading = false;
          Swal.fire(
            'Error',
            err?.error?.message ?? 'No se pudo cargar el pedido.',
            'error'
          );
        },
      });
  }

  // ======= Habilitadores de acciones por estado =======
  get canAcceptReject(): boolean {
    return this.detail?.status === 'PendingReview';
  }
  get canMarkPreparing(): boolean {
    return this.detail?.status === 'PaymentSubmitted';
  }
  get canMarkDispatched(): boolean {
    return this.detail?.status === 'Preparing';
  }
  get canMarkDelivered(): boolean {
    return this.detail?.status === 'Dispatched';
  }

  // ======= Chip de estado (clases CSS) =======
  chipClass(
    status: string
  ):
    | 'pending'
    | 'accepted'
    | 'progress'
    | 'completed'
    | 'rejected'
    | 'disputed' {
    const s = (status || '').toLowerCase();

    // Pendiente de decisión o de confirmación del comprador
    if (s === 'pendingreview' || s === 'deliveredpendingbuyerconfirm')
      return 'pending';

    // Aceptado/avance del flujo
    if (
      s === 'acceptedawaitingpayment' || // nuevo
      s === 'paymentsubmitted' ||        // nuevo
      s === 'preparing' ||               // nuevo
      s === 'dispatched'                 // nuevo
    ) return 'progress';

    if (s === 'completed') return 'completed';
    if (s === 'rejected' || s === 'expired' || s === 'cancelledbyuser')
      return 'rejected';
    if (s === 'disputed') return 'disputed';

    // fallback
    return 'accepted';
  }

  openReceipt(): void {
    const url = this.detail?.paymentImageUrl;
    if (url) window.open(url, '_blank');
  }

  // ======= Acciones =======
  async accept(): Promise<void> {
    if (!this.detail) return;

    const { value: notes, isConfirmed } = await Swal.fire({
      title: 'Aceptar pedido',
      input: 'textarea',
      inputLabel: 'Notas al cliente (opcional)',
      inputPlaceholder: 'Escribe notas internas o para el cliente…',
      inputAttributes: { 'aria-label': 'Notas' },
      showCancelButton: true,
      confirmButtonText: 'Aceptar',
      cancelButtonText: 'Cancelar',
      preConfirm: (v) => (v ? String(v).trim() : ''),
    });

    if (!isConfirmed) return;

    this.ordersSrv
      .acceptOrder(this.code, {
        notes: notes || undefined,
        rowVersion: this.detail.rowVersion,
      })
      .pipe(take(1))
      .subscribe({
        next: async () => {
          await Swal.fire('OK', 'Pedido aceptado.', 'success');
          this.loadDetail(); // refresca para traer nuevo estado/rowVersion
        },
        error: (err) => {
          const msg = err?.error?.message || 'No se pudo aceptar.';
          Swal.fire('Error', msg, 'error');
        },
      });
  }

  async reject(): Promise<void> {
    if (!this.detail) return;

    const { value: reason, isConfirmed } = await Swal.fire<string>({
      title: 'Rechazar pedido',
      input: 'textarea',
      inputLabel: 'Motivo (requerido)',
      inputPlaceholder: 'Explica por qué se rechaza…',
      showCancelButton: true,
      confirmButtonText: 'Rechazar',
      cancelButtonText: 'Cancelar',
      inputValidator: (v) => {
        const txt = (v ?? '').toString().trim();
        if (txt.length < 5)
          return 'El motivo debe tener al menos 5 caracteres.';
        return undefined;
      },
    });

    if (!isConfirmed || !reason) return;

    this.ordersSrv
      .rejectOrder(this.code, {
        reason,
        rowVersion: this.detail.rowVersion,
      })
      .pipe(take(1))
      .subscribe({
        next: async () => {
          await Swal.fire('OK', 'Pedido rechazado.', 'success');
          this.loadDetail();
        },
        error: (err) => {
          const msg = err?.error?.message || 'No se pudo rechazar.';
          Swal.fire('Error', msg, 'error');
        },
      });
  }

  markPreparing(): void {
    if (!this.detail) return;
    this.ordersSrv
      .markPreparing(this.code, this.detail.rowVersion)
      .pipe(take(1))
      .subscribe({
        next: async () => {
          await Swal.fire('OK', 'Orden en preparación.', 'success');
          this.loadDetail();
        },
        error: (err) =>
          Swal.fire('Error', err?.error?.message ?? 'No se pudo marcar.', 'error'),
      });
  }

  markDispatched(): void {
    if (!this.detail) return;
    this.ordersSrv
      .markDispatched(this.code, this.detail.rowVersion)
      .pipe(take(1))
      .subscribe({
        next: async () => {
          await Swal.fire('OK', 'Orden despachada.', 'success');
          this.loadDetail();
        },
        error: (err) =>
          Swal.fire('Error', err?.error?.message ?? 'No se pudo marcar.', 'error'),
      });
  }

  markDelivered(): void {
    if (!this.detail) return;
    this.ordersSrv
      .markDelivered(this.code, this.detail.rowVersion)
      .pipe(take(1))
      .subscribe({
        next: async () => {
          await Swal.fire(
            'OK',
            'Orden entregada (pendiente de confirmación del cliente).',
            'success'
          );
          this.loadDetail();
        },
        error: (err) =>
          Swal.fire('Error', err?.error?.message ?? 'No se pudo marcar.', 'error'),
      });
  }
}
