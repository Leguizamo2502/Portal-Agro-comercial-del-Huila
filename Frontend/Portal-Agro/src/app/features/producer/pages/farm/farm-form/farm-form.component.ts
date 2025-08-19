import { AfterViewInit, Component, ElementRef, EventEmitter, inject, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { FarmSelectModel, FarmImageSelectModel, FarmUpdateModel, FarmWithProducerRegisterModel, FarmRegisterModel } from '../../../../../shared/models/farm/farm.model';
import { DepartmentModel, CityModel } from '../../../../../shared/models/location/location.model';
import { FarmService } from '../../../../../shared/services/farm/farm.service';
import { LocationService } from '../../../../../shared/services/location/location.service';

// Leaflet
import * as L from 'leaflet';
import { ButtonComponent } from "../../../../../shared/components/button/button.component";
import { MatIconModule } from "@angular/material/icon";
import { MatStepperModule } from "@angular/material/stepper";
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-farm-form',
  imports: [ButtonComponent, MatIconModule, MatStepperModule, MatInputModule, MatSelectModule,ReactiveFormsModule,CommonModule],
  templateUrl: './farm-form.component.html',
  styleUrl: './farm-form.component.css'
})
export class FarmFormComponent implements OnInit, AfterViewInit, OnDestroy {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private farmSrv = inject(FarmService);
  private locationSrv = inject(LocationService);

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
  readonly MAX_FILE_SIZE_MB = 5;
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
  @ViewChild('mapContainer', { static: false }) mapContainer?: ElementRef<HTMLDivElement>;
  private map?: L.Map;
  private marker?: L.Marker;
  // Centro por defecto (Huila aprox.)
  private defaultCenter: [number, number] = [2.9386, -75.2519];
  private defaultZoom = 8;
  

  farmId?: number;

  ngOnInit(): void {
    this.initForms();
    
    
    this.loadDepartments();

    // Modo edición si viene :id
    this.route.paramMap.subscribe((params) => {
      const idParam = params.get('id');

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
      
    });


    
    
  }

  ngAfterViewInit(): void {
    this.initMap();
  }

  ngOnDestroy(): void {
    this.map?.remove();
  }

