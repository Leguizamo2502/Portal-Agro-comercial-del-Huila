import { Component, inject, OnInit } from '@angular/core';
import { ProductService } from '../../../../shared/services/product/product.service';
import { ProductSelectModel } from '../../../../shared/models/product/product.model';
import { FavoriteService } from '../../../../shared/services/favorite/favorite.service';
import { ContainerCardFlexComponent } from "../../../../shared/components/cards/container-card-flex/container-card-flex.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-favorite',
  imports: [ContainerCardFlexComponent,CommonModule],
  templateUrl: './favorite.component.html',
  styleUrl: './favorite.component.css',
})
export class FavoriteComponent implements OnInit {
  private productService = inject(ProductService);
  private favoriteSrv = inject(FavoriteService);

  products: ProductSelectModel[] = [];
  togglingId: number | null = null;

  ngOnInit(): void {
    this.loadFavorites();
  }

  loadFavorites(): void {
    this.productService.getFavorites().subscribe((data) => {
      // Asegura que vengan marcados (el backend debería hacerlo)
      this.products = data.map((p) => ({ ...p, isFavorite: true }));
    });
  }

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
