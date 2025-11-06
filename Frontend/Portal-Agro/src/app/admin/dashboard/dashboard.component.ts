import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartConfiguration, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { forkJoin, catchError, of, finalize } from 'rxjs';
import { MatCardModule } from "@angular/material/card";

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, MatCardModule, BaseChartDirective],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
   // === Datos de resumen (KPIs) ===
    summary = {
      totalProducers: 120,
      activeProducers: 95,
      totalOrders: 350,
      pendingOrders: 42,
      totalProducts: 180
    };
  
    // === Gráfica de Barras: Pedidos por mes ===
    barChartOptions: ChartConfiguration<'bar'>['options'] = {
      responsive: true,
      plugins: {
        legend: { display: false },
        title: { display: true, text: 'Pedidos por Mes' }
      },
      scales: {
        y: { beginAtZero: true }
      }
    };
  
    barChartData: ChartConfiguration<'bar'>['data'] = {
      labels: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct'],
      datasets: [
        { label: 'Pedidos', data: [20, 25, 40, 60, 50, 80, 90, 100, 85, 110], backgroundColor: '#4CAF50' }
      ]
    };
  
    // === Gráfica Doughnut: Top Productos ===
    doughnutChartOptions: ChartConfiguration<'doughnut'>['options'] = {
      responsive: true,
      plugins: {
        legend: { position: 'bottom' },
        title: { display: true, text: 'Top Productos Más Vendidos' }
      }
    };
  
    doughnutChartData: ChartConfiguration<'doughnut'>['data'] = {
      labels: ['Café Orgánico', 'Panela', 'Queso', 'Miel', 'Plátano'],
      datasets: [{
        data: [120, 90, 70, 50, 30],
        backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#8BC34A', '#9C27B0']
      }]
    };
  
    // === Gráfica de Línea: Productores Activos ===
    lineChartOptions: ChartConfiguration<'line'>['options'] = {
      responsive: true,
      plugins: {
        legend: { display: false },
        title: { display: true, text: 'Productores Activos (Últimos 6 Meses)' }
      },
      elements: {
        line: { tension: 0.3 }
      },
      scales: {
        y: { beginAtZero: true }
      }
    };
  
    lineChartData: ChartConfiguration<'line'>['data'] = {
      labels: ['Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov'],
      datasets: [
        {
          label: 'Productores Activos',
          data: [70, 75, 80, 90, 95, 100],
          fill: true,
          borderColor: '#2196F3',
          backgroundColor: 'rgba(33,150,243,0.2)',
          pointBackgroundColor: '#2196F3'
        }
      ]
    };
  
    constructor() {}
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }
  }