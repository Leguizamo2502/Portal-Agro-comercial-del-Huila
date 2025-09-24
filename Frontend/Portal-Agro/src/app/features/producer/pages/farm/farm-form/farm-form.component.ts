import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  inject,
  Input,
  NgZone,
  OnDestroy,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
  AbstractControl,
  ValidationErrors,
  ValidatorFn,
  FormArray,
} from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import {
  FarmSelectModel,
  FarmImageSelectModel,
  FarmUpdateModel,
  FarmWithProducerRegisterModel,
  FarmRegisterModel,
} from '../../../../../shared/models/farm/farm.model';
import {
  DepartmentModel,
  CityModel,
} from '../../../../../shared/models/location/location.model';
import { FarmService } from '../../../../../shared/services/farm/farm.service';
import { LocationService } from '../../../../../shared/services/location/location.service';

// Leaflet
import * as L from 'leaflet';
// import 'leaflet/dist/leaflet.css';
import { ButtonComponent } from '../../../../../shared/components/button/button.component';
import { MatIconModule } from '@angular/material/icon';
import { MatStepperModule } from '@angular/material/stepper';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { catchError, finalize, of, take } from 'rxjs';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { AuthState } from '../../../../../Core/services/auth/auth.state';
import {
  SocialNetwork,
  ProducerSocialCreateModel,
} from '../../../../../shared/models/producer/producer.model';


//
// ---- Validadores utilitarios (alineados con FluentValidation del backend) ----
//
const positiveNumberValidator =
  (label: string): ValidatorFn =>
  (c: AbstractControl): ValidationErrors | null => {
    const n = Number(c.value);
    if (!Number.isFinite(n) || n <= 0)
      return { positive: `${label} debe ser mayor a 0.` };
    return null;
  };

const rangeValidator =
  (min: number, max: number, label: string): ValidatorFn =>
  (c: AbstractControl): ValidationErrors | null => {
    const n = Number(c.value);
    if (!Number.isFinite(n)) return { required: `${label} es obligatorio.` };
    if (n < min || n > max)
      return { range: `${label} debe estar entre ${min} y ${max}.` };
    return null;
  };

const positiveIntValidator =
  (label: string): ValidatorFn =>
  (c: AbstractControl): ValidationErrors | null => {
    const n = Number(c.value);
    if (!Number.isInteger(n) || n <= 0)
      return { positiveInt: `Debe seleccionar ${label.toLowerCase()} válida.` };
    return null;
  };

@Component({
  selector: 'app-farm-form',
  imports: [
    ButtonComponent,
    MatIconModule,
    MatStepperModule,
    MatInputModule,
    MatSelectModule,
    ReactiveFormsModule,
    CommonModule,
    MatFormFieldModule,
    MatButtonModule,
    MatTooltipModule,
  ],
  templateUrl: './farm-form.component.html',
  standalone: true,
  styleUrls: ['./farm-form.component.css'],
})
export class FarmFormComponent implements OnInit, OnDestroy {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private farmSrv = inject(FarmService);
  private locationSrv = inject(LocationService);
  private zone = inject(NgZone);
  private authState = inject(AuthState);

  SocialNetwork = SocialNetwork;

  get socialLinksFA(): FormArray<FormGroup> {
    return this.generalGroup.get('socialLinks') as FormArray<FormGroup>;
  }

  get socialLinksControls(): FormGroup[] {
    return this.socialLinksFA.controls as FormGroup[];
  }

  /** Si es true usa createWithProducer (requiere descripción); si es false usa create */
  @Input() createWithProducer = false;

  /** Emite cuando se crea/actualiza */
  @Output() saved = new EventEmitter<FarmSelectModel>();

  // Step groups
  generalGroup!: FormGroup;
  ubicacionGroup!: FormGroup;

  // Estado UI
  isEdit = false;
  isLoading = false;
  isDragging = false;
  isDeletingImage = false;

  // Límites
  readonly MAX_IMAGES = 5;
  readonly MAX_FILE_SIZE_MB = 5; // Si decides alinear a backend con 2MB, cambia aquí y en backend.
  readonly MAX_FILE_SIZE_BYTES = this.MAX_FILE_SIZE_MB * 1024 * 1024;

  // Imágenes
  selectedFiles: File[] = [];
  imagesPreview: string[] = [];
  existingImages: FarmImageSelectModel[] = [];
  imagesToDelete: string[] = []; // si decides delegar borrado en PUT

  // Ubicación
  departments: DepartmentModel[] = [];
  cities: CityModel[] = [];

