import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Producer } from '../../shared/models/producer-profile/producer-profile.model';
import { ProducerService } from './producer.service';

@Component({
  selector: 'app-producer-profile',
  imports: [CommonModule],
  templateUrl: './producer-profile.component.html',
  styleUrls: ['./producer-profile.component.css'],
})
export class ProducerProfileComponent implements OnInit {
  producer?: Producer;

  constructor(private producerService: ProducerService) {}

  ngOnInit(): void {

    // this.producerService.getProducerProfile().subscribe({
    //   next: (data) => this.producer = data,
    //   error: (err) => console.error('Error cargando productor:', err)
    // });

    this.producer = {
    id: 1,
    description: 'Productores de café orgánico y frutas tropicales.',
    code: 'PROD123',
    qrUrl: 'https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=PROD123',
    user: {
      id: 1,
      name: 'Juan Felipe',
      email: 'veresauriot@gmail.com',
      active: true,
      imageUrl: 'https://i.pinimg.com/736x/54/96/78/549678ed467d9ac5799c12a96d96c233.jpg'
    },
    farms: [
      {
        id: 1,
        name: 'Finca El Paraíso',
        location: 'Antioquia, Colombia',
        hectares: 20,
        altitude: 1800,
        latitude: 6.2518,
        longitude: -75.5636,
        imageUrl: 'https://i.pinimg.com/736x/06/50/59/06505944c3781610c0d46691777992e4.jpg'
      },
      {
        id: 2,
        name: 'Villa Leo',
        location: 'Nariño, Colombia',
        hectares: 15,
        altitude: 2000,
        latitude: 1.2,
        longitude: -77.3,
        imageUrl: 'https://i.pinimg.com/736x/e6/b5/fb/e6b5fb2110f94e75df89f335cf0f9ea7.jpg'
      }
    ],
    products: [
      {
        id: 1,
        name: 'Café Orgánico',
        price: 25000,
        description: 'Café cultivado a 1800 msnm, proceso lavado.',
        stock: 120,
        imageUrl: 'https://i.pinimg.com/736x/c4/92/ba/c492ba241ebe80fbc03250506b73de33.jpg'
      },
      {
        id: 2,
        name: 'Banano',
        price: 1800,
        description: 'Banano dulce cultivado en tierra fértil.',
        stock: 300,
        imageUrl: 'https://i.pinimg.com/736x/36/41/4c/36414cfe178e4d1d4ca0c93ec4bccff9.jpg'
      },
      {
        id: 3,
        name: 'Miel Natural',
        price: 15000,
        description: 'Miel 100% orgánica de abejas locales.',
        stock: 45,
        imageUrl: 'https://i.pinimg.com/736x/79/11/a8/7911a835d0e02dca6bb4ee1600ad8f7b.jpg'
      }
    ]
  };

  }
}