import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonComponent } from "../button/button.component";

@Component({
  selector: 'app-carrusel',
  imports: [CommonModule, ButtonComponent],
  templateUrl: './carrusel.component.html',
  styleUrl: './carrusel.component.css'
})
export class CarruselComponent {
  slides = [
    {
      src: 'assets/vacas.jpg',
      part1: 'Conectando los',
      highlight: 'Productores',
      part2: 'de nuestra tierra'
    },
    {
      src: 'assets/descarga.jpg',
      part1: 'Del campo a tu',
      highlight: 'mesa',
      part2: 'productos frescos y de calidad'
    },
    {
      src: 'assets/Wild and beautiful great plains of south dakota a golden landscape with native grasslands and _ Premium AI-generated image.jpg',
      part1: 'Apoyando a los',
      highlight: 'agricultores',
      part2: 'locales'
    }
  ];
}