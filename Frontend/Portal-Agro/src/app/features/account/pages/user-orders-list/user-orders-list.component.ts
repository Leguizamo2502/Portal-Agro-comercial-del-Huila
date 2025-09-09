import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { firstValueFrom, take } from 'rxjs';
import Swal from 'sweetalert2';
import {
  OrderListItemModel,
  OrderDetailModel,
  OrderConfirmRequest,
} from '../../../products/models/order/order.model';
import { OrderService } from '../../../products/services/order/order.service';
import { CommonModule } from '@angular/common';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { StatusTranslatePipe } from "../../../../shared/pipes/statusTranslate/status-translate.pipe";

@Component({
  selector: 'app-user-orders-list',
  imports: [CommonModule, ButtonComponent, StatusTranslatePipe],
  templateUrl: './user-orders-list.component.html',
  styleUrl: './user-orders-list.component.css',
})
export class UserOrdersListComponent implements OnInit {
  private ordersSrv = inject(OrderService);
  private router = inject(Router);

  loading = true;
  items: OrderListItemModel[] = [];

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

  view(id: number): void {
    this.router.navigate(['/account/orders', id]);
  }

  canConfirm(status: string): boolean {
    return status === 'AcceptedAwaitingUser';
  }

  async confirm(id: number): Promise<void> {
    let detail: OrderDetailModel;
    try {
      // Obtiene exactamente un valor o lanza error
      detail = await firstValueFrom(this.ordersSrv.getDetailForUser(id));
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

    this.ordersSrv.confirmReceived(id, body).subscribe({
      next: async () => {
        await Swal.fire(
          'Hecho',
          res.isConfirmed
            ? '¡Gracias por confirmar!'
            : 'Hemos registrado tu reporte.',
          'success'
        );
        this.load(); // recarga la lista
      },
      error: (err) =>
        Swal.fire(
          'Error',
          err?.error?.message ?? 'No se pudo registrar la confirmación.',
          'error'
        ),
    });
  }

  trackById = (_: number, it: OrderListItemModel) => it.id;
}
