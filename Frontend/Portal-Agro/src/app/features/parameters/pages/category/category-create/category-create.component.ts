import { Component, inject, OnInit } from '@angular/core';
import { CategoryService } from '../../../services/category/category.service';
import { CategoryRegistertModel, CategorySelectModel } from '../../../models/category/category.model';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';
import { CategoryFormComponent } from "../category-form/category-form.component";

@Component({
  selector: 'app-category-create',
  imports: [CategoryFormComponent],
  templateUrl: './category-create.component.html',
  styleUrl: './category-create.component.css'
})
export class CategoryCreateComponent implements OnInit{
  router = inject(Router);
  
  formService = inject(CategoryService);
  parentList: CategorySelectModel[] = [];
  ngOnInit(): void {
    this.formService.getAll().subscribe((data)=>{
      this.parentList = data;
    })
  }


  saveChange(form: CategoryRegistertModel) {
    this.formService.create(form).subscribe({
      next: () => {
        console.log(form)
        Swal.fire({
          icon: 'success',
          title: 'Categoria creada',
          text: 'la categoria se ha guardado correctamente',
          confirmButtonText: 'Aceptar',
        }).then(() => {
          this.router.navigate(['/account/parameters/category']);
        });
        
        console.log(form);
      },
      error: (error) => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo guardar la categoria.',
        });

        console.error(error);
      },
    });
  }
}
