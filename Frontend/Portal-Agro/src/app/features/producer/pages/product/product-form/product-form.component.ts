import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  inject,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { MatStepperModule } from '@angular/material/stepper';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import {
  ProductSelectModel,
  ProductImageSelectModel,
} from '../../../../../shared/models/product/product.model';
import { ProductService } from '../../../../../shared/services/product/product.service';
import {
  ProductUpdateModel,
  ProductRegisterModel,
} from '../../../../products/Models/product.model';
import { ProductImageService } from '../../../../products/services/productImage/product-image.service';
import { FarmService } from '../../../../../shared/services/farm/farm.service';
import { CategoryService } from '../../../../parameters/services/category/category.service';
import { FarmSelectModel } from '../../../../../shared/models/farm/farm.model';
import { CategorySelectModel } from '../../../../parameters/models/category/category.model';
import { ButtonComponent } from '../../../../../shared/components/button/button.component';
import Swal from 'sweetalert2';

/** Modelos simples para selects */
export interface CategoryOption {
  id: number;
  name: string;
}
export interface FarmOption {
  id: number;
  name: string;
}

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatStepperModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
    ButtonComponent,
  ],
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.css'],
})
export class ProductFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private productSrv = inject(ProductService);
  private imageSrv = inject(ProductImageService);
  private farmService = inject(FarmService);
  farms: FarmSelectModel[] = [];
  private categoryService = inject(CategoryService);
  categories: CategorySelectModel[] = [];

  loadFarm() {
    this.farmService.getFarms().subscribe((dara) => {
      this.farms = dara;
    });
  }
  loadCategories() {
    this.categoryService.getAll().subscribe((data) => {
      this.categories = data;
    });
  }

  /** Modo edición si llega productId o initialData */
  // @Input() productId?: number;
  // @Input() initialData?: ProductSelectModel;

  /** Listas externas para selects */
  // @Input({ required: true }) categories: CategoryOption[] = [];
  // @Input({ required: true }) farms: FarmOption[] = [];

  /** Emite cuando se crea/actualiza */
  @Output() saved = new EventEmitter<ProductSelectModel>();

  // Step groups
  generalGroup!: FormGroup;
  detallesGroup!: FormGroup;

  // Estado UI
  isEdit = false;
  isLoading = false;
  isDragging = false;
  isDeletingImage = false;

  // Límites
  readonly MAX_IMAGES = 5;
  readonly MAX_FILE_SIZE_MB = 5;
  readonly MAX_FILE_SIZE_BYTES = this.MAX_FILE_SIZE_MB * 1024 * 1024;

  // Imágenes
  selectedFiles: File[] = [];
  imagesPreview: string[] = [];
  existingImages: ProductImageSelectModel[] = [];
  imagesToDelete: string[] = []; // publicId a borrar en UPDATE

  productId?: number;

  ngOnInit(): void {
    this.initForms();
    this.loadCategories();
    this.loadFarm();

    // Escucha cambios del :id (soporta navegar de update/5 a update/6 sin destruir componente)
    this.route.paramMap.subscribe((params) => {
      const idParam = params.get('id');

      if (idParam) {
        // ---- MODO EDICIÓN ----
        this.productId = Number(idParam);
        this.isEdit = true;

        // Limpia estados previos por si venías de 'create' o de otro 'id'
        this.resetForm();
        this.existingImages = [];
        this.imagesToDelete = [];
        this.selectedFiles = [];
        this.imagesPreview = [];

        // Carga el producto y sus imágenes
        this.loadProduct(this.productId);
      } else {
        // ---- MODO CREACIÓN ----
        this.productId = undefined;
        this.isEdit = false;

        // Deja el form listo para crear
        this.resetForm();
        this.existingImages = [];
        this.imagesToDelete = [];
        this.selectedFiles = [];
        this.imagesPreview = [];
      }
    });
  }

  private initForms(): void {
    this.generalGroup = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(120)]],
      description: ['', [Validators.required, Validators.maxLength(1000)]],
      price: [0, [Validators.required, Validators.min(0)]],
      unit: ['', [Validators.required, Validators.maxLength(50)]],
      production: ['', [Validators.required, Validators.maxLength(100)]],
    });

    this.detallesGroup = this.fb.group({
      stock: [0, [Validators.required, Validators.min(0)]],
      status: [true, [Validators.required]],
      categoryId: [null, [Validators.required]],
      farmId: [null, [Validators.required]],
    });
  }

  private loadProduct(id: number): void {
    this.isLoading = true;
    this.productSrv.getById(id).subscribe({
      next: (p) => {
        this.patchFromSelect(p);
        // Cargar imágenes existentes del producto (si tu select ya trae publicId y url, úsalo directo)
        this.imageSrv.getImagesByProductId(p.id).subscribe({
          next: (imgs) => (this.existingImages = imgs ?? []),
          complete: () => (this.isLoading = false),
          error: () => (this.isLoading = false),
        });
      },
      error: () => (this.isLoading = false),
    });
  }

  private patchFromSelect(p: ProductSelectModel): void {
    this.generalGroup.patchValue({
      name: p.name,
      description: p.description,
      price: p.price,
      unit: p.unit,
      production: p.production,
    });

    this.detallesGroup.patchValue({
      stock: p.stock,
      status: p.status,
      categoryId: p.categoryId,
      farmId: p.farmId,
    });
  }

  // Drag & Drop
  onDragOver(e: DragEvent): void {
    e.preventDefault();
    e.stopPropagation();
    this.isDragging = true;
  }

  onDragLeave(e: DragEvent): void {
    e.preventDefault();
    e.stopPropagation();
    this.isDragging = false;
  }

  onDropOrInput(files: FileList | null): void {
    this.isDragging = false;
    if (!files?.length) return;
    this.processFiles(files);
  }

  private processFiles(files: FileList): void {
    const total = this.selectedFiles.length + this.existingImages.length;
    const remaining = this.MAX_IMAGES - total;
    if (remaining <= 0) {
      alert(`Máximo ${this.MAX_IMAGES} imágenes permitidas`);
      return;
    }

    const newFiles: File[] = [];
    const errors: string[] = [];

    Array.from(files).forEach((f) => {
      if (!f.type.startsWith('image/')) {
        errors.push(`"${f.name}" no es una imagen`);
      } else if (f.size > this.MAX_FILE_SIZE_BYTES) {
        errors.push(`"${f.name}" excede ${this.MAX_FILE_SIZE_MB} MB`);
      } else if (newFiles.length < remaining) {
        newFiles.push(f);
      }
    });

    if (errors.length) alert(errors.join('\n'));

    this.selectedFiles.push(...newFiles);
    newFiles.forEach((file) => {
      const reader = new FileReader();
      reader.onload = (ev) => {
        if (ev.target?.result)
          this.imagesPreview.push(ev.target.result as string);
      };
      reader.readAsDataURL(file);
    });
  }

  removeImage(index: number, isExisting: boolean): void {
    if (this.isDeletingImage) return;

    if (isExisting) {
      const img = this.existingImages[index];
      if (!img?.publicId) {
        // Si tu modelo no trae publicId, adapta el servicio/DTO
        this.existingImages.splice(index, 1);
        return;
      }
      this.isDeletingImage = true;
      this.imageSrv.deleteImagesByPublicIds([img.publicId]).subscribe({
        next: () => {
          this.existingImages.splice(index, 1);
          // Registrar para el update por si prefieres delegar borrado en PUT
          // this.imagesToDelete.push(img.publicId);
        },
        complete: () => (this.isDeletingImage = false),
        error: () => (this.isDeletingImage = false),
      });
    } else {
      this.selectedFiles.splice(index, 1);
      this.imagesPreview.splice(index, 1);
    }
  }

  // Submit
  submit(): void {
    if (this.generalGroup.invalid || this.detallesGroup.invalid) {
      this.generalGroup.markAllAsTouched();
      this.detallesGroup.markAllAsTouched();
      return;
    }

    if (!this.isEdit && this.selectedFiles.length === 0) {
      alert('Debes agregar al menos una imagen para crear el producto');
      return;
    }

    this.isLoading = true;
    const g = this.generalGroup.value;
    const d = this.detallesGroup.value;

    if (this.isEdit) {
      const dto: ProductUpdateModel = {
        id: this.productId!,
        name: g.name,
        description: g.description,
        price: Number(g.price),
        unit: g.unit,
        production: g.production,
        stock: Number(d.stock),
        status: Boolean(d.status),
        categoryId: Number(d.categoryId),
        farmId: Number(d.farmId),
        images: this.selectedFiles.length ? this.selectedFiles : undefined,
        imagesToDelete: this.imagesToDelete.length
          ? this.imagesToDelete
          : undefined,
      };

      this.productSrv.update(dto).subscribe({
        next: (resp) => {
          Swal.fire({
            icon: 'success',
            title: '¡Actualizado!',
            text: 'El producto se actualizó con éxito',
            confirmButtonText: 'Aceptar',
          }).then(() => {
            this.saved.emit(resp);
            this.resetAfterSave();
            // Redirigir al dashboard (ajusta la ruta real)
            this.router.navigateByUrl('/account/producer/management/product');
          });
        },
        error: (err) => {
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo actualizar el producto',
            confirmButtonText: 'Cerrar',
          });
          this.isLoading = false;
        },
      });
    } else {
      const dto: ProductRegisterModel = {
        name: g.name,
        description: g.description,
        price: Number(g.price),
        unit: g.unit,
        production: g.production,
        stock: Number(d.stock),
        status: Boolean(d.status),
        categoryId: Number(d.categoryId),
        farmId: Number(d.farmId),
        images: this.selectedFiles.length ? this.selectedFiles : undefined,
      };

      this.productSrv.create(dto).subscribe({
        next: (resp) => {
          Swal.fire({
            icon: 'success',
            title: '¡Creado!',
            text: 'El producto se registró con éxito',
            confirmButtonText: 'Aceptar',
          }).then(() => {
            this.saved.emit(resp);
            this.resetAfterSave();
            // Redirigir al dashboard (ajusta la ruta real)
            this.router.navigateByUrl('/account/producer/management/product');
          });
        },
        error: (err) => {
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo registrar el producto',
            confirmButtonText: 'Cerrar',
          });
          this.isLoading = false;
        },
      });
    }
  }

  cancel(): void {
    // Si quieres navegar:
    // this.router.navigate(['/account/producer/management/product']);
    // O simplemente limpiar:
    this.resetForm();
  }

  private resetAfterSave(): void {
    this.isLoading = false;
    this.resetForm();
  }

  private resetForm(): void {
    this.generalGroup.reset({ price: 0 });
    this.detallesGroup.reset({ stock: 0, status: true });
    this.selectedFiles = [];
    this.imagesPreview = [];
    this.existingImages = this.isEdit ? this.existingImages : [];
    this.imagesToDelete = [];
  }
}
