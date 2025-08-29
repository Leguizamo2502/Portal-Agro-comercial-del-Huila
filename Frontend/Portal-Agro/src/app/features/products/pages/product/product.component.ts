import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormControl } from '@angular/forms';

import { ProductService } from '../../../../shared/services/product/product.service';
import { ProductSelectModel } from '../../../../shared/models/product/product.model';
import { CategoryService } from '../../../parameters/services/category/category.service';

// Tus cards
import { ContainerCardFlexComponent } from '../../../../shared/components/cards/container-card-flex/container-card-flex.component';

// Angular Material
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule }     from '@angular/material/select';
import { MatButtonModule }     from '@angular/material/button';
import { MatIconModule }       from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatChipsModule }      from '@angular/material/chips';
import { MatDividerModule }    from '@angular/material/divider';
import { CategoryNodeModel } from '../../../parameters/models/category/category.model';
import { ButtonComponent } from "../../../../shared/components/button/button.component";

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule,
    MatFormFieldModule, MatSelectModule, MatButtonModule, MatIconModule, MatProgressBarModule, MatChipsModule, MatDividerModule,
    ContainerCardFlexComponent,
    ButtonComponent
],
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  private productService  = inject(ProductService);
  private categoryService = inject(CategoryService);

  products: ProductSelectModel[] = [];
  categories: CategoryNodeModel[] = [];

  breadcrumb: { id: number; name: string }[] = [];
  selectedCategoryId: number | null = null;

  categoryCtrl = new FormControl<number | null>(null, { nonNullable: false });

  // estados de carga por tipo
  isLoadingProducts = false;
  isLoadingCategories = false;

  // true cuando el nivel actual NO tiene hijos
  atLeaf = false;

  ngOnInit(): void {
    this.loadRootCategories();
    this.loadProductsHome();
  }

  // ---------- Productos ----------
  private loadProductsHome(): void {
    this.isLoadingProducts = true;
    this.productService.getAllHome().subscribe({
      next: items => { this.products = items; this.isLoadingProducts = false; },
      error: err   => { console.error(err);  this.isLoadingProducts = false; }
    });
  }

  private loadProductsByCategory(categoryId: number): void {
    this.isLoadingProducts = true;
    this.productService.getByCategory(categoryId).subscribe({
      next: items => { this.products = items; this.isLoadingProducts = false; },
      error: err   => { console.error(err);  this.isLoadingProducts = false; }
    });
  }

  // ---------- Categorías ----------
  private loadRootCategories(): void {
    this.isLoadingCategories = true;
    this.categoryService.getNodes(null).subscribe({
      next: nodes => {
        this.categories = nodes;
        this.atLeaf = nodes.length === 0; // normalmente false en raíces
        this.isLoadingCategories = false;
      },
      error: err => { console.error(err); this.isLoadingCategories = false; }
    });
  }

  private loadChildren(parentId: number): void {
    this.isLoadingCategories = true;
    this.categoryService.getNodes(parentId).subscribe({
      next: nodes => {
        this.categories = nodes;
        this.atLeaf = nodes.length === 0; // si no hay hijos -> hoja
        // si es hoja, ocultaremos el selector de nivel y la barra de carga de categorías
        this.isLoadingCategories = false;
      },
      error: err => { console.error(err); this.isLoadingCategories = false; }
    });
  }

  // ---------- UI handlers ----------
  onSelectCategory(categoryId: number): void {
    const node = this.categories.find(c => c.id === categoryId);
    if (!node) return;

    this.selectedCategoryId = categoryId;
    this.pushToBreadcrumb(categoryId, node.name);
    this.loadProductsByCategory(categoryId);
    this.loadChildren(categoryId);
  }

  onBreadcrumbClick(index: number): void {
    const target = this.breadcrumb[index];
    this.breadcrumb = this.breadcrumb.slice(0, index + 1);
    this.selectedCategoryId = target.id;
    this.categoryCtrl.setValue(target.id, { emitEvent: false });
    this.loadProductsByCategory(target.id);
    this.loadChildren(target.id);
  }

  clearFilter(): void {
    this.selectedCategoryId = null;
    this.breadcrumb = [];
    this.categoryCtrl.reset(null, { emitEvent: false });
    this.loadRootCategories();
    this.loadProductsHome();
  }

  private pushToBreadcrumb(id: number, name: string): void {
    const existsIdx = this.breadcrumb.findIndex(b => b.id === id);
    if (existsIdx >= 0) this.breadcrumb = this.breadcrumb.slice(0, existsIdx + 1);
    else this.breadcrumb.push({ id, name });
  }

  trackById = (_: number, item: { id: number }) => item.id;
}