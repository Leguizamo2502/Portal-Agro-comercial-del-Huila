import { Component, inject, OnInit } from '@angular/core';
import { ProductService } from '../../../../shared/services/product/product.service';
import { ProductSelectModel } from '../../../../shared/models/product/product.model';
import { CommonModule } from '@angular/common';
import { ContainerCardFlexComponent } from "../../../../shared/components/cards/container-card-flex/container-card-flex.component";
import { FavoriteFacadeService } from '../../../../shared/services/favorite/favorite-facade.service';
import { finalize, Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-favorite',
  imports: [CommonModule, ContainerCardFlexComponent],
  templateUrl: './favorite.component.html',
  styleUrl: './favorite.component.css',
})
export class FavoriteComponent implements OnInit {
   private productService = inject(ProductService);
  private fav = inject(FavoriteFacadeService);
  private destroy$ = new Subject<void>();

  products: ProductSelectModel[] = [];

  loadingProducts = true;

  ngOnInit(): void {
    this.loadFavorites();

    // Quita de la lista cuando deje de ser favorito en cualquier parte
    this.fav.changes$
      .pipe(takeUntil(this.destroy$))
      .subscribe(({ id, isFavorite }) => {
        // if (!isFavorite) {
        //   this.products = this.products.filter(p => p.id !== id);
        // }
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadFavorites(): void {
    this.loadingProducts = true
    this.productService.getFavorites().pipe(finalize(()=> this.loadingProducts = false))
    .subscribe((data) => {
      this.products = data;
    });
  }
}
