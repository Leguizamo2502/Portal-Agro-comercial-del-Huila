import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { Params, RouterLink } from '@angular/router';

@Component({
  selector: 'app-stat-card',
  imports: [CommonModule,MatIconModule,RouterLink],
  templateUrl: './stat-card.component.html',
  styleUrl: './stat-card.component.css'
})
export class StatCardComponent {
    /** Icono de Material (ej: 'hourglass_empty', 'check_circle') */
  @Input() icon: string = 'info';

  /** Texto descriptivo (ej: 'Pedidos pendientes') */
  @Input() text: string = '';

  /** Cantidad a mostrar (se formatea si es numérico) */
  @Input() count: number | string = 0;

  /** Hace la tarjeta clicable (cursor/hover + manejadores de teclado). */
  @Input() clickable = false;

  /** Variante de color para borde/icono. */
  @Input() color: 'success' | 'warning' | 'info' | 'neutral' = 'success';

  /** Estado de carga (muestra esqueleto). */
  @Input() loading = false;

  /** Navegación opcional: ruta y query params (si se define `to`, se usa <a [routerLink]>) */
  @Input() to?: string | any[];
  @Input() queryParams?: Params;
  @Input() fragment?: string;

  /** Evento alternativo si no usas routerLink. */
  @Output() clicked = new EventEmitter<void>();

  get isLink(): boolean {
    return Array.isArray(this.to) || typeof this.to === 'string';
  }

  get formattedCount(): string {
    if (typeof this.count === 'number') {
      return new Intl.NumberFormat('es-CO').format(this.count);
    }
    return String(this.count ?? '');
  }

  onActivateClick() {
    if (!this.isLink) this.clicked.emit();
  }

  onKeyDown(e: KeyboardEvent) {
    if (!this.clickable || this.isLink) return;
    if (e.key === 'Enter' || e.key === ' ') {
      e.preventDefault();
      this.clicked.emit();
    }
  }
}
