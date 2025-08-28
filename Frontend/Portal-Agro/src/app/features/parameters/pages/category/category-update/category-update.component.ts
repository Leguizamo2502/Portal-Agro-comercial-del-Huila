import { Component, inject, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { CategoryFormComponent } from "../category-form/category-form.component";
import { CategoryService } from '../../../services/category/category.service';
import { CategoryRegistertModel, CategorySelectModel } from '../../../models/category/category.model';

type CategoryPayload = Omit<CategoryRegistertModel, 'id'>;

@Component({
  selector: 'app-category-update',
  imports: [CategoryFormComponent],
  templateUrl: './category-update.component.html',
  styleUrl: './category-update.component.css'
})
export class CategoryUpdateComponent implements OnInit {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private formService = inject(CategoryService);

  id!: number;
  model?: CategorySelectModel;
  parentList: CategorySelectModel[] = [];

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.id) return;

    this.formService.getAll().subscribe(data => {
      // evita ofrecerse a sí misma como padre
      this.parentList = data.filter(c => c.id !== this.id);
    });

    this.formService.getById(this.id).subscribe(m => (this.model = m));
  }

  // <<-- recibe payload sin id desde el form
  save(payload: CategoryPayload) {
    const body: CategoryRegistertModel = { id: this.id, ...payload };

    this.formService.update(this.id, body).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Categoria Actualizada',
          text: 'La categoria se ha actualizado correctamente',
          confirmButtonText: 'Aceptar',
        }).then(() => this.router.navigate(['/account/parameters/category']));
      },
      error: (error) => {
        Swal.fire({ icon: 'error', title: 'Error', text: 'No se pudo actualizar la categoria.' });
        console.error(error);
      },
    });
  }
}
