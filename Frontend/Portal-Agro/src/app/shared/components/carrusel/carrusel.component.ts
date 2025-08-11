import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-carrusel',
  imports: [CommonModule],
  templateUrl: './carrusel.component.html',
  styleUrl: './carrusel.component.css'
})
export class CarruselComponent {

  slides = [
    {
      src: 'assets/Wild and beautiful great plains of south dakota a golden landscape with native grasslands and _ Premium AI-generated image.jpg',
      title: 'Conectando los Productores de nuestra tierra'
    },
    {
      src: 'assets/descarga.jpg',
      title: 'Del campo a tu mesa, productos frescos y de calidad'
    },
    {
      src: 'assets/vacas.jpg',
      title: 'Apoyando a los agricultores locales'
    }
  ];

}