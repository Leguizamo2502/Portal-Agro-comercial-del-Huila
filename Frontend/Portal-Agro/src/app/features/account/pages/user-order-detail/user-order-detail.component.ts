import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import Swal from 'sweetalert2';
import { OrderDetailModel, OrderStatus, OrderConfirmRequest } from '../../../products/models/order/order.model';
import { OrderService } from '../../../products/services/order/order.service';
import { ButtonComponent } from "../../../../shared/components/button/button.component";


@Component({
  selector: 'app-user-order-detail',
  imports: [CommonModule, ButtonComponent],
  templateUrl: './user-order-detail.component.html',
  styleUrl: './user-order-detail.component.css'
})
export class UserOrderDetailComponent implements OnInit{
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private ordersSrv = inject(OrderService);


  id!: number;
  loading = true;
  confirming = false;
  detail?: OrderDetailModel;

  // zoom imagen
  showImage = false;

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.id) {
      this.router.navigateByUrl('/account/orders');
      return;
    }
    this.load();
  }

  async load(): Promise<void> {
    this.loading = true;
    try {
      this.detail = await firstValueFrom(this.ordersSrv.getDetailForUser(this.id));
    } catch (err: any) {
      Swal.fire('Error', err?.error?.message ?? 'No se pudo cargar el pedido.', 'error');
      this.detail = undefined;
    } finally {
      this.loading = false;
    }
  }

  get canConfirm(): boolean {
    return this.detail?.status === 'AcceptedAwaitingUser';
  }

  get statusChip(): { text: string; cls: string } {
    const s = (this.detail?.status || '') as OrderStatus;
    switch (s) {
      case 'PendingReview':       return { text: 'Pendiente de revisión', cls: 'chip info' };
      case 'AcceptedAwaitingUser':return { text: 'Aceptado, esperando confirmación', cls: 'chip warning' };
      case 'Rejected':            return { text: 'Rechazado', cls: 'chip danger' };
      case 'Completed':           return { text: 'Completado', cls: 'chip success' };
      case 'Disputed':            return { text: 'En disputa', cls: 'chip danger' };
      default:                    return { text: s, cls: 'chip neutral' };
    }
  }

 

  async confirm(answer: 'yes'|'no'): Promise<void> {
    if (!this.detail) return;

    const dialog = await Swal.fire({
      title: answer === 'yes' ? '¿Confirmar recepción?' : '¿Reportar problema?',
      text: answer === 'yes'
        ? 'Se dará por completado el pedido.'
        : 'Se marcará como “En disputa”.',
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: answer === 'yes' ? 'Sí, recibido' : 'Reportar',
      cancelButtonText: 'Cancelar',
      reverseButtons: true
    });

    if (!dialog.isConfirmed) return;

    this.confirming = true;
    const body: OrderConfirmRequest = { answer, rowVersion: this.detail.rowVersion };

    this.ordersSrv.confirmReceived(this.id, body).subscribe({
      next: async () => {
        await Swal.fire(
          'Hecho',
          answer === 'yes' ? '¡Gracias por confirmar!' : 'Se registró tu reporte.',
          'success'
        );
        this.load();
      },
      error: (err) => {
        Swal.fire('Error', err?.error?.message ?? 'No se pudo registrar la confirmación.', 'error');
      },
      complete: () => (this.confirming = false)
    });
  }
}
