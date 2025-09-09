import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { FarmSelectModel } from '../../../../../shared/models/farm/farm.model';
import { ProductService } from '../../../../../shared/services/product/product.service';
import { CategorySelectModel } from '../../../../parameters/models/category/category.model';
import { CategoryService } from '../../../../parameters/services/category/category.service';
import { FarmService } from '../../../../../shared/services/farm/farm.service';
import { MatSelectModule } from "@angular/material/select";
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';


@Component({
  selector: 'app-product-create',
  imports: [ CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatSlideToggleModule,
    MatButtonModule],
  templateUrl: './product-create.component.html',
  styleUrl: './product-create.component.css'
})
export class ProductCreateComponent implements OnInit {
  private fb = inject(FormBuilder);
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  private farmService = inject(FarmService);
  private router = inject(Router);

  form!: FormGroup;

  categories: CategorySelectModel[] = [];
  farms: FarmSelectModel[] = [];

  selectedFiles: File[] = [];

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(500)]],
      price: [null, [Validators.required, Validators.min(0)]],
      unit: ['', [Validators.required, Validators.maxLength(20)]],
      production: ['', [Validators.required, Validators.maxLength(100)]],
      farmId: [null, [Validators.required]],
      stock: [0, [Validators.required, Validators.min(0)]],
      status: [true, [Validators.required]],
      categoryId: [null, [Validators.required]]
    });

    // Carga de selects
    this.categoryService.getAll().subscribe({
      next: (data) => this.categories = data ?? [],
      error: () => Swal.fire('Error', 'No se pudieron cargar las categorías.', 'error')
    });

    this.farmService.getAll().subscribe({
      next: (data) => this.farms = data ?? [],
      error: () => Swal.fire('Error', 'No se pudieron cargar las fincas.', 'error')
    });
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    const files = input.files;
    if (!files) return;

    const allowedTypes = ['image/png', 'image/jpeg', 'image/jpg', 'image/webp'];
    const maxFileSizeMB = 5;
    const maxFiles = 5;

    this.selectedFiles = [];

    if (files.length > maxFiles) {
      Swal.fire('Demasiados archivos', `Solo se permiten ${maxFiles} imágenes.`, 'warning');
      return;
    }

    for (const file of Array.from(files)) {
      if (!allowedTypes.includes(file.type)) {
        Swal.fire('Archivo inválido', `El archivo "${file.name}" no es una imagen válida.`, 'error');
        continue;
      }
      const fileSizeMB = file.size / (1024 * 1024);
      if (fileSizeMB > maxFileSizeMB) {
        Swal.fire('Archivo muy grande', `El archivo "${file.name}" supera ${maxFileSizeMB}MB.`, 'error');
        continue;
      }
      this.selectedFiles.push(file);
    }
  }

  // registerProduct() {
  //   if (this.form.invalid) {
  //     this.form.markAllAsTouched();
  //     return;
  //   }

  //   const v = this.form.value;

  //   // Alinear keys con el DTO del backend: ProductCreateDto
  //   const fd = new FormData();
  //   fd.append('Name', String(v.name));
  //   fd.append('Description', String(v.description));
  //   fd.append('Price', String(v.price));          // número a string
  //   fd.append('Unit', String(v.unit));
  //   fd.append('Production', String(v.production));
  //   fd.append('FarmId', String(v.farmId));        // número a string
  //   fd.append('Stock', String(v.stock));
  //   fd.append('Status', String(v.status));        // boolean a string
  //   fd.append('CategoryId', String(v.categoryId));

  //   // Imágenes (propiedad 'Images' del DTO)
  //   for (const file of this.selectedFiles) {
  //     fd.append('Images', file, file.name);
  //   }

  //   this.productService.create(fd).subscribe({
  //     next: (resp) => {
  //       if (resp) {
  //         Swal.fire({ icon: 'success', title: 'Producto creado', text: 'Registro exitoso.' });
  //         this.router.navigate(['/account/producer/management/product']); // Ajusta ruta de destino
  //       } else {
  //         Swal.fire({ icon: 'error', title: 'Error', text: resp ?? 'No se pudo crear el producto.' });
  //       }
  //     },
  //     error: (err) => {
  //       Swal.fire({ icon: 'error', title: 'Error', text: err?.message ?? 'Error inesperado.' });
  //     }
  //   });
  // }
  
}
