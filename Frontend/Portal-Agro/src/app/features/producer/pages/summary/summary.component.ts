import { Component, OnInit } from '@angular/core';
import { StatCardComponent } from "../../../../shared/components/stat-card/stat-card.component";
import { ChartConfiguration, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { CommonModule } from '@angular/common';
import { OrderService } from '../../../products/services/order/order.service';

@Component({
  selector: 'app-summary',
  standalone: true,
  imports: [CommonModule, StatCardComponent, BaseChartDirective],
  templateUrl: './summary.component.html',
  styleUrl: './summary.component.css'
})
export class SummaryComponent implements OnInit {
  // Propiedades para stat-cards
  totalOrders = 0;
  pendingOrders = 0;
  confirmedOrders = 0;
  loading = true;

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {
    this.loadSummary();
  }

  private loadSummary() {
    this.loading = true;

    // Primero trae todos los pedidos
    this.orderService.getProducerOrders().subscribe({
      next: (orders) => {
        this.totalOrders = orders.length;

        // Luego trae pendientes
        this.orderService.getProducerPendingOrders().subscribe({
          next: (pending) => {
            this.pendingOrders = pending.length;
            this.confirmedOrders = this.totalOrders - this.pendingOrders;
            this.loading = false;
          },
          error: (err) => {
            console.error('Error cargando pedidos pendientes', err);
            this.loading = false;
          }
        });
      },
      error: (err) => {
        console.error('Error cargando pedidos', err);
        this.loading = false;
      }
    });
  }

  // Datos del gráfico (por ahora quemados)
  barChartData: ChartConfiguration<'bar'>['data'] = {
    labels: ['Café', 'Maíz', 'Cacao', 'Frijol', 'Arroz'],
    datasets: [
      {
        label: 'Ventas',
        data: [120, 90, 150, 70, 110],
        backgroundColor: ['#42A5F5', '#66BB6A', '#FFA726', '#AB47BC'],
      }
    ]
  };

  barChartOptions: ChartOptions<'bar'> = {
    responsive: true,
    plugins: {
      legend: {
        display: true,
        position: 'top',
        align: 'start',
        labels: {
          color: '#333',
          font: { size: 14, weight: 'bold' },
        },
      },
    },
  };
}
