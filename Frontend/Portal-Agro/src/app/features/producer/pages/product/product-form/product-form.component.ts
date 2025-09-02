// product-form.component.ts
import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  OnInit,
  Output,
  inject,
} from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
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
  ProductRegisterModel,
  ProductUpdateModel,
  ApiOk,
} from '../../../../../shared/models/product/product.model';
import { ProductService } from '../../../../../shared/services/product/product.service';
import { FarmService } from '../../../../../shared/services/farm/farm.service';
import { CategoryService } from '../../../../parameters/services/category/category.service';
import { FarmSelectModel } from '../../../../../shared/models/farm/farm.model';
import { CategorySelectModel } from '../../../../parameters/models/category/category.model';
import { ButtonComponent } from '../../../../../shared/components/button/button.component';
import Swal from 'sweetalert2';
import { ProductImageService } from '../../../../../shared/services/productImage/product-image.service';
import { catchError, finalize, of, take } from 'rxjs';

// ---- Validadores utilitarios (alineados con backend) ----
const notWhiteSpaceValidator = (label: string): ValidatorFn =>
  (c: AbstractControl): ValidationErrors | null =>
    (typeof c.value === 'string' && c.value.trim().length === 0)
      ? { whitespace: `${label} no puede estar en blanco.` } : null;

const positiveNumberValidator = (label: string): ValidatorFn =>
  (c: AbstractControl): ValidationErrors | null => {
    const n = Number(c.value);
    if (!Number.isFinite(n) || n <= 0) return { positive: `${label} debe ser mayor a 0.` };
    return null;
  };

const positiveIntValidator = (label: string): ValidatorFn =>
  (c: AbstractControl): ValidationErrors | null => {
    const n = Number(c.value);
    if (!Number.isInteger(n) || n <= 0) return { positiveInt: `Debe seleccionar ${label.toLowerCase()} válida.` };
    return null;
  };

