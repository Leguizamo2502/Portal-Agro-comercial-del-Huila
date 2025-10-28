import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../../../shared/services/product/product.service';
import { ProductSelectModel } from '../../../../shared/models/product/product.model';
import { CarruselComponent } from '../../../../shared/components/carrusel/carrusel.component';
import { ContainerCardComponent } from "../../../../shared/components/cards/container-card/container-card.component";
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { finalize } from 'rxjs';
import { FarmingBannerComponent } from "../../../../shared/components/farming-banner/farming-banner.component";

//Guía Driver.js

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, CarruselComponent, ContainerCardComponent, ButtonComponent, FarmingBannerComponent],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  
  private productService = inject(ProductService);

  products: ProductSelectModel[] = [];
  productFeatured:ProductSelectModel[] = [];
  //loading
  loadingProducts = true;
  loadingFeatured = true;

  ngOnInit(): void {
    this.loadProduct();
    this.loadProductFeatured();

  }

  loadProduct() {
    this.loadingProducts = true;
    this.productService.getAllHome(15)
      .pipe(finalize(() => this.loadingProducts = false))
      .subscribe(data => {    
        this.products = data ?? [];
      });
  }
  
  loadProductFeatured() {
    this.loadingFeatured = true;
    this.productService.getFeatured()
      .pipe(finalize(() => this.loadingFeatured = false))
      .subscribe(data => {
        this.productFeatured = data ?? [];
      });
  }
}
