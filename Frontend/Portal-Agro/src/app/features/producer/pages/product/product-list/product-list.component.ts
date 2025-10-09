import { FarmSelectModel } from './../../../../../shared/models/farm/farm.model';
import { Component, inject, OnInit } from '@angular/core';
import { FarmService } from '../../../../../shared/services/farm/farm.service';
import { ButtonComponent } from "../../../../../shared/components/button/button.component";
import { ProductService } from '../../../../../shared/services/product/product.service';
import { ProductSelectModel, StockUpdateModel } from '../../../../../shared/models/product/product.model';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';
import { ContainerCardFlexComponent } from "../../../../../shared/components/cards/container-card-flex/container-card-flex.component";
import { StockDialogComponent } from '../components/stock-dialog/stock-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-product-list',
  imports: [ButtonComponent, CommonModule, ContainerCardFlexComponent],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit{
  private productService = inject(ProductService);
  private router = inject(Router);
  private dialog = inject(MatDialog);

  loadingProducts = true;
  
  products: ProductSelectModel[] =[];

  ngOnInit(): void {
    this.loadProduct();
  }
  trackById = (_: number, p: ProductSelectModel) => p.id;


  loadProduct(){
    this.loadingProducts = true;
    this.productService.getByProducerId().pipe(finalize(()=> this.loadingProducts = false))
    .subscribe(data=>{
      this.products = data ?? []; 
    }
    )
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

  onEditStock = (payload: StockUpdateModel) => {
    const product = this.products.find(x => x.id === payload.productId);
    const dialogRef = this.dialog.open(StockDialogComponent, {
      width: '420px',
      data: {
        productId: payload.productId,
        currentStock: product?.stock ?? payload.newStock ?? 0,
        productName: product?.name
      }
    });

    dialogRef.afterClosed().subscribe((result?: StockUpdateModel) => {
      if (!result) return;

      const prev = product?.stock;
      if (product) product.stock = result.newStock; // optimista

      this.productService.updateStock(result).subscribe({
        next: () => {
          Swal.fire({ toast: true, position: 'top-end', timer: 1500, showConfirmButton: false,
                      icon: 'success', title: 'Stock actualizado' });
        },
        error: () => {
          if (product && prev != null) product.stock = prev; // revertir
          Swal.fire({ toast: true, position: 'top-end', timer: 2000, showConfirmButton: false,
                      icon: 'error', title: 'No se pudo actualizar el stock' });
        }
      });
    });
  };

  

}
