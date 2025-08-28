import { Component, inject, OnInit } from '@angular/core';
import { ProductService } from '../../../../shared/services/product/product.service';
import { ProductSelectModel } from '../../../../shared/models/product/product.model';
import { ContainerCardFlexComponent } from "../../../../shared/components/cards/container-card-flex/container-card-flex.component";
import { CategoryService } from '../../../parameters/services/category/category.service';
import { CategorySelectModel } from '../../../parameters/models/category/category.model';
import { CategoryPillComponent } from "../../../../shared/components/cards/category-pill/category-pill.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product',
  imports: [ContainerCardFlexComponent, CategoryPillComponent,CommonModule],
  templateUrl: './product.component.html',
  styleUrl: './product.component.css'
})
export class ProductComponent implements OnInit{
  private productService = inject(ProductService);
  products:ProductSelectModel[]=[];
  private categoryService = inject(CategoryService);
  categories:CategorySelectModel[]=[];

  ngOnInit(): void {
    this.loadProducts();
    this.loadCategories();
  }


  loadProducts(){
    this.productService.getAllHome().subscribe({
      next:(products)=>{
        this.products=products;
        // console.log(this.products);
      },
      error:(err)=>{
        console.log(err);
      }
    })
  }

  loadCategories(){
    this.categoryService.getAll().subscribe({
      next:(categories)=>{
        this.categories=categories;
        console.log(this.categories);
      },
      error:(err)=>{
        console.log(err);
      }
    })
  }

}
