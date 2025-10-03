import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductSelectModel, StockUpdateModel } from '../../../models/product/product.model';
import { FarmSelectModel } from '../../../models/farm/farm.model';
import { CardComponent } from '../card/card.component';
import { CardFarmComponent } from '../card-farm/card-farm.component';

type Item = ProductSelectModel | FarmSelectModel;

@Component({
  selector: 'app-container-card-flex',
  standalone: true,
  imports: [CommonModule, CardComponent, CardFarmComponent],
  templateUrl: './container-card-flex.component.html',
  styleUrl: './container-card-flex.component.css',
})
export class ContainerCardFlexComponent {
  /** 'product' => usa <app-card>; 'farm' => usa <app-card-farm> */
  @Input() type: 'product' | 'farm' = 'product';

  /** Lista de ítems (según el type) */
  @Input({ required: true }) items: Item[] = [];

  /** Opcionales comunes */
  @Input() showActions = false;
  @Input() showFavorite = false; // solo aplica a product
  @Input() togglingId: number | null = null; // solo aplica a product

  /** Eventos comunes */
  @Output() editProduct = new EventEmitter<ProductSelectModel>();
  @Output() deleteProduct = new EventEmitter<ProductSelectModel>();
  @Output() editFarm = new EventEmitter<FarmSelectModel>();
  @Output() deleteFarm = new EventEmitter<FarmSelectModel>();
  @Output() toggleFavorite = new EventEmitter<ProductSelectModel>(); // solo product
  @Output() editStock = new EventEmitter<StockUpdateModel>();

   //loading
  @Input() loading = false;
  @Input() skeletonCount = 6;

  get skeletons(): number[] {
    return Array.from({ length: this.skeletonCount });
  }

  trackById = (_: number, it: Item) => (it as any).id;

  // Helpers de tipado
  isProduct(it: Item): it is ProductSelectModel {
    return (it as ProductSelectModel).price !== undefined;
  }

  

  isFarm(it: Item): it is FarmSelectModel {
    return (it as FarmSelectModel).hectares !== undefined;
  }
}
