import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart, ChartConfiguration, ChartDataset } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { MatCardModule } from "@angular/material/card";
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, BaseChartDirective],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {

  summary = {
    totalProducers: 120,
    activeProducers: 95,
    totalOrders: 350,
    pendingOrders: 42,
    totalProducts: 180
  };

  kpis = [
    { title: 'Productores Totales', value: 120, icon: 'groups' },
    { title: 'Activos', value: 95, icon: 'eco' },
    { title: 'Pedidos Totales', value: 350, icon: 'local_shipping' },
    { title: 'Pendientes', value: 42, icon: 'hourglass_empty' },
    { title: 'Productos', value: 180, icon: 'storefront' }
  ];

  // === Pedidos por Mes ===
  barChartOptions: ChartConfiguration<'bar'>['options'] = {
    responsive: true,
    plugins: { legend: { display: false } },
    scales: { y: { beginAtZero: true } }
  };

  barChartData: ChartConfiguration<'bar'>['data'] = {
    labels: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun'],
    datasets: [{
      label: 'Pedidos',
      data: [25, 40, 55, 65, 70, 90],
      backgroundColor: '#81C784',
      borderRadius: 8
    }]
  };

  // === Productos Más Vendidos ===
  topProductsChartOptions: ChartConfiguration<'bar'>['options'] = {
    responsive: true,
    indexAxis: 'y',
    plugins: { legend: { display: false } },
    scales: { x: { beginAtZero: true } }
  };

  //se podría dividir por categoría
  topProductsChartData: ChartConfiguration<'bar'>['data'] = {
    labels: ['Café', 'Panela', 'Miel', 'Plátano', 'Yuca'],
    datasets: [{
      data: [120, 100, 80, 60, 40],
      backgroundColor: [] as string[],
      borderRadius: 10
    }]
  };

  lineChartOptions: ChartConfiguration<'line'>['options'] = {
    responsive: true,
    plugins: { legend: { display: false } },
    elements: { line: { tension: 0.3 } },
    scales: { y: { beginAtZero: true } }
  };

  lineChartData: ChartConfiguration<'line'>['data'] = {
    labels: ['Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov'],
    datasets: [{
      label: 'Productores Activos',
      data: [70, 75, 80, 90, 95, 100],
      borderColor: '#66bb6a',
      backgroundColor: 'rgba(102,187,106,0.2)',
      fill: true
    }]
  };

  ngOnInit(): void {
    console.log( 'Panel del Productor cargado correctamente');

    // Crear degradados suaves para cada barra del gráfico de productos
    const canvas = document.createElement('canvas');
    const ctx = canvas.getContext('2d');

    if (ctx) {
      const gradients = [
        this.createGradient(ctx, '#A5D6A7', '#66BB6A'),
        this.createGradient(ctx, '#FFD54F', '#FFA000'),
        this.createGradient(ctx, '#FFE082', '#FBC02D'),
        this.createGradient(ctx, '#AED581', '#8BC34A'),
        this.createGradient(ctx, '#C8E6C9', '#81C784')
      ];

      (this.topProductsChartData.datasets[0] as ChartDataset<'bar'>).backgroundColor = gradients;
    }
  }

  private createGradient(ctx: CanvasRenderingContext2D, colorStart: string, colorEnd: string): CanvasGradient {
    const gradient = ctx.createLinearGradient(0, 0, 300, 0);
    gradient.addColorStop(0, colorStart);
    gradient.addColorStop(1, colorEnd);
    return gradient;
  }
}
