import { FarmSelectModel } from './../../../../../shared/models/farm/farm.model';
import { Component, inject, OnInit } from '@angular/core';
import { FarmService } from '../../../../../shared/services/farm/farm.service';
import { ButtonComponent } from "../../../../../shared/components/button/button.component";
import { ProductService } from '../../../../../shared/services/product/product.service';
import { ProductSelectModel } from '../../../../../shared/models/product/product.model';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';
import { CardComponent } from '../../../../../shared/components/cards/card/card.component';
import { ContainerCardFlexComponent } from "../../../../../shared/components/cards/container-card-flex/container-card-flex.component";

@Component({
  selector: 'app-product-list',
  imports: [ButtonComponent, CardComponent, CommonModule, ContainerCardFlexComponent],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit{
  private productService = inject(ProductService);
  private router = inject(Router);
  
  products: ProductSelectModel[] =[];
  ngOnInit(): void {
    this.loadProduct();
  }
  trackById = (_: number, p: ProductSelectModel) => p.id;


  loadProduct(){
    this.productService.getByProducerId().subscribe((data)=>{
      this.products = data;
    })
  }

  onEdit(p: ProductSelectModel) {
    this.router.navigate(['/account/producer/management/product/update', p.id]);
  }

  onDelete(p: ProductSelectModel) {
    Swal.fire({
      title: '¿Eliminar producto?',
      text: `Se eliminará "${p.name}". Esta acción no se puede deshacer.`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then(result => {
      if (!result.isConfirmed) return;

      this.productService.delete(p.id).subscribe({
        next: () => {
          // Remueve localmente sin recargar toda la lista
          this.products = this.products.filter(x => x.id !== p.id);
          Swal.fire('Eliminado', 'El producto fue eliminado.', 'success');
        },
        error: () => {
          Swal.fire('Error', 'No se pudo eliminar el producto.', 'error');
        }
      });
    });
  }

  

}
