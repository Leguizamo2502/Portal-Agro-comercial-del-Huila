import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { ButtonComponent } from '../../shared/components/button/button.component';
import { FarmService } from '../../shared/services/farm/farm.service';
import { FarmSelectModel } from '../../shared/models/farm/farm.model';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser'; // <-- NUEVO

@Component({
  selector: 'app-farm-detail',
  standalone: true,
  imports: [],
  templateUrl: './farm-detail.component.html',
  styleUrls: ['./farm-detail.component.css'],
})
export class FarmDetailComponent {
  // private route = inject(ActivatedRoute);
  // private farmService = inject(FarmService);
  // private sanitizer = inject(DomSanitizer); // <-- NUEVO

  // private sub?: Subscription;

  // farmId!: number;
  // farm?: FarmSelectModel;

  // // Estados
  // loadingFarm = true;
  // errorMsg: string | null = null;
  // noId = false;

  // // Galería
  // selectedImage: string | null = null;

  // // Mapa (usado por el template)
  // mapUrl?: SafeResourceUrl; // <-- NUEVO

  // ngOnInit(): void {

  //   this.loadFarm(); // (lo dejo como lo tienes)
  //   const rawPath = this.route.snapshot.paramMap.get('id') ?? this.route.snapshot.paramMap.get('farmId');
  //   const rawQuery = this.route.snapshot.queryParamMap.get('id');
  //   const rawId = rawPath ?? rawQuery;

  //   console.log('[FarmDetail] rawPath:', rawPath, 'rawQuery:', rawQuery, 'rawId:', rawId);

  //   const parsed = rawId !== null ? Number(rawId) : NaN;

  //   if (Number.isNaN(parsed)) {
  //     console.warn('[FarmDetail] No hay id válido en la URL.');
  //     this.noId = true;
  //     this.loadingFarm = false;
  //     this.farm = undefined;
  //     return;
  //   }

  //   this.farmId = parsed;
    
  // }

  // ngOnDestroy(): void {
  //   this.sub?.unsubscribe();
  // }

  // private loadFarm(): void {
  //   this.loadingFarm = true;
  //   this.errorMsg = null;
  //   console.log('[FarmDetail] Solicitando finca id =', this.farmId);

  //   this.sub = this.farmService.getById(1).subscribe({ // <-- NO lo cambio
  //     next: (f: FarmSelectModel) => {
  //       console.log('[FarmDetail] Respuesta finca:', f);
  //       this.farm = f;
  //       this.loadingFarm = false;
  //       this.selectedImage = f?.images?.[0]?.imageUrl ?? null;

  //       // NUEVO: construir URL segura del mapa si hay coordenadas
  //       if (f?.latitude != null && f?.longitude != null) {
  //         const url = `https://www.google.com/maps?q=${f.latitude},${f.longitude}&z=14&output=embed`;
  //         this.mapUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
  //       } else {
  //         this.mapUrl = undefined;
  //       }
  //     },
  //     error: (err) => {
  //       console.error('[FarmDetail] Error cargando finca:', err);
  //       this.errorMsg = 'No fue posible cargar la finca. Revisa la conexión con la API.';
  //       this.loadingFarm = false;
  //       this.farm = undefined;
  //       this.mapUrl = undefined; // <-- limpia mapa en error
  //     }
  //   });
  // }

  // changeMainImage(url: string): void {
  //   this.selectedImage = url;
  // }

  // get mainImage(): string {
  //   if (this.selectedImage) return this.selectedImage;
  //   return this.farm?.images?.[0]?.imageUrl ?? '';
  // }

  // trackByImage = (_: number, img: FarmSelectModel['images'][number]) => img.id;
}
