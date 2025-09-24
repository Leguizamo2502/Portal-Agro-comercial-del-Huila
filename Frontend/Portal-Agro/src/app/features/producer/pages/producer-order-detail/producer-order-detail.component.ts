import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs';
import Swal from 'sweetalert2';
import { OrderDetailModel } from '../../../products/models/order/order.model';
import { OrderService } from '../../../products/services/order/order.service';
import { CommonModule } from '@angular/common';
import { ButtonComponent } from "../../../../shared/components/button/button.component";
import { StatusTranslatePipe } from "../../../../shared/pipes/statusTranslate/status-translate.pipe";

@Component({
  selector: 'app-producer-order-detail',
  imports: [CommonModule, ButtonComponent, StatusTranslatePipe],
  templateUrl: './producer-order-detail.component.html',
  styleUrl: './producer-order-detail.component.css'
})
export class ProducerOrderDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private ordersSrv = inject(OrderService);

  orderId!: number;
  detail?: OrderDetailModel;
  loading = true;

  ngOnInit(): void {
    this.orderId = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.orderId) {
      this.router.navigateByUrl('/account/producer/orders');
      return;
    }
    this.loadDetail();
  }

  get canAct(): boolean {
    return this.detail?.status === 'PendingReview';
  }

  loadDetail(): void {
    this.loading = true;
    this.ordersSrv.getDetailForProducer(this.orderId)
      .pipe(take(1))
      .subscribe({
        next: (d) => { this.detail = d; this.loading = false; },
        error: (err) => {
          this.loading = false;
          Swal.fire('Error', err?.error?.message ?? 'No se pudo cargar el pedido.', 'error');
        }
      });
  }

  openReceipt(): void {
    const url = this.detail?.paymentImageUrl;
    if (url) window.open(url, '_blank');
  }

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

    this.ordersSrv.acceptOrder(this.orderId, {
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
      }
    });
  }

  async reject(): Promise<void> {
    if (!this.detail) return;

    const { value: reason, isConfirmed } = await Swal.fire<string>({
      title: 'Rechazar pedido',
      input: 'textarea',
      inputLabel: 'Motivo (requerido)',
      inputPlaceholder: 'Explica por qué se rechaza…',
      inputAttributes: { 'aria-label': 'Motivo' },
      showCancelButton: true,
      confirmButtonText: 'Rechazar',
      cancelButtonText: 'Cancelar',
      preConfirm: (v) => {
        const txt = (v ?? '').toString().trim();
        if (txt.length < 5) {
          Swal.showValidationMessage('El motivo debe tener al menos 5 caracteres.');
          return false as any;
        }
        return txt;
      }
    });

    if (!isConfirmed || !reason) return;

    this.ordersSrv.rejectOrder(this.orderId, {
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
      }
    });
  }
}
