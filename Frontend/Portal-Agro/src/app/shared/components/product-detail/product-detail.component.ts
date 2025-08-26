import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductSelectModel } from '../../models/product/product.model';
import { ProductService } from '../../services/product/product.service';
import { ButtonComponent } from '../button/button.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, ButtonComponent],
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css'],
})
export class ProductDetailComponent implements OnInit {
  product!: ProductSelectModel & { reviews: any[] }; // extendemos con reviews
  loading = true;
  selectedImage: string | null = null;
  route = inject(ActivatedRoute);

  Math = Math;

  // para la reseña nueva
  newReview: string = '';
  selectedRating: number = 0; // rating que el usuario selecciona
  stars = Array(5).fill(0); // arreglo de 5 estrellas

  // ratings
  averageRating: number = 0;
  distribution: { star: number; count: number; percentage: number }[] = [];
  totalReviews: number = 0;

  constructor(private productService: ProductService) {}

  productId!: number;

  ngOnInit(): void {
    this.productId = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.productId) return;

    this.productService.getById(this.productId).subscribe((data) => {
      // agregar reseñas mock
      this.product = {
        ...data,
        reviews: [
          {
            user: 'Isabella Riel',
            avatar:
              'https://i.pinimg.com/1200x/21/21/6b/21216b7b9f889e2f9619dc59ef138497.jpg',
            rating: 3,
            comment: 'Excelente calidad y frescura.',
            date: new Date('2025-08-10'),
          },
          {
            user: 'Marcos Alzate',
            avatar:
              'https://i.pinimg.com/736x/36/5f/40/365f40852f2e121163f8636a09d23491.jpg',
            rating: 5,
            comment: 'Muy buen sabor, pero me gustaría más tamaño.',
            date: new Date('2025-08-12'),
          },
        ],
      };

      this.calculateRatings(); // calcula promedio + barras
      this.loading = false;
    });
  }

  changeMainImage(imgUrl: string) {
    this.selectedImage = imgUrl;
  }

  // seleccionar estrellas
  setRating(rating: number) {
    this.selectedRating = rating;
  }

  submitReview() {
    if (!this.newReview.trim() || this.selectedRating === 0) return;

    this.product.reviews.unshift({
      user: 'Usuario Demo', // en futuro: usuario logueado
      avatar: 'https://i.pravatar.cc/40',
      rating: this.selectedRating,
      comment: this.newReview,
      date: new Date(),
    });

    // limpiar inputs
    this.newReview = '';
    this.selectedRating = 0;

    this.calculateRatings(); // recalcular después de nueva reseña
  }

  private calculateRatings() {
    if (!this.product?.reviews?.length) return;

    this.totalReviews = this.product.reviews.length;
    const sum = this.product.reviews.reduce(
      (acc, r) => acc + (r.rating || 0),
      0
    );
    this.averageRating = sum / this.totalReviews;

    this.distribution = [1, 2, 3, 4, 5]
      .map((star) => {
        const count = this.product.reviews.filter((r) => r.rating === star)
          .length;
        const percentage = (count / this.totalReviews) * 100;
        return { star, count, percentage };
      })
      .reverse();
  }

  getRoundedAverage(): number {
    return Math.round(this.averageRating);
  }
}
