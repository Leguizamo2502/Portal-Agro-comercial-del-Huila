import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router, Params, RouterLink } from '@angular/router';
import { Subscription, switchMap } from 'rxjs';
import Swal from 'sweetalert2';
import { OrderListItemModel } from '../../../products/models/order/order.model';
import { OrderService } from '../../../products/services/order/order.service';
import { MatIconModule } from "@angular/material/icon";
import { CommonModule } from '@angular/common';
import { StatusTranslatePipe } from '../../../../shared/pipes/StatusTranslatePipe/status-translate-pipe.pipe';

@Component({
  selector: 'app-producer-orders-list',
  imports: [MatIconModule, RouterLink, CommonModule, StatusTranslatePipe],
  templateUrl: './producer-orders-list.component.html',
  styleUrl: './producer-orders-list.component.css'
})
export class ProducerOrdersListComponent implements OnInit, OnDestroy{
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private orderSrv = inject(OrderService);

  isLoading = false;
  items: OrderListItemModel[] = [];
  statusFilter: 'pending' | 'all' = 'pending';

  private sub?: Subscription;

  ngOnInit(): void {
    // lee ?status=pending|all y carga
    this.sub = this.route.queryParams
      .pipe(
        switchMap((qp: Params) => {
          this.statusFilter = qp['status'] === 'all' ? 'all' : 'pending';
          this.isLoading = true;
          return this.statusFilter === 'pending'
            ? this.orderSrv.getProducerPendingOrders()
            : this.orderSrv.getProducerOrders();
        })
      )
      .subscribe({
        next: (data) => {
          this.items = data ?? [];
          console.log(data);
          this.isLoading = false;
        },
        error: (err) => {
          this.isLoading = false;
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: err?.error?.message ?? 'No se pudo cargar la lista de pedidos.',
          });
        },
      });
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  // Navega preservando la UX del filtro
  goFilter(status: 'pending' | 'all'): void {
    if (this.statusFilter === status) return;
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { status },
      queryParamsHandling: 'merge',
    });
  }

  // Helper UI
  asCurrency(v: number) {
    return new Intl.NumberFormat('es-CO', { style: 'currency', currency: 'COP', maximumFractionDigits: 0 }).format(v);
  }

  statusChipColor(status: string): 'warning' | 'info' | 'success' | 'danger' {
    const s = (status || '').toLowerCase();
    if (s.includes('pending')) return 'warning';
    if (s.includes('accepted')) return 'info';
    if (s.includes('completed')) return 'success';
    if (s.includes('rejected') || s.includes('disputed')) return 'danger';
    return 'info';
  }
}
