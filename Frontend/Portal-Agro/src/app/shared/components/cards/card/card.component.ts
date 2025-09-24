import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import {
  ProductSelectModel,
  StockUpdateModel,
} from '../../../models/product/product.model';
import { Router } from '@angular/router';
import { FavoriteFacadeService } from '../../../services/favorite/favorite-facade.service';
import { IfLoggedInDirective } from '../../../../Core/directives/if-logged-in.directive';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    IfLoggedInDirective,
  ],
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css'],
})
export class CardComponent {
  private fav = inject(FavoriteFacadeService);
  router = inject(Router);

  @Input({ required: true }) product!: ProductSelectModel;
  @Input() showActions = false;
  @Input() showFavorite = false;

  // (mantén los outputs si los usas para otras acciones)
  @Output() edit = new EventEmitter<ProductSelectModel>();
  @Output() delete = new EventEmitter<ProductSelectModel>();
  @Output() editStack = new EventEmitter<StockUpdateModel>();

  private readonly placeholder = 'img/cargaImagen.png';

  get imageUrl(): string {
    const url = this.product?.images?.[0]?.imageUrl;
    return url && url.trim() ? url : this.placeholder;
  }

  get disabledFavorite(): boolean {
    return this.fav.isToggling(this.product?.id);
  }

  onImgError(ev: Event) {
    (ev.target as HTMLImageElement).src = this.placeholder;
  }
  onDetail(item: ProductSelectModel) {
    this.router.navigate(['/home/product', item.id]);
  }
  onEditClick(ev: Event) {
    ev.stopPropagation();
    this.edit.emit(this.product);
  }
  onDeleteClick(ev: Event) {
    ev.stopPropagation();
    this.delete.emit(this.product);
  }
  onEditStockClick(ev: Event) {
    ev.stopPropagation();
    this.editStack.emit({
      productId: this.product.id,
      newStock: this.product.stock,
    });
  }

  // favorito con UI optimista centralizada
  onFavoriteClick(ev: Event) {
    ev.stopPropagation();
    this.fav.toggle(this.product).subscribe({
      next: (isFav) => {
        Swal.fire({
          toast: true,
          position: 'top-end',
          timer: 1500,
          showConfirmButton: false,
          icon: 'success',
          title: isFav ? 'Añadido a favoritos' : 'Quitado de favoritos',
        });
      },
      error: () => {
        Swal.fire({
          toast: true,
          position: 'top-end',
          timer: 2000,
          showConfirmButton: false,
          icon: 'error',
          title: 'No se pudo actualizar el favorito',
        });
      },
    });
  }
}
