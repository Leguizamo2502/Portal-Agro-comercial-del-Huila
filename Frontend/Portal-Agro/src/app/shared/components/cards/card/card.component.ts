import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ProductSelectModel } from '../../../models/product/product.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule],
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent {
  router = inject(Router);

  @Input({ required: true }) product!: ProductSelectModel;
  @Input() showActions = false;

  @Input() showFavorite = false;
  @Input() isFavorite: boolean = false;
  @Input() disabledFavorite = false;

  @Output() edit = new EventEmitter<ProductSelectModel>();
  @Output() delete = new EventEmitter<ProductSelectModel>();
  @Output() toggleFavorite = new EventEmitter<ProductSelectModel>();

  private readonly placeholder = 'img/cargaImagen.png';

  get imageUrl(): string {
    const url = this.product?.images?.[0]?.imageUrl;
    return url && url.trim() ? url : this.placeholder;
  }

  onImgError(ev: Event) {
    (ev.target as HTMLImageElement).src = this.placeholder;
  }

  // Navegar al detalle
  onDetail(item: ProductSelectModel) {
    this.router.navigate(['/home/product', item.id]);
  }

  // Click favorito: detener propagación y emitir
  onFavoriteClick(ev: Event) {
    ev.stopPropagation();
    this.toggleFavorite.emit(this.product);
  }

  // Click editar/eliminar: detener propagación y emitir
  onEditClick(ev: Event) {
    ev.stopPropagation();
    this.edit.emit(this.product);
  }

  onDeleteClick(ev: Event) {
    ev.stopPropagation();
    this.delete.emit(this.product);
  }
}
