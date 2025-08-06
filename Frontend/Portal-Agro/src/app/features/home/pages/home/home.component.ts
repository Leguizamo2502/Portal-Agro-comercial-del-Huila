import { Component, inject, OnInit } from '@angular/core';
import { ProductService } from '../../../../shared/services/product/product.service';
import { ProductSelectModel } from '../../../../shared/models/product/product.model';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  
  productService = inject(ProductService);
  products : ProductSelectModel[]=[]
  
  
  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct(){
    this.productService.getProduct().subscribe((data)=>{
      this.products = data;
      console.log(data);
    })
  }
}
