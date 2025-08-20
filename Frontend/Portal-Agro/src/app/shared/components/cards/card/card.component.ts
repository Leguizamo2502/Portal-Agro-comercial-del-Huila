import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ProductSelectModel } from '../../../models/product/product.model';

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent {
  @Input({ required: true }) product!: ProductSelectModel;

  // Acciones de edición/eliminación existentes
  @Input() showActions = false;

  // Nuevo: controlar visibilidad y estado del favorito
  @Input() showFavorite = false;       // muestra/oculta el botón estrella
  @Input() isFavorite: boolean = false; // estado actual del favorito
  @Input() disabledFavorite = false;
  // Eventos hacia el padre
  @Output() edit = new EventEmitter<ProductSelectModel>();
  @Output() delete = new EventEmitter<ProductSelectModel>();
  @Output() toggleFavorite = new EventEmitter<ProductSelectModel>();

  private readonly placeholder = 'img/cargaImagen.png';

  get imageUrl(): string {
    const url = this.product?.images?.[0]?.imageUrl;
    return url && url.trim() ? url : this.placeholder;
    // Si tu DTO ya trae imageUrl plano, podrías usar: return this.product.imageUrl ?? this.placeholder;
  }

  onImgError(ev: Event) {
    (ev.target as HTMLImageElement).src = this.placeholder;
  }

  onToggleFavorite() {
    this.toggleFavorite.emit(this.product);
  }
}
