import { Component, inject, OnInit } from '@angular/core';
import { CategoryService } from '../../../services/category/category.service';
import {
  CategoryRegistertModel,
  CategorySelectModel,
} from '../../../models/category/category.model';
import Swal from 'sweetalert2';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryFormComponent } from '../category-form/category-form.component';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-category-update',
  imports: [CategoryFormComponent],
  templateUrl: './category-update.component.html',
  styleUrl: './category-update.component.css',
})
export class CategoryUpdateComponent implements OnInit {
  categoryService = inject(CategoryService);
  router = inject(Router);
  route = inject(ActivatedRoute);

  parentList: CategorySelectModel[] = [];

  id!: number;
  model?: CategorySelectModel;

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.id) return;

    forkJoin({
      parents: this.categoryService.getAll(),
      model: this.categoryService.getById(this.id),
    }).subscribe({
      next: ({ parents, model }) => {
        this.parentList = parents.filter((p) => p.id !== this.id);

        const parentId: number | null =
          model.parentCategoryId == null
            ? null
            : Number(model.parentCategoryId);

        this.model = { ...model, parentCategoryId: parentId }; // ahora cuadra con el tipo
      },
    });
  }

  loadCategories() {
    this.categoryService.getAll().subscribe((data) => {
      this.parentList = data;
    });
  }

  save(category: CategoryRegistertModel) {
    this.categoryService.update(this.id, category).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Categoria Actualizado',
          text: 'la Categoria se ha actualiazo correctamente',
          confirmButtonText: 'Aceptar',
        }).then(() => {
          this.router.navigate(['/account/parameters/category']);
        });

        console.log(category);
      },
      error: (error) => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo actualizar la Categoria.',
        });

        console.error(error);
      },
    });
  }
}