  // Mapa
  @ViewChild('mapContainer', { static: false })
  mapContainer?: ElementRef<HTMLDivElement>;
  private map?: L.Map;
  private marker?: L.Marker;
  // Centro por defecto (Huila aprox.)
  private defaultCenter: [number, number] = [2.9386, -75.2519];
  private defaultZoom = 8;

  farmId?: number;

  onStepChange(ev: StepperSelectionEvent) {
    // índice 1 = segundo paso (Ubicación)
    if (ev.selectedIndex === 1) {
      this.zone.runOutsideAngular(() => {
        requestAnimationFrame(() => this.initMap());
      });
    }
  }

  ngOnInit(): void {
    this.initForms();

    // Validación condicional de 'description'
    const desc = this.generalGroup.get('description');
    desc?.clearValidators();
    if (this.createWithProducer) {
      // Backend: 5–500 obligatoria en ProducerWithFarmRegisterDto
      desc?.addValidators([
        Validators.required,
        Validators.minLength(5),
        Validators.maxLength(500),
      ]);
    } else {
      // No obligatoria; límite razonable
      desc?.addValidators([Validators.maxLength(500)]);
    }
    desc?.updateValueAndValidity({ emitEvent: false });

    this.loadDepartments();

    // Modo edición: lee el id una sola vez
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.farmId = Number(idParam);
      this.isEdit = true;
      this.resetBeforeLoad();
      this.loadFarm(this.farmId);
    } else {
      this.farmId = undefined;
      this.isEdit = false;
      this.resetBeforeLoad();
    }
  }

  ngOnDestroy(): void {
    this.map?.remove();
  }

  /* ============================ INIT FORMS ============================ */
  private initForms(): void {
    this.generalGroup = this.fb.group({
      // Backend: Name Length(2,100)
      name: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(100),
        ],
      ],
      // Backend: Hectares > 0
      hectares: [
        null,
        [Validators.required, positiveNumberValidator('Las hectáreas')],
      ],
      // Backend: Altitude >= 0 && <= 9000
      altitude: [
        null,
        [Validators.required, Validators.min(0), Validators.max(9000)],
      ],
      // Solo si createWithProducer
      description: [''],

      //REDES
      socialLinks: this.fb.array<FormGroup>([]),
    });

    this.ubicacionGroup = this.fb.group({
      departmentId: [null, [Validators.required]],
      // Backend: CityId > 0
      cityId: [null, [Validators.required, positiveIntValidator('Ciudad')]],
      // Backend: Latitude [-90, 90]
      latitude: [
        null,
        [Validators.required, rangeValidator(-90, 90, 'Latitud')],
      ],
      // Backend: Longitude [-180, 180]
      longitude: [
        null,
        [Validators.required, rangeValidator(-180, 180, 'Longitud')],
      ],
    });
  }

  // Agregar un item (opcional)
  addSocialLink(): void {
    const fg = this.fb.group({
      network: [SocialNetwork.Instagram, [Validators.required]],
      url: ['', [Validators.required, Validators.maxLength(512)]],
    });
    this.socialLinksFA.push(fg);
  }

  // Quitar item
  removeSocialLink(i: number): void {
    this.socialLinksFA.removeAt(i);
  }

  /* ============================ LOAD DATA ============================ */
  private loadFarm(id: number): void {
    this.isLoading = true;
    this.farmSrv
      .getById(id)
      .pipe(take(1))
      .subscribe({
        next: (f) => {
          if (!f) return;
          this.patchFromSelect(f);

          this.existingImages = f.images ?? [];

          const lat = Number(this.ubicacionGroup.value.latitude);
          const lng = Number(this.ubicacionGroup.value.longitude);
          const safeLat = Number.isFinite(lat) ? lat : this.defaultCenter[0];
          const safeLng = Number.isFinite(lng) ? lng : this.defaultCenter[1];
          this.setMarker(safeLat, safeLng, true);
        },
        error: (err) => {
          console.error('getById error', err);
          this.isLoading = false;
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo cargar la finca',
          });
        },
        complete: () => (this.isLoading = false),
      });
  }

  private patchFromSelect(f: FarmSelectModel): void {
    this.generalGroup.patchValue({
      name: f.name,
      hectares: f.hectares ?? 0,
      altitude: f.altitude ?? 0,
      description: '',
    });

    this.ubicacionGroup.patchValue({
      departmentId: f.departmentId ?? null,
      cityId: f.cityId ?? null,
      latitude: Number(f.latitude),
      longitude: Number(f.longitude),
    });

    // Cargar ciudades según departmentId y fijar cityId
    if (f.departmentId) {
      this.onDepartmentChange(f.departmentId, f.cityId);
    }
  }

  private resetBeforeLoad(): void {
    this.generalGroup.reset({ hectares: 0, altitude: 0, description: '' });
    this.ubicacionGroup.reset({
      departmentId: null,
      cityId: null,
      latitude: null,
      longitude: null,
    });
    this.selectedFiles = [];
    this.imagesPreview = [];
    this.existingImages = this.isEdit ? this.existingImages : [];
    this.imagesToDelete = [];
    // Reset marker
    this.setMarker(
      this.defaultCenter[0] as number,
      this.defaultCenter[1] as number,
      true
    );
  }

  private loadDepartments(): void {
    this.locationSrv.getDepartment().subscribe({
      next: (deps) => (this.departments = deps),
    });
  }

  onDepartmentChange(depId: number, presetCityId?: number): void {
    this.ubicacionGroup.patchValue({ cityId: null });
    this.cities = [];
    if (!depId) return;

    this.locationSrv.getCity(depId).subscribe({
      next: (cities) => {
        this.cities = cities;
        if (presetCityId && cities.some((c) => c.id === presetCityId)) {
          this.ubicacionGroup.patchValue({ cityId: presetCityId });
        }
      },
    });
  }

  /* ============================ MAPA ============================ */
  private initMap(): void {
    if (!this.mapContainer) return;

    // si ya existe, solo revalida tamaño y mueve marcador
    if (this.map) {
      this.map.invalidateSize(); // importante al abrir el step
      const lat =
        Number(this.ubicacionGroup.value.latitude) || this.defaultCenter[0];
      const lng =
        Number(this.ubicacionGroup.value.longitude) || this.defaultCenter[1];
      this.setMarker(lat, lng, true);
      return;
    }

    
    L.Icon.Default.mergeOptions({
      iconRetinaUrl: 'leaflet/marker-icon-2x.png',
      iconUrl: 'leaflet/marker-icon.png',
      shadowUrl: 'leaflet/marker-shadow.png',
    });

    this.map = L.map(this.mapContainer.nativeElement, {
      center: this.defaultCenter,
      zoom: this.defaultZoom,
    });

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors',
      maxZoom: 19,
    }).addTo(this.map);

    this.map.on('click', (e: L.LeafletMouseEvent) => {
      const { lat, lng } = e.latlng;
      this.setMarker(lat, lng, true);
      this.ubicacionGroup.patchValue({ latitude: lat, longitude: lng });
    });

    const lat =
      Number(this.ubicacionGroup.value.latitude) || this.defaultCenter[0];
    const lng =
      Number(this.ubicacionGroup.value.longitude) || this.defaultCenter[1];
    this.setMarker(lat, lng, false);

    // asegura tamaño correcto tras render
    setTimeout(() => this.map!.invalidateSize(), 0);
  }

  private setMarker(lat: number, lng: number, pan = false): void {
    if (!this.map) return;

    if (!this.marker) {
      this.marker = L.marker([lat, lng], { draggable: true }).addTo(this.map);
      this.marker.on('dragend', () => {
        const pos = this.marker!.getLatLng();
        this.ubicacionGroup.patchValue({
          latitude: pos.lat,
          longitude: pos.lng,
        });
      });
    } else {
      this.marker.setLatLng([lat, lng]);
    }

    if (pan) {
      this.map.setView([lat, lng], this.map.getZoom(), { animate: true });
    }
  }

  // Para cuando el usuario edita manualmente lat/lng en inputs (opcional)
  onLatLngManualChange(): void {
    const lat = Number(this.ubicacionGroup.value.latitude);
    const lng = Number(this.ubicacionGroup.value.longitude);
    if (isFinite(lat) && isFinite(lng)) {
      this.setMarker(lat, lng, true);
    }
  }

  /* ============================ DRAG & DROP ============================ */
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
        this.existingImages.splice(index, 1);
        return;
      }
      // Delegar al PUT usando imagesToDelete
      this.imagesToDelete.push(img.publicId);
      this.existingImages.splice(index, 1);
    } else {
      this.selectedFiles.splice(index, 1);
      this.imagesPreview.splice(index, 1);
    }
  }

  /* ============================ SUBMIT ============================ */
  submit(): void {
    // Bloquea doble envío
    if (this.isLoading) return;

    // Fuerza visualización de errores
    this.generalGroup.markAllAsTouched();
    this.ubicacionGroup.markAllAsTouched();

    if (this.generalGroup.invalid || this.ubicacionGroup.invalid) return;

    // Reglas adicionales (backend: create requiere al menos 1 imagen)
    if (!this.isEdit && this.selectedFiles.length === 0) {
      Swal.fire({
        icon: 'warning',
        title: 'Falta imagen',
        text: 'Debes agregar al menos una imagen para crear la finca.',
        confirmButtonText: 'Entendido',
      });
      return;
    }

    const g = this.generalGroup.value;
    const u = this.ubicacionGroup.value;

    // Mapea socialLinks solo si aplica y hay datos válidos
    let socialLinks: ProducerSocialCreateModel[] | undefined = undefined;
    if (this.createWithProducer && this.socialLinksFA.length > 0) {
      const items = this.socialLinksFA.controls
        .map((c) => c.value as ProducerSocialCreateModel)
        // descarta filas medio vacías
        .filter((x) => x && x.url && String(x.url).trim().length > 0);

      if (items.length > 0) socialLinks = items;
    }

    const name = (g.name ?? '').trim();
    const description = (g.description ?? '').trim();
    const hectares = Number(g.hectares);
    const altitude = Number(g.altitude);
    const latitude = Number(u.latitude);
    const longitude = Number(u.longitude);
    const cityId = Number(u.cityId);

    this.isLoading = true;

    // Loading con SweetAlert2
    Swal.fire({
      title: this.isEdit
        ? 'Actualizando finca...'
        : this.createWithProducer
        ? 'Creando productor y finca...'
        : 'Creando finca...',
      text: 'Por favor espera',
      allowOutsideClick: false,
      didOpen: () => Swal.showLoading(),
    });

    // DTOs
    const dtoUpdate: FarmUpdateModel = {
      id: this.farmId!,
      name,
      hectares,
      altitude,
      latitude,
      longitude,
      cityId,
      images: this.selectedFiles.length ? this.selectedFiles : undefined,
      imagesToDelete: this.imagesToDelete.length
        ? this.imagesToDelete
        : undefined,
    };

    const dtoCreateWithProducer: FarmWithProducerRegisterModel = {
      name,
      description,
      hectares,
      altitude,
      latitude,
      longitude,
      images: this.selectedFiles,
      cityId,
      ...(socialLinks ? { socialLinks } : {}),
    };

    const dtoCreate: FarmRegisterModel = {
      name,
      hectares,
      altitude,
      latitude,
      longitude,
      images: this.selectedFiles,
      cityId,
      // Si el backend exige ProducerId > 0 en FarmRegisterDto, agrega aquí producerId desde tu AuthState:
      // producerId: this.authState.me?.producerId
    };

    // Selección de request según modo
    const request$ = this.isEdit
      ? this.farmSrv.update(dtoUpdate)
      : this.createWithProducer
      ? this.farmSrv.createWithProducer(dtoCreateWithProducer)
      : this.farmSrv.create(dtoCreate);

    request$
      .pipe(
        take(1),
        catchError((err) => {
          const msg =
            err?.error?.message ||
            err?.message ||
            (this.isEdit
              ? 'No se pudo actualizar la finca.'
              : this.createWithProducer
              ? 'No se pudo crear productor + finca.'
              : 'No se pudo registrar la finca.');
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
      .subscribe(async (resp) => {
        if (!resp) return; // error ya informado

        if (!this.isEdit && this.createWithProducer) {
          await Swal.fire({
            icon: 'success',
            title: '¡Creado!',
            text: 'Productor y finca creados',
            confirmButtonText: 'Aceptar',
          });

          try {
            await this.authState.reloadMeOnce();
          } catch {
            console.warn(
              'No se pudo recargar el perfil (me) tras la creación.'
            );
          }
        } else {
          await Swal.fire({
            icon: 'success',
            title: this.isEdit ? '¡Actualizada!' : '¡Creada!',
            text: this.isEdit
              ? 'La finca se actualizó con éxito'
              : 'La finca se registró con éxito',
            confirmButtonText: 'Aceptar',
          });
        }

        this.saved.emit(resp);
        this.resetAfterSave();
        this.router.navigateByUrl('/account/producer/management/farm');
      });
  }

  cancel(): void {
    this.resetBeforeLoad();
  }

  private resetAfterSave(): void {
    this.isLoading = false;
    this.resetBeforeLoad();
  }
}
