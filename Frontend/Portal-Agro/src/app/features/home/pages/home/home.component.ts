import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../../../shared/services/product/product.service';
import { ProductSelectModel } from '../../../../shared/models/product/product.model';
import { CarruselComponent } from '../../../../shared/components/carrusel/carrusel.component';
import { ContainerCardComponent } from "../../../../shared/components/cards/container-card/container-card.component";
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { finalize } from 'rxjs';

//Guía Driver.js
import { DriverJsService } from '../../../../shared/services/driverJS/driver-js.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, CarruselComponent, ContainerCardComponent,ButtonComponent],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  
  private productService = inject(ProductService);
  private driverService = inject(DriverJsService);

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
  startHomeTour() {
    const steps = [
      {
        element: '#carrusel',
        popover: {
          title: 'Carrusel',
          description: 'Aquí se muestran los banners destacados.',
          side: 'bottom' as const
        }
      },
      {
        element: '#ultimosAgregados',
        popover: {
          title: 'Últimos Agregados',
          description: 'Productos que se han agregado recientemente.',
          side: 'top' as const
        }
      },
      {
        element: '#productosDestacados',
        popover: {
          title: 'Productos Destacados',
          description: 'Nuestros productos más recomendados.',
          side: 'top' as const
        }
      },
      {
        element: '#explorarBtn',
        popover: {
          title: 'Explorar Productos',
          description: 'Haz clic aquí para ir a la página de todos los productos.',
          side: 'top' as const
        }
      }
    ];

    this.driverService.startTour(steps);
  }
}
