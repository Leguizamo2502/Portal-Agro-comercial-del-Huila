// product.component.ts
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormControl } from '@angular/forms';

import { ProductService } from '../../../../shared/services/product/product.service';
import { ProductSelectModel } from '../../../../shared/models/product/product.model';
import { CategoryService } from '../../../parameters/services/category/category.service';

// Cards
import { ContainerCardFlexComponent } from '../../../../shared/components/cards/container-card-flex/container-card-flex.component';

// Angular Material
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';

import { CategoryNodeModel } from '../../../parameters/models/category/category.model';
import { ButtonComponent } from '../../../../shared/components/button/button.component';

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule,
    MatFormFieldModule, MatSelectModule, MatButtonModule, MatIconModule,
    MatProgressBarModule, MatChipsModule, MatDividerModule, MatPaginatorModule,
    ContainerCardFlexComponent, ButtonComponent
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

  // loading
  isLoadingProducts = false;
  isLoadingCategories = false;
  atLeaf = false;

  // paginación (front)
  pageIndex = 0;
  pageSize = 12;
  pageSizeOptions = [8, 12, 24, 48];

  // items visibles (slice)
  get pagedProducts(): ProductSelectModel[] {
    const start = this.pageIndex * this.pageSize;
    return this.products.slice(start, start + this.pageSize);
  }

  ngOnInit(): void {
    this.loadRootCategories();
    this.loadProductsHome();
  }

  // ---------- Productos ----------
  private loadProductsHome(): void {
    this.isLoadingProducts = true;
    this.productService.getAllHome().subscribe({
      next: items => {
        this.products = items;
        this.resetPaginator();
        this.isLoadingProducts = false;
      },
      error: err => { console.error(err); this.isLoadingProducts = false; }
    });
  }

  private loadProductsByCategory(categoryId: number): void {
    this.isLoadingProducts = true;
    this.productService.getByCategory(categoryId).subscribe({
      next: items => {
        this.products = items;
        this.resetPaginator();
        this.isLoadingProducts = false;
      },
      error: err => { console.error(err); this.isLoadingProducts = false; }
    });
  }

  // ---------- Categorías ----------
  private loadRootCategories(): void {
    this.isLoadingCategories = true;
    this.categoryService.getNodes(null).subscribe({
      next: nodes => {
        this.categories = nodes;
        this.atLeaf = nodes.length === 0;
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
        this.atLeaf = nodes.length === 0;
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

  onPageChange(e: PageEvent): void {
    this.pageIndex = e.pageIndex;
    this.pageSize  = e.pageSize;
    // no llamamos al servidor: solo re-slice en getter
  }

  private resetPaginator(): void {
    this.pageIndex = 0;
    // opcional: si quieres ajustar pageSize según cantidad
    // this.pageSize = Math.min(this.pageSize, Math.max(8, this.products.length));
  }

  private pushToBreadcrumb(id: number, name: string): void {
    const existsIdx = this.breadcrumb.findIndex(b => b.id === id);
    if (existsIdx >= 0) this.breadcrumb = this.breadcrumb.slice(0, existsIdx + 1);
    else this.breadcrumb.push({ id, name });
  }

  trackById = (_: number, item: { id: number }) => item.id;
}