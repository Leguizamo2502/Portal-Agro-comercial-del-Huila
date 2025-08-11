import { Component, inject, OnInit } from '@angular/core';
import { CategoryService } from '../../../services/category/category.service';
import { Router } from '@angular/router';
import { CategorySelectModel } from '../../../models/category/category.model';
import { TableComponent } from "../../../../../shared/components/table/table.component";

@Component({
  selector: 'app-category-list',
  imports: [TableComponent],
  templateUrl: './category-list.component.html',
  styleUrl: './category-list.component.css'
})
export class CategoryListComponent implements OnInit{
  categoryService = inject(CategoryService);
  categorys: CategorySelectModel[] = [];
  router = inject(Router);
  // route = inject(ActivatedRoute);

  ngOnInit(): void {
    this.loadForm();
  }

  columns = [
    { key: 'name', label: 'Nombre' },
    { key: 'parentName', label: 'Categoria Padre' },
  ];

  onEdit(item: any) {
    const id = item.id;
    this.router.navigate(['/account/security/category/update', id]);

  }

  onDelete(item: any) {
    this.categoryService.deleteLogic(item.id).subscribe(() => {
      this.loadForm();
    });
  }

  loadForm() {
    this.categoryService.getAll().subscribe((data) => {
      this.categorys = data;
      console.log(data);
    });
  }
}
