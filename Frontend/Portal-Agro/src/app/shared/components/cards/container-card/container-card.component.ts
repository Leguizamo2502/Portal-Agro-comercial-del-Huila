import { Component, inject, Input } from '@angular/core';
import { ProductSelectModel } from '../../../models/product/product.model';
import { CardComponent } from '../card/card.component';
import { CommonModule } from '@angular/common';
import { FavoriteService } from '../../../services/favorite/favorite.service';

@Component({
  selector: 'app-container-card',
  imports: [CardComponent, CommonModule],
  templateUrl: './container-card.component.html',
  styleUrl: './container-card.component.css',
})
export class ContainerCardComponent {
  favoriteSrv = inject(FavoriteService);

  @Input() title = 'Últimos Agregados';
  @Input() showHeader = true;
  @Input({ required: true }) products: ProductSelectModel[] = [];

  trackById = (_: number, p: ProductSelectModel) => p.id;

  togglingId: number | null = null;
  onToggleFavorite(p: ProductSelectModel) {
    if (this.togglingId === p.id) return;
    this.togglingId = p.id;

    // UI optimista: aplica de inmediato y haz rollback si falla
    const original = !!p.isFavorite;
    p.isFavorite = !original;

    this.favoriteSrv.toggle(p.id, original).subscribe({
      next: (newState) => {
        p.isFavorite = newState;
      },
      error: (_) => {
        p.isFavorite = original;
      },
      complete: () => {
        this.togglingId = null;
      },
    });
  }
}