  /* ============================ INIT FORMS ============================ */
  private initForms(): void {
    this.generalGroup = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(120)]],
      hectares: [0, [Validators.required, Validators.min(0)]],
      altitude: [0, [Validators.required, Validators.min(0)]],
      // Solo si createWithProducer
      description: [''],
    });

    this.ubicacionGroup = this.fb.group({
      departmentId: [null, [Validators.required]],
      cityId: [null, [Validators.required]],
      latitude: [null, [Validators.required]],
      longitude: [null, [Validators.required]],
    });
  }

  /* ============================ LOAD DATA ============================ */
  private loadFarm(id: number): void {
    this.isLoading = true;
    this.farmSrv.getById(id).subscribe({
      next: (f) => {
        this.patchFromSelect(f);
        // Si tu select incluye imágenes con publicId y url, úsalo directo; si no, crea un service aparte
        this.existingImages = f.images ?? [];
        // Sincroniza marcador con coordenadas
        const lat = Number(this.ubicacionGroup.value.latitude) || this.defaultCenter[0];
        const lng = Number(this.ubicacionGroup.value.longitude) || this.defaultCenter[1];
        this.setMarker(lat, lng, true);
      },
      complete: () => (this.isLoading = false),
      error: () => (this.isLoading = false),
    });
  }

  private patchFromSelect(f: FarmSelectModel): void {
    this.generalGroup.patchValue({
      name: f.name,
      hectares: f['hectares' as keyof FarmSelectModel] ?? 0, // por si tu SelectModel no lo trae
      altitude: f['altitude' as keyof FarmSelectModel] ?? 0,
      description: '',
    });

    // Necesitas mapear cityId; tu SelectModel trae cityName/departmentName, no IDs.
    // Para edición real, tu endpoint GET/{id} debería también retornar cityId y departmentId.
    // Aquí asumimos que tienes cityId disponible (ajusta según tu DTO real).
    // Ejemplo defensivo:
    const cityIdGuess = (f as any).cityId ?? null;
    const departmentIdGuess = (f as any).departmentId ?? null;

    this.ubicacionGroup.patchValue({
      departmentId: departmentIdGuess,
      cityId: cityIdGuess,
      latitude: Number(f.latitude),
      longitude: Number(f.longitude),
    });

    // Si tenemos departmentId, cargamos ciudades y seleccionamos cityId
    if (departmentIdGuess) {
      this.onDepartmentChange(departmentIdGuess, cityIdGuess);
    }
  }

  private resetBeforeLoad(): void {
    this.generalGroup.reset({ hectares: 0, altitude: 0, description: '' });
    this.ubicacionGroup.reset({ departmentId: null, cityId: null, latitude: null, longitude: null });
    this.selectedFiles = [];
    this.imagesPreview = [];
    this.existingImages = this.isEdit ? this.existingImages : [];
    this.imagesToDelete = [];
    // Reset marker
    this.setMarker(this.defaultCenter[0] as number, this.defaultCenter[1] as number, true);
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
        if (presetCityId && cities.some(c => c.id === presetCityId)) {
          this.ubicacionGroup.patchValue({ cityId: presetCityId });
        }
      },
    });
  }

  /* ============================ MAPA ============================ */
  private initMap(): void {
    if (!this.mapContainer) return;

    // Fix iconos en Angular (evita 404)
    const iconRetinaUrl = 'leaflet/layers-2x.png';
    const iconUrl = 'leaflet/marker-icon.png';
    const shadowUrl = 'leaflet/marker-shadow.png';
    // Copia estos 3 archivos desde node_modules/leaflet/dist/images/ a assets/leaflet/
    // o ajusta las rutas según tu bundling.
    L.Icon.Default.mergeOptions({ iconRetinaUrl, iconUrl, shadowUrl });

    this.map = L.map(this.mapContainer.nativeElement, {
      center: this.defaultCenter,
      zoom: this.defaultZoom,
    });

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution:
        '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
      maxZoom: 19,
    }).addTo(this.map);

    // Click en mapa → mover marcador y actualizar form
    this.map.on('click', (e: L.LeafletMouseEvent) => {
      const { lat, lng } = e.latlng;
      this.setMarker(lat, lng, true);
      this.ubicacionGroup.patchValue({ latitude: lat, longitude: lng });
    });

    // Inicia marcador
    const lat = this.ubicacionGroup.value.latitude ?? (this.defaultCenter as number[])[0];
    const lng = this.ubicacionGroup.value.longitude ?? (this.defaultCenter as number[])[1];
    this.setMarker(lat, lng, false);
  }

  private setMarker(lat: number, lng: number, pan = false): void {
    if (!this.map) return;

    if (!this.marker) {
      this.marker = L.marker([lat, lng], { draggable: true }).addTo(this.map);
      this.marker.on('dragend', () => {
        const pos = this.marker!.getLatLng();
        this.ubicacionGroup.patchValue({ latitude: pos.lat, longitude: pos.lng });
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
        if (ev.target?.result) this.imagesPreview.push(ev.target.result as string);
      };
      reader.readAsDataURL(file);
    });
  }

  removeImage(index: number, isExisting: boolean): void {
    if (this.isDeletingImage) return;

    if (isExisting) {
      // Si soportas borrado inmediato en backend, llama a un servicio análogo a ProductImageService para Farm
      const img = this.existingImages[index];
      if (!img?.publicId) {
        this.existingImages.splice(index, 1);
        return;
      }
      // Alternativa: delegar al PUT usando imagesToDelete
      this.imagesToDelete.push(img.publicId);
      this.existingImages.splice(index, 1);
    } else {
      this.selectedFiles.splice(index, 1);
      this.imagesPreview.splice(index, 1);
    }
  }

  /* ============================ SUBMIT ============================ */
  submit(): void {
    if (this.generalGroup.invalid || this.ubicacionGroup.invalid) {
      this.generalGroup.markAllAsTouched();
      this.ubicacionGroup.markAllAsTouched();
      return;
    }

    if (!this.isEdit && this.selectedFiles.length === 0) {
      alert('Debes agregar al menos una imagen para crear la finca');
      return;
    }

    this.isLoading = true;

    const g = this.generalGroup.value;
    const u = this.ubicacionGroup.value;

    if (this.isEdit) {
      const dto: FarmUpdateModel = {
        id: this.farmId!,
        name: g.name,
        hectares: Number(g.hectares),
        altitude: Number(g.altitude),
        latitude: Number(u.latitude),
        longitude: Number(u.longitude),
        cityId: Number(u.cityId),
        images: this.selectedFiles.length ? this.selectedFiles : undefined,
        imagesToDelete: this.imagesToDelete.length ? this.imagesToDelete : undefined,
      };

      this.farmSrv.update(dto).subscribe({
        next: (resp) => {
          Swal.fire({
            icon: 'success',
            title: '¡Actualizada!',
            text: 'La finca se actualizó con éxito',
            confirmButtonText: 'Aceptar',
          }).then(() => {
            this.saved.emit(resp);
            this.resetAfterSave();
            this.router.navigateByUrl('/account/producer/management/farm');
          });
        },
        error: () => {
          Swal.fire({ icon: 'error', title: 'Error', text: 'No se pudo actualizar la finca' });
          this.isLoading = false;
        },
      });
    } else {
      if (this.createWithProducer) {
        const dto: FarmWithProducerRegisterModel = {
          name: g.name,
          description: g.description ?? '',
          hectares: Number(g.hectares),
          altitude: Number(g.altitude),
          latitude: Number(u.latitude),
          longitude: Number(u.longitude),
          images: this.selectedFiles,
          cityId: Number(u.cityId),
        };
        this.farmSrv.createWithProducer(dto).subscribe({
          next: (resp) => {
            Swal.fire({ icon: 'success', title: '¡Creado!', text: 'Productor y finca creados' }).then(() => {
              this.saved.emit(resp);
              this.resetAfterSave();
              this.router.navigateByUrl('/account/producer/management/farm');
            });
          },
          error: () => {
            Swal.fire({ icon: 'error', title: 'Error', text: 'No se pudo crear productor + finca' });
            this.isLoading = false;
          },
        });
      } else {
        const dto: FarmRegisterModel = {
          name: g.name,
          hectares: Number(g.hectares),
          altitude: Number(g.altitude),
          latitude: Number(u.latitude),
          longitude: Number(u.longitude),
          images: this.selectedFiles,
          cityId: Number(u.cityId),
        };
        this.farmSrv.create(dto).subscribe({
          next: (resp) => {
            Swal.fire({ icon: 'success', title: '¡Creada!', text: 'La finca se registró con éxito' }).then(() => {
              this.saved.emit(resp);
              this.resetAfterSave();
              this.router.navigateByUrl('/account/producer/management/farm');
            });
          },
          error: () => {
            Swal.fire({ icon: 'error', title: 'Error', text: 'No se pudo registrar la finca' });
            this.isLoading = false;
          },
        });
      }
    }
  }

  

  cancel(): void {
    this.resetBeforeLoad();
  }

  private resetAfterSave(): void {
    this.isLoading = false;
    this.resetBeforeLoad();
  }
}
