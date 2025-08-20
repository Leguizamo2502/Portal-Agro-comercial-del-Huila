import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ProductSelectModel } from '../../../models/product/product.model';
import { CardComponent } from "../card/card.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-container-card-flex',
  imports: [CardComponent,CommonModule],
  templateUrl: './container-card-flex.component.html',
  styleUrl: './container-card-flex.component.css'
})
export class ContainerCardFlexComponent {
  @Input({ required: true }) products: ProductSelectModel[] = [];

  // Opcionales: controla si se muestran acciones (editar/eliminar) y la estrellita
  @Input() showActions = false;
  @Input() showFavorite = false;

  // Para deshabilitar el botón favorito en items específicos (opcional)
  @Input() togglingId: number | null = null;

  // Eventos hacia el padre
  @Output() edit = new EventEmitter<ProductSelectModel>();
  @Output() delete = new EventEmitter<ProductSelectModel>();
  @Output() toggleFavorite = new EventEmitter<ProductSelectModel>();

  trackById = (_: number, p: ProductSelectModel) => p.id;
}
