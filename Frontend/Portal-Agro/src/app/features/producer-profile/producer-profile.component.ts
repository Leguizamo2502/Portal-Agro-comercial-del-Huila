import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { forkJoin, of } from 'rxjs';
import { map, catchError, switchMap, tap } from 'rxjs/operators';

import { ProductSelectModel } from '../../shared/models/product/product.model';
import { FarmSelectModel } from '../../shared/models/farm/farm.model';

import { ProductService } from '../../shared/services/product/product.service';
import { FarmService } from '../../shared/services/farm/farm.service';
import { ProducerService } from '../../shared/services/producer/producer.service';

import { CardComponent } from '../../shared/components/cards/card/card.component';
import { CardFarmComponent } from '../../shared/components/cards/card-farm/card-farm.component';

type UserVM = {
  id: number;
  name: string;
  email: string;
  imageUrl?: string | null;
};

type FarmVM = FarmSelectModel & { imageUrl?: string | null };
type ProductVM = ProductSelectModel & { imageUrl?: string | null };

type ProfileVM = {
  user: UserVM;
  farms: FarmVM[];
  products: ProductVM[];
  qrUrl?: string | null;
  description?: string | null;
  phoneNumber: string;
};

@Component({
  selector: 'app-producer-profile',
  standalone: true,
  imports: [CommonModule, CardComponent, CardFarmComponent],
  templateUrl: './producer-profile.component.html',
  styleUrls: ['./producer-profile.component.css'],
})
export class ProducerProfileComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private productService = inject(ProductService);
  private farmService = inject(FarmService);
  private producerService = inject(ProducerService);

  profile?: ProfileVM;

  ngOnInit(): void {
    // Param :code o :producerCode, limpiando un posible ":" inicial
    const code$ = this.route.paramMap.pipe(
      map(pm => pm.get('code') || pm.get('producerCode')),
      map(code => (code ?? '').trim().replace(/^:/, '')),
      tap(code => console.log('[ProducerProfile] code param =', code))
    );

    code$
      .pipe(
        switchMap(code => {
          if (!code) {
            return of(undefined as unknown as ProfileVM);
          }

          const producer$ = this.producerService.getByCodeProducer(code).pipe(
            tap(res => console.log('[ProducerProfile] raw producer =', res)),
            catchError(err => {
              console.error('[ProducerProfile] error producer =', err);
              return of(null);
            })
          );

          // FINCAS: normaliza objeto/array -> array (sin límite)
          const farms$ = this.farmService.getFarmByCodeProducer(code).pipe(
            tap(res => console.log('[ProducerProfile] raw farm =', res)),
            map((res: any) => {
              const list: FarmSelectModel[] = Array.isArray(res)
                ? res
                : (res ? [res] : []);
              const vms: FarmVM[] = list.map((f: FarmSelectModel) => ({
                ...f,
                imageUrl: f?.images?.[0]?.imageUrl ?? 'assets/default-farm.jpg',
              }));
              return vms; // ← sin slice: trae todas
            }),
            catchError(err => {
              console.error('[ProducerProfile] error farm =', err);
              return of([] as FarmVM[]);
            })
          );

          // PRODUCTOS: normaliza objeto/array -> array (sin límite)
          const products$ = this.productService.getProductByCodeProducer(code).pipe(
            tap(res => console.log('[ProducerProfile] raw product =', res)),
            map((res: any) => {
              const list: ProductSelectModel[] = Array.isArray(res)
                ? res
                : (res ? [res] : []);
              const vms: ProductVM[] = list.map((p: ProductSelectModel) => ({
                ...p,
                imageUrl: p?.images?.[0]?.imageUrl ?? 'assets/default-product.jpg',
              }));
              return vms; // ← sin slice: trae todas
            }),
            catchError(err => {
              console.error('[ProducerProfile] error product =', err);
              return of([] as ProductVM[]);
            })
          );

          return forkJoin({ producer: producer$, farms: farms$, products: products$ }).pipe(
            map(({ producer, farms, products }) => {
              if (!producer) return undefined as unknown as ProfileVM;

              const user: UserVM = {
                id: producer.id,
                name: producer.fullName,
                email: producer.email,
                imageUrl: null,
              };

              const vm: ProfileVM = {
                user,
                farms,
                products,
                qrUrl: producer.qrUrl ?? null,
                description: producer.description ?? null,
                phoneNumber: producer.phoneNumber,
              };
              console.log('[ProducerProfile] final VM =', vm);
              return vm;
            })
          );
        })
      )
      .subscribe({
        next: vm => (this.profile = vm),
        error: err => {
          console.error('[ProducerProfile] Error stream principal:', err);
          this.profile = undefined;
        },
      });
  }
}
