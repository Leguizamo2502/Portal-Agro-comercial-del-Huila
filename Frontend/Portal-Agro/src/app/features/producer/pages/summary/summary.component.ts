import { Component } from '@angular/core';
import { StatCardComponent } from "../../../../shared/components/stat-card/stat-card.component";
import { ChartConfiguration, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';

@Component({
  selector: 'app-summary',
  imports: [StatCardComponent, BaseChartDirective ],
  templateUrl: './summary.component.html',
  styleUrl: './summary.component.css'
})
export class SummaryComponent {

   // Datos quemados
  barChartData: ChartConfiguration<'bar'>['data'] = {
    labels: ['Café', 'Maíz', 'Cacao', 'Frijol', 'Arroz'],
    datasets: [
      {
        label: 'Ventas',
        data: [120, 90, 150, 70, 110],
        backgroundColor: ['#42A5F5', '#66BB6A', '#FFA726', '#AB47BC'], //color de barra (de cada una)
        
      }
    ]
  };

  // Opciones del gráfico
  barChartOptions: ChartOptions<'bar'> = {
    responsive: true,
  plugins: {
    legend: {
      display: true,
      position: 'top',
      align:'start',
      labels: {
        color: '#333',
        font: {
          size: 14,
          weight:'bold',
        },
      },
    },
  },
  };
}
