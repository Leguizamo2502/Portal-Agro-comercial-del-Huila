import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartConfiguration, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';

import { StatCardComponent } from '../../../../shared/components/stat-card/stat-card.component';
import { OrderService } from '../../../products/services/order/order.service';
import { AnalyticService } from '../../../../shared/services/analytics/analytic.service';
import { forkJoin, catchError, of, finalize } from 'rxjs';
import { RouterLink } from '@angular/router';
import { OrderListItemModel } from '../../../products/models/order/order.model';
import { ProducerService } from '../../../../shared/services/producer/producer.service';
import { ButtonComponent } from "../../../../shared/components/button/button.component";
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { ProducerUpdateModel } from '../../../../shared/models/producer/producer.model';
import { ProfileDialogComponent } from '../../components/profile-dialog/profile-dialog.component';

@Component({
  selector: 'app-summary',
  standalone: true,
  imports: [CommonModule, StatCardComponent, BaseChartDirective, RouterLink, ButtonComponent],
  templateUrl: './summary.component.html',
  styleUrl: './summary.component.css',
})
export class SummaryComponent implements OnInit {
  @ViewChild(BaseChartDirective) chart?: BaseChartDirective;
  private orderService = inject(OrderService);
  private analyticService = inject(AnalyticService);
  private producerService = inject(ProducerService);
  private dialog = inject(MatDialog);

  orderListAll : OrderListItemModel[] = [];

  // Stat-cards
  totalOrders = 0;
  pendingOrders = 0;
  // confirmedOrders = 0;
  loading = true;

  codeProducer?: string

  // Carga de la gráfica
  chartLoading = true;

  constructor() {}

  ngOnInit(): void {
    this.loadSummary();
    this.loadTopProductsChart();
    this.loadProducer();
  }



  private loadProducer(){
    this.producerService.getCodeProducer().subscribe((data)=>{
      this.codeProducer = data.code;
      console.log(this.codeProducer);
      
    })
  }

  private loadSummary() {
    this.loading = true;

    forkJoin({
      all: this.orderService.getProducerOrders().pipe(catchError(() => of([]))),
      pending: this.orderService
        .getProducerPendingOrders()
        .pipe(catchError(() => of([]))),
    })
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(({ all, pending }) => {
        this.orderListAll = all;
        this.totalOrders = all.length;
        this.pendingOrders = pending.length;

      });
  }

  updateProfile() {
  if (!this.codeProducer) {
    Swal.fire('Atención', 'Aún no se cargó tu código de productor.', 'warning');
    return;
  }

  // Traer datos actuales para preload del modal
  this.producerService.getByCodeProducer(this.codeProducer).subscribe({
    next: (producer) => {
      const ref = this.dialog.open(ProfileDialogComponent, {
        width: '720px',
        data: { producer }
      });

      ref.afterClosed().subscribe((payload?: ProducerUpdateModel) => {
        if (!payload) return;

        this.producerService.updateProfile(payload).subscribe({
          next: () => {
            Swal.fire({ toast: true, position: 'top-end', icon: 'success', title: 'Perfil actualizado', showConfirmButton: false, timer: 1500 });
          },
          error: (err) => {
            const msg = err?.error?.message || 'No se pudo actualizar el perfil';
            Swal.fire('Error', msg, 'error');
          }
        });
      });
    },
    error: () => {
      Swal.fire('Error', 'No se pudo cargar tu perfil actual.', 'error');
    }
  });
}

  // ====== Gráfica dinámica (Top productos por pedidos completados) ======

  // Inicia vacío; se llenará con la API de analytics
  barChartData: ChartConfiguration<'bar'>['data'] = {
    labels: [],
    datasets: [
      {
        label: 'Pedidos completados',
        data: [],
        // Si quieres mantener colores personalizados, déjalos.
        // Si prefieres automático de Chart.js, elimina backgroundColor.
        backgroundColor: [
          '#42A5F5',
          '#66BB6A',
          '#FFA726',
          '#AB47BC',
          '#29B6F6',
          '#EC407A',
          '#7E57C2',
        ],
      },
    ],
  };

  barChartOptions: ChartOptions<'bar'> = {
    responsive: true,
    plugins: {
      legend: { display: true, position: 'top', align: 'start' },
      tooltip: { enabled: true },
    },
    scales: {
      x: {
        ticks: {
          callback: function (value, index, ticks) {
            const lbl = (this.getLabelForValue as any)?.(value) ?? '';
            return String(lbl).length > 18
              ? String(lbl).slice(0, 17) + '…'
              : lbl;
          },
        },
      },
      y: { beginAtZero: true, ticks: { stepSize: 1, precision: 0 } },
    },
  };

  private loadTopProductsChart(limit = 5): void {
    this.chartLoading = true;

    this.analyticService.getTopProducts(limit).subscribe({
      next: ({ items }) => {
        if (!items || items.length === 0) {
          // Estado vacío legible en la UI
          this.barChartData.labels = ['Sin datos'];
          this.barChartData.datasets[0].data = [0];
        } else {
          this.barChartData.labels = items.map((i) => i.productName);
          // Por definición pediste “más pedidos completados”
          this.barChartData.datasets[0].data = items.map(
            (i) => i.completedOrders
          );

        }

        // Forzar redibujo cuando se cambian referencias de data/labels
        this.chart?.update();
        this.chartLoading = false;
      },
      error: (err) => {
        console.error('Error cargando top de productos', err);
        // Falla segura para no romper la vista
        this.barChartData.labels = ['Error'];
        this.barChartData.datasets[0].data = [0];
        this.chart?.update();
        this.chartLoading = false;
      },
    });
  }
}
