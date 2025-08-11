import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-stat-card',
  imports: [CommonModule,MatIconModule],
  templateUrl: './stat-card.component.html',
  styleUrl: './stat-card.component.css'
})
export class StatCardComponent {
   /** Nombre del ícono de Material (ej: 'hourglass_empty', 'check_circle') */
  @Input() icon: string = 'info';
  /** Texto descriptivo (ej: 'Pedidos pendientes:') */
  @Input() text: string = '';
  /** Cantidad a mostrar */
  @Input() count: number | string = 0;
}
