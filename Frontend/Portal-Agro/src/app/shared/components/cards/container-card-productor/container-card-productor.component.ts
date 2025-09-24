import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductSelectModel } from '../../../models/product/product.model';
import { FarmSelectModel } from '../../../models/farm/farm.model';
import { CardComponent } from '../card/card.component';
import { CardFarmComponent } from '../card-farm/card-farm.component';

type Item = ProductSelectModel | FarmSelectModel;

@Component({
  selector: 'app-container-card-productor',
  standalone: true,
  imports: [CommonModule, CardComponent, CardFarmComponent],
  templateUrl: './container-card-productor.component.html',
  styleUrl: './container-card-productor.component.css',
})
export class ContainerCardProductorComponent {
  /** 'product' => usa <app-card>; 'farm' => usa <app-card-farm> */
  @Input() type: 'product' | 'farm' = 'product';

  /** Lista de ítems (según el type) */
  @Input({ required: true }) items: Item[] = [];

  /** Opcionales comunes */
  

  

  trackById = (_: number, it: Item) => (it as any).id;

  // Helpers de tipado
  isProduct(it: Item): it is ProductSelectModel {
    return (it as ProductSelectModel).price !== undefined;
  }

  isFarm(it: Item): it is FarmSelectModel {
    return (it as FarmSelectModel).hectares !== undefined;
  }
}
