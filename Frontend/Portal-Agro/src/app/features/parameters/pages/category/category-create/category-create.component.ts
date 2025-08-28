import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { CategoryFormComponent } from "../category-form/category-form.component";
import { CategoryService } from '../../../services/category/category.service';
import { CategoryRegistertModel, CategorySelectModel } from '../../../models/category/category.model';

// lo que emite el form (sin id)
type CategoryPayload = Omit<CategoryRegistertModel, 'id'>;

@Component({
  selector: 'app-category-create',
  imports: [CategoryFormComponent],
  templateUrl: './category-create.component.html',
  styleUrl: './category-create.component.css'
})
export class CategoryCreateComponent implements OnInit {
  private router = inject(Router);
  private formService = inject(CategoryService);

  parentList: CategorySelectModel[] = [];

  ngOnInit(): void {
    this.formService.getAll().subscribe(data => {
      this.parentList = data;
    });
  }

  // <<-- recibe payload sin id desde el form
  saveChange(payload: CategoryPayload) {
    // el service espera CategoryRegistertModel con id -> mandamos id=0
    const body: CategoryRegistertModel = { id: 0, ...payload };

    this.formService.create(body).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Categoria creada',
          text: 'La categoria se ha guardado correctamente',
          confirmButtonText: 'Aceptar',
        }).then(() => this.router.navigate(['/account/parameters/category']));
      },
      error: (error) => {
        Swal.fire({ icon: 'error', title: 'Error', text: 'No se pudo guardar la categoria.' });
        console.error(error);
      },
    });
  }
}
