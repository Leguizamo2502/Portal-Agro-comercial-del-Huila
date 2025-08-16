import { Component, OnInit } from '@angular/core';
import { Product } from './product-model';
import { ProductService } from './product.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product-detail',
  imports: [CommonModule,FormsModule],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.css'
})
export class ProductDetailComponent implements OnInit{
  
  [key: string]: any;
  product!: Product;
  loading = true;
  newReview = '';

  constructor(private productService: ProductService ) {}

  ngOnInit(): void {
    this.productService.getProductDetail().subscribe(data => {
      this.product = data;
      this.loading = false;
    });
  }

  submitReview(): void {
    if (this.newReview.trim()) {
      this.product.reviews.push({
        user: 'Usuario de prueba',
        avatar: 'https://i.pinimg.com/736x/c5/9b/8d/c59b8d4acc0bd63cbba8732393871760.jpg',
        rating: 5,
        comment: this.newReview,
        date: new Date().toISOString()
      });
      this.newReview = '';
    }
  }
  selectedImage: string | null = null;

changeMainImage(img: string) {
  this.selectedImage = img;
}


}
