// product-detail.component.ts
import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { ProductSelectModel } from '../../models/product/product.model';
import { ProductService } from '../../services/product/product.service';
import { ReviewService } from '../../services/review/review.service';
import {
  ReviewRegisterModel,
  ReviewSelectModel,
} from '../../models/product/product.model'; // o desde review.models
import { ButtonComponent } from '../button/button.component';
import { firstValueFrom, Observable, of } from 'rxjs';
import { UserMeDto } from '../../../Core/Models/login.model';
import { AuthState } from '../../../Core/services/auth/auth.state';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, ButtonComponent],
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css'],
})
export class ProductDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private productService = inject(ProductService);
  private reviewService = inject(ReviewService);
  private authState = inject(AuthState);

  // Usuario actual (reactivo)
  me$: Observable<UserMeDto | null> = of(null);

  // Producto
  productId!: number;
  product!: ProductSelectModel;
  loadingProduct = true;

  // Imágenes
  selectedImage: string | null = null;

  // Reseñas
  reviews: ReviewSelectModel[] = [];
  loadingReviews = true;

  // Nueva reseña (UI)
  newReview = '';
  selectedRating = 0;
  stars = Array(5).fill(0);
  
  // Métricas
  averageRating = 0;
  distribution: { star: number; count: number; percentage: number }[] = [];
  totalReviews = 0;

  Math = Math; // para usar Math en template

  ngOnInit(): void {
    // Usuario actual (desde storage y backend)
    this.authState.hydrateFromStorage();
    this.me$ = this.authState.loadMe();

    // Id de producto
    this.productId = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.productId) return;

    // Carga de datos
    this.loadProduct();
    this.loadReviews();
  }

  private loadProduct(): void {
    this.loadingProduct = true;
    this.productService.getById(this.productId).subscribe({
      next: (data) => {
        this.product = data;
        this.loadingProduct = false;
        if (this.product.images?.length) {
          this.selectedImage = this.product.images[0].imageUrl;
        }
      },
      error: () => {
        this.loadingProduct = false;
      },
    });
  }

  private loadReviews(): void {
    this.loadingReviews = true;
    this.reviewService.getReviewByProduct(this.productId).subscribe({
      next: (list) => {
        this.reviews = list ?? [];
        this.recomputeStats();
        this.loadingReviews = false;
        // console.log(this.reviews);
      },
      error: () => {
        this.loadingReviews = false;
      },
    });
  }

  changeMainImage(url: string): void {
    this.selectedImage = url;
  }

  hoverRating = 0;
  // Selección de estrellas
  setRating(value: number): void {
    this.selectedRating = value;
  }

  onMouseEnter(rating: number) {
  this.hoverRating = rating;
  }

  onMouseLeave() {
  this.hoverRating = 0;
  }

  submitReview(): void {
    if (this.selectedRating < 1 || this.selectedRating > 5) return;
    const comment = this.newReview.trim();
    if (!comment) return;

    const payload: ReviewRegisterModel = {
      productId: this.productId,
      rating: this.selectedRating,
      comment,
    };

    this.reviewService.createReview(payload).subscribe({
      next: (created) => {
        this.reviews.unshift(created);
        this.newReview = '';
        this.selectedRating = 0;
        this.recomputeStats();

        Swal.fire({
          toast: true,
          position: 'bottom-end',
          timer: 1500,
          showConfirmButton: false,
          icon: 'success',
          title: 'Reseña publicada',
        });
      },
      error: (err) => {
        Swal.fire({
          toast: true,
          position: 'top-end',
          timer: 2000,
          showConfirmButton: false,
          icon: 'error',
          title: err?.error?.message ?? 'No se pudo publicar la reseña',
        });
      },
    });
  }

  deleteReview(reviewId: number): void {
    Swal.fire({
      title: '¿Eliminar reseña?',
      text: 'Esta acción no se puede deshacer.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar',
      reverseButtons: true,
      showLoaderOnConfirm: true,
      allowOutsideClick: () => !Swal.isLoading(),
      preConfirm: async () => {
        try {
          await firstValueFrom(this.reviewService.deleteReview(reviewId));
        } catch (err: any) {
          Swal.showValidationMessage(
            err?.error?.message ?? 'No se pudo eliminar la reseña'
          );
          throw err; // mantiene el modal abierto si falla
        }
      },
    }).then((result) => {
      if (result.isConfirmed) {
        // éxito: ya se eliminó en el preConfirm
        this.reviews = this.reviews.filter((r) => r.id !== reviewId);
        this.recomputeStats();

        Swal.fire({
          toast: true,
          position: 'bottom-end',
          timer: 1500,
          showConfirmButton: false,
          icon: 'success',
          title: 'Reseña eliminada',
        });
      }
    });
  }

  private recomputeStats(): void {
    const n = this.reviews.length;
    this.totalReviews = n;

    if (n === 0) {
      this.averageRating = 0;
      this.distribution = [5, 4, 3, 2, 1].map((star) => ({
        star,
        count: 0,
        percentage: 0,
      }));
      return;
    }

    const sum = this.reviews.reduce((acc, r) => acc + (r.rating || 0), 0);
    this.averageRating = sum / n;

    const counts: Record<number, number> = { 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 };
    for (const r of this.reviews)
      counts[r.rating] = (counts[r.rating] ?? 0) + 1;

    this.distribution = [5, 4, 3, 2, 1].map((star) => {
      const count = counts[star] ?? 0;
      const percentage = (count / n) * 100;
      return { star, count, percentage };
    });
  }

  trackByReview = (_: number, r: ReviewSelectModel) => r.id;
}
