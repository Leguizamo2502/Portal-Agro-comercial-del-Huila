import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../../../shared/services/product/product.service';
import { ProductSelectModel } from '../../../../shared/models/product/product.model';
import { CarruselComponent } from '../../../../shared/components/carrusel/carrusel.component';
import { ContainerCardComponent } from "../../../../shared/components/container-card/container-card.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, CarruselComponent, ContainerCardComponent],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  private productService = inject(ProductService);
  products: ProductSelectModel[] = [];

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    this.productService.getAll().subscribe(data => {
      this.products = data;
      console.log('HomeComponent - productos cargados:', this.products);
    });
  }
}