// Lista con al menos N elementos
const arrayMinLen = (min: number): ValidatorFn =>
  (c: AbstractControl): ValidationErrors | null => {
    const v = c.value as number[] | null | undefined;
    return Array.isArray(v) && v.length >= min ? null : { arrayMinLen: { required: min, actual: (v?.length ?? 0) } };
  };

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
  private categoryService = inject(CategoryService);

  farms: FarmSelectModel[] = [];
  categories: CategorySelectModel[] = [];

  // Ya no emitimos el producto; basta el evento de éxito
  @Output() saved = new EventEmitter<void>();

  generalGroup!: FormGroup;
  detallesGroup!: FormGroup;

  isEdit = false;
  isLoading = false;
  isDragging = false;
  isDeletingImage = false;

  readonly MAX_IMAGES = 5;
  readonly MAX_FILE_SIZE_MB = 5;
  readonly MAX_FILE_SIZE_BYTES = this.MAX_FILE_SIZE_MB * 1024 * 1024;

  selectedFiles: File[] = [];
  imagesPreview: string[] = [];
  existingImages: ProductImageSelectModel[] = [];
  imagesToDelete: string[] = []; // publicId a borrar en UPDATE

  productId?: number;

  // === Helpers para el límite de imágenes ===
  get totalImages(): number {
    return this.selectedFiles.length + this.existingImages.length;
  }

  get canAddMore(): boolean {
    return this.totalImages < this.MAX_IMAGES;
  }

  get imagesLimitMsg(): string {
    return this.canAddMore
      ? `Puedes agregar hasta ${this.MAX_IMAGES - this.totalImages} imagen(es) más`
      : `Límite alcanzado: ${this.MAX_IMAGES} imágenes`;
  }

  ngOnInit(): void {
    this.initForms();
    this.loadCategories();
    this.loadFarm();

    // Cambia validaciones según modo (create/update)
    this.route.paramMap.subscribe((params) => {
      const idParam = params.get('id');

      if (idParam) {
        // ---- MODO EDICIÓN ----
        this.productId = Number(idParam);
        this.isEdit = true;

        // Reset estados
        this.resetForm();
        this.existingImages = [];
        this.imagesToDelete = [];
        this.selectedFiles = [];
        this.imagesPreview = [];

        // Ajustar límites de UPDATE: price ≤ 1,000,000; production ≤ 50
        const priceCtrl = this.generalGroup.get('price');
        priceCtrl?.clearValidators();
        priceCtrl?.addValidators([Validators.required, positiveNumberValidator('El precio'), Validators.max(1_000_000)]);
        priceCtrl?.updateValueAndValidity({ emitEvent: false });

        const prodCtrl = this.generalGroup.get('production');
        prodCtrl?.clearValidators();
        prodCtrl?.addValidators([Validators.required, Validators.maxLength(50), notWhiteSpaceValidator('El tipo de producción')]);
        prodCtrl?.updateValueAndValidity({ emitEvent: false });

        // Cargar el producto + imágenes
        this.loadProduct(this.productId);
      } else {
        // ---- MODO CREACIÓN ----
        this.productId = undefined;
        this.isEdit = false;

        // Reset estados
        this.resetForm();
        this.existingImages = [];
        this.imagesToDelete = [];
        this.selectedFiles = [];
        this.imagesPreview = [];

        // Asegurar límites de CREATE: price ≤ 100,000,000; production ≤ 150
        const priceCtrl = this.generalGroup.get('price');
        priceCtrl?.clearValidators();
        priceCtrl?.addValidators([Validators.required, positiveNumberValidator('El precio'), Validators.max(100_000_000)]);
        priceCtrl?.updateValueAndValidity({ emitEvent: false });

        const prodCtrl = this.generalGroup.get('production');
        prodCtrl?.clearValidators();
        prodCtrl?.addValidators([Validators.required, Validators.maxLength(150), notWhiteSpaceValidator('El tipo de producción')]);
        prodCtrl?.updateValueAndValidity({ emitEvent: false });
      }
    });
  }

  private initForms(): void {
    this.generalGroup = this.fb.group({
      name: ['', [
        Validators.required, Validators.minLength(2), Validators.maxLength(100),
        notWhiteSpaceValidator('El nombre')
      ]],
      description: ['', [
        Validators.required, Validators.minLength(5), Validators.maxLength(500),
        notWhiteSpaceValidator('La descripción')
      ]],
      // Por defecto (create); en ngOnInit se ajusta si es edición
      price: [null, [
        Validators.required,
        Validators.min(0), // ⬅️ bloquea negativos
        Validators.max(100_000_000),
        Validators.pattern(/^\d+$/)   // Solo números enteros
      ]],
      unit: ['', [
        Validators.required, Validators.maxLength(20), notWhiteSpaceValidator('La unidad')
      ]],
      // Por defecto (create); en ngOnInit se ajusta si es edición
      production: ['', [
        Validators.required, Validators.maxLength(150), notWhiteSpaceValidator('El tipo de producción')
      ]],
    });

    this.detallesGroup = this.fb.group({
      stock: [0, [
        Validators.required,
        Validators.pattern(/^[0-9]+$/), // solo números enteros positivos o 0
        Validators.max(100_000)         // límite superior
      ]],
      status: [true, [Validators.required]],
      categoryId: [null, [Validators.required, positiveIntValidator('Categoría')]],
      farmIds: new FormControl<number[]>([], {
        nonNullable: true,
        validators: [arrayMinLen(1)]
      }),
      shippingIncluded: [false],
    });

  }

  private loadProduct(id: number): void {
    this.isLoading = true;
    this.productSrv.getById(id).subscribe({
      next: (p) => {
        this.patchFromSelect(p);
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

    // Preferir p.farmIds; si el backend aún manda solo farmId, usarlo como array
    const farms = (p.farmIds && p.farmIds.length ? p.farmIds : (p.farmId ? [p.farmId] : []));

    this.detallesGroup.patchValue({
      stock: p.stock,
      status: p.status,
      categoryId: p.categoryId,
      farmIds: farms,
      shippingIncluded: p.shippingIncluded,
    });
  }

  loadFarm() {
    this.farmService.getByProducer().subscribe((data) => {
      this.farms = data ?? [];
    });
  }

  loadCategories() {
    this.categoryService.getAll().subscribe((data) => {
      this.categories = data ?? [];
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
    const remaining = this.MAX_IMAGES - this.totalImages;
    if (remaining <= 0) {
      alert(`Límite alcanzado: máximo ${this.MAX_IMAGES} imágenes`);
      return;
    }

    const newFiles: File[] = [];
    const errors: string[] = [];

    Array.from(files).some((f) => {
      if (!f.type.startsWith('image/')) {
        errors.push(`"${f.name}" no es una imagen`);
        return false;
      }
      if (f.size > this.MAX_FILE_SIZE_BYTES) {
        errors.push(`"${f.name}" excede ${this.MAX_FILE_SIZE_MB} MB`);
        return false;
      }
      if (newFiles.length >= remaining) {
        return true; // corta el bucle .some
      }
      newFiles.push(f);
      return false;
    });

    if (errors.length) alert(errors.join('\n'));

    this.selectedFiles.push(...newFiles);
    newFiles.forEach((file) => {
      const reader = new FileReader();
      reader.onload = (ev) => {
        if (ev.target?.result) this.imagesPreview.push(ev.target.result as string);
      };
      reader.readAsDataURL(file);
    });
  }

  removeImage(index: number, isExisting: boolean): void {
    if (this.isDeletingImage) return;

    if (isExisting) {
      const img = this.existingImages[index];
      if (!img?.publicId) {
        this.existingImages.splice(index, 1);
        return;
      }
      this.isDeletingImage = true;
      this.imageSrv.deleteImagesByPublicIds([img.publicId]).subscribe({
        next: () => {
          this.existingImages.splice(index, 1);
          // Alternativa: delegar borrado en PUT con imagesToDelete.push(img.publicId)
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
    if (this.isLoading) return;

    this.generalGroup.markAllAsTouched();
    this.detallesGroup.markAllAsTouched();
    if (this.generalGroup.invalid || this.detallesGroup.invalid) return;

    // Límite total y requisito de imagen en CREATE
    if (this.totalImages > this.MAX_IMAGES) {
      Swal.fire({ icon: 'warning', title: 'Límite de imágenes', text: `Máximo ${this.MAX_IMAGES} imágenes en total.` });
      return;
    }
    if (!this.isEdit && this.totalImages === 0) {
      Swal.fire({ icon: 'warning', title: 'Falta imagen', text: 'Debes agregar al menos una imagen para crear el producto.' });
      return;
    }

    this.isLoading = true;

    const g = this.generalGroup.value;
    const d = this.detallesGroup.value;

    const base = {
      name: (g.name ?? '').trim(),
      description: (g.description ?? '').trim(),
      price: Number(g.price),
      unit: (g.unit ?? '').trim(),
      production: (g.production ?? '').trim(),
      stock: Number(d.stock),
      status: Boolean(d.status),
      shippingIncluded : Boolean(d.shippingIncluded),
      categoryId: Number(d.categoryId),
      farmIds: (d.farmIds as number[]) ?? [],   // << NUEVO
    };

    const dtoUpdate: ProductUpdateModel = {
      id: this.productId!,
      ...base,
      images: this.selectedFiles.length ? this.selectedFiles : undefined,
      imagesToDelete: this.imagesToDelete.length ? this.imagesToDelete : undefined,
    };

    const dtoCreate: ProductRegisterModel = {
      ...base,
      images: this.selectedFiles.length ? this.selectedFiles : undefined,
    };

    Swal.fire({
      title: this.isEdit ? 'Actualizando producto...' : 'Creando producto...',
      text: 'Por favor espera',
      allowOutsideClick: false,
      didOpen: () => Swal.showLoading(),
    });

    const request$ = this.isEdit
      ? this.productSrv.update(dtoUpdate)
      : this.productSrv.create(dtoCreate);

    request$
      .pipe(
        take(1),
        catchError((err) => {
          const msg =
            err?.error?.message ||
            err?.message ||
            (this.isEdit
              ? 'No se pudo actualizar el producto.'
              : 'No se pudo registrar el producto.');
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: msg,
            confirmButtonText: 'Cerrar',
          });
          return of(null);
        }),
        finalize(() => (this.isLoading = false))
      )
      .subscribe((resp: ApiOk | null) => {
        if (!resp) return;
        const ok = (resp as any).isSuccess ?? (resp as any).IsSuccess;
        if (!ok) return;

        const msg = (resp as any).message ?? (resp as any).Message
          ?? (this.isEdit ? 'Producto actualizado.' : 'Producto creado.');

        Swal.fire({
          icon: 'success',
          title: this.isEdit ? '¡Actualizado!' : '¡Creado!',
          text: msg,
          confirmButtonText: 'Aceptar',
        }).then(() => {
          this.saved.emit(); // ya no emitimos el producto
          this.resetAfterSave();
          this.router.navigateByUrl('/account/producer/management/product');
        });
      });
  }

  cancel(): void {
    this.resetForm();
  }

  private resetAfterSave(): void {
    this.isLoading = false;
    this.resetForm();
  }

  private resetForm(): void {
    this.generalGroup.reset();
    this.detallesGroup.reset({ stock: 0, status: true, farmIds: [], shippingIncluded: false});
    this.selectedFiles = [];
    this.imagesPreview = [];
    this.existingImages = this.isEdit ? this.existingImages : [];
    this.imagesToDelete = [];
    
  }

  // ✅ Normaliza entradas en tiempo real:
  // - No permite espacios iniciales
  // - Fuerza primera letra en mayúscula
  onInputChange(event: Event, controlName: string): void {
    const input = event.target as HTMLInputElement | HTMLTextAreaElement;
    let value = input.value;

    // ❌ Quita espacios al inicio
    if (value.startsWith(' ')) {
      value = value.trimStart();
    }

    // 🔠 Fuerza primera letra en mayúscula
    if (value.length === 1) {
      value = value.toUpperCase();
    }

    // ✅ Actualiza el control sin disparar ciclos infinitos
    this.generalGroup.get(controlName)?.setValue(value, { emitEvent: false });
  }
}

