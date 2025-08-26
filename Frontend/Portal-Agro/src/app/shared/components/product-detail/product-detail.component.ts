import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductSelectModel } from '../../models/product/product.model';
import { ProductService } from '../../services/product/product.service';
import { ButtonComponent } from "../button/button.component";

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, ButtonComponent],
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {
  product!: ProductSelectModel & { reviews: any[] }; // extendemos con reviews mock
  loading = true;
  selectedImage: string | null = null;

  // para la reseña nueva
  newReview: string = '';

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    // TODO: más adelante se puede leer el ID desde la ruta con ActivatedRoute
    const productId = 1;
    this.productService.getById(productId).subscribe(data => {
      // aquí agregamos reseñas de prueba si no vienen del backend
      this.product = {
        ...data,
        reviews: [
          {
            user: 'Isabella Riel',
            avatar: 'https://i.pinimg.com/1200x/21/21/6b/21216b7b9f889e2f9619dc59ef138497.jpg',
            comment: 'Excelente calidad y frescura.',
            date: new Date('2025-08-10')
          },
          {
            user: 'Marcos Alzate',
            avatar: 'https://i.pinimg.com/736x/36/5f/40/365f40852f2e121163f8636a09d23491.jpg',
            comment: 'Muy buen sabor, pero me gustaría más tamaño.',
            date: new Date('2025-08-12')
          }
        ]
      };
      this.loading = false;
    });
  }

  changeMainImage(imgUrl: string) {
    this.selectedImage = imgUrl;
  }

  submitReview() {
    if (!this.newReview.trim()) return;

    this.product.reviews.unshift({
      user: 'Usuario Demo', // en futuro: usuario logueado
      avatar: 'https://i.pravatar.cc/40', // avatar genérico
      comment: this.newReview,
      date: new Date()
    });

    this.newReview = ''; // limpiar textarea
  }
}
