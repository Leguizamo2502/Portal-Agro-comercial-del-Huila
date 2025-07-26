import { Component } from '@angular/core';
import { Card } from './card.model';
import { MOCK_CARDS } from '../mock/mock';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-card',
  imports: [CommonModule],
  templateUrl: './card.component.html',
  styleUrl: './card.component.css'
})
export class CardComponent {
  cards: Card[] = MOCK_CARDS;
  
    getImage(url: string): string {
      const defaultImage = 'https://via.placeholder.com/300x200?text=Imagen+no+disponible';
      return url?.trim() ? url : defaultImage;
    }
  
    verMas(productName: string): void {
      alert(`Producto: ${productName}`);
    }
}
