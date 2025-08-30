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
  @Input() title = 'Últimos Agregados';
  @Input() showHeader = true;
  @Input() showFavorite = true; // <- define si el card muestra el botón
  @Input({ required: true }) products: ProductSelectModel[] = [];

  trackById = (_: number, p: ProductSelectModel) => p.id;
}
