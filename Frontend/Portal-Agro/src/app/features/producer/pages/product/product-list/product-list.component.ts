import { FarmSelectModel } from './../../../../../shared/models/farm/farm.model';
import { Component, inject, OnInit } from '@angular/core';
import { FarmService } from '../../../../../shared/services/farm/farm.service';
import { ButtonComponent } from "../../../../../shared/components/button/button.component";
import { ProductService } from '../../../../../shared/services/product/product.service';
import { ProductSelectModel } from '../../../../../shared/models/product/product.model';
import { CardComponent } from "../../../../../shared/components/card/card.component";

@Component({
  selector: 'app-product-list',
  imports: [ButtonComponent, CardComponent],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit{
  private productService = inject(ProductService);
  
  products: ProductSelectModel[] =[];
  ngOnInit(): void {
    this.loadProduct();
  }


  loadProduct(){
    this.productService.getProduct().subscribe((data)=>{
      this.products = data;
    })
  }

  

}
