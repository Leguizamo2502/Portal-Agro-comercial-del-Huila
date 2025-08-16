import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Product } from './product-model';

@Injectable({ providedIn: 'root' })
export class ProductService {

  getProductDetail(): Observable<Product> {
    return of({
      id: 1,
      name: 'Plátano Dominico Hartón',
      image: 'https://upload.wikimedia.org/wikipedia/commons/4/4c/Bananas.jpg',
      thumbnails: [
        'https://upload.wikimedia.org/wikipedia/commons/4/4c/Bananas.jpg',
        'https://i.pinimg.com/736x/5d/e9/22/5de9226b74c2d216310205611319a8f7.jpg',
        'https://i.pinimg.com/736x/e3/a8/b6/e3a8b6a01cdb33ed4dc859734ac80c84.jpg'
      ],
      orders: 100,
      producer: 'Juan Lozada',
      categories: ['Frutas', 'Plátano'],
      stock: 50,
      price: 60000,
      description: 'El Plátano Dominico Hartón es una fruta tropical muy valorada...',
      moreInfo: 'Información de producción: 400 bultos cada 5 meses.',
      location: 'Neiva - Huila',
      reviews: [
        {
          user: 'Isabella Riel',
          avatar: 'https://i.pinimg.com/1200x/21/21/6b/21216b7b9f889e2f9619dc59ef138497.jpg',
          rating: 5,
          comment: 'Excelente calidad y frescura...',
          date: '2025-08-10'
        },
        {
          user: 'Marcos Alzate',
          avatar: 'https://i.pinimg.com/736x/36/5f/40/365f40852f2e121163f8636a09d23491.jpg',
          rating: 4,
          comment: 'Muy buen sabor, pero me gustaría más tamaño...',
          date: '2025-08-12'
        }
      ]
    });
  }
}
