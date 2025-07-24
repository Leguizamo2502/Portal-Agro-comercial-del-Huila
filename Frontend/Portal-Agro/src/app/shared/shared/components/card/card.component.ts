import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MOCK_CARDS } from '../mock/mock';
import { Card } from '../card/card.model';

@Component({
  selector: 'app-card',
  standalone: true, // <-- ya lo tienes
  imports: [CommonModule], // <-- AÑADE ESTO
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
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
