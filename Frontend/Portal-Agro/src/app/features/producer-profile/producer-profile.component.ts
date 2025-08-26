import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProductSelectModel } from '../../shared/models/product/product.model';
import { FarmSelectModel } from '../../shared/models/farm/farm.model';

import { ProductService } from '../../shared/services/product/product.service';
import { FarmService } from '../../shared/services/farm/farm.service';
import { AuthService } from '../../Core/services/auth/auth.service';
import { UserSelectModel } from '../../Core/Models/user.model';

import { CarruselComponent } from '../../shared/components/carrusel/carrusel.component';
import { ContainerCardComponent } from '../../shared/components/cards/container-card/container-card.component';
import { CardFarmComponent } from '../../shared/components/cards/card-farm/card-farm.component';

import { forkJoin, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { CardComponent } from '../../shared/components/cards/card/card.component';

type UserVM = {
  id: number;
  name: string;
  email: string;
  imageUrl?: string | null;
};

type FarmVM = FarmSelectModel & {
  imageUrl?: string | null;
};

type ProductVM = ProductSelectModel & {
  imageUrl?: string | null;
};

type ProfileVM = {
  user: UserVM;
  farms: FarmVM[];
  products: ProductVM[];
  qrUrl?: string | null;
  description?: string | null;
};

@Component({
  selector: 'app-producer-profile',
  standalone: true,
  imports: [CommonModule, CardComponent, CardFarmComponent],
  templateUrl: './producer-profile.component.html',
  styleUrls: ['./producer-profile.component.css'],
})
export class ProducerProfileComponent implements OnInit {
  profile?: ProfileVM;

  constructor(
    private productService: ProductService,
    private farmService: FarmService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    // USER
    const user$ = this.authService.GetDataBasic().pipe(
      map((u: UserSelectModel) => ({
        id: u.id,
        name: `${u.firstName ?? ''} ${u.lastName ?? ''}`.trim(),
        email: u.email,
        imageUrl: null,
      }) as UserVM),
      catchError(() => of({ id: 0, name: '', email: '', imageUrl: null } as UserVM))
    );

    // FARMS (solo 3)
    const farms$ = this.farmService.getByProducer().pipe(
      map((farms: FarmSelectModel[]) =>
        farms.slice(0, 3).map((f) => ({
          ...f,
          imageUrl: f.images?.[0]?.imageUrl ?? 'assets/default-farm.jpg',
        }) as FarmVM)
      ),
      catchError(() => of([] as FarmVM[]))
    );

    // PRODUCTS (solo 3)
    const products$ = this.productService.getByProducerId().pipe(
      map((ps: ProductSelectModel[]) =>
        ps.slice(0, 3).map((p) => ({
          ...p,
          imageUrl: p.images?.[0]?.imageUrl ?? 'assets/default-product.jpg',
        }) as ProductVM)
      ),
      catchError(() => of([] as ProductVM[]))
    );

    forkJoin({ user: user$, farms: farms$, products: products$ })
      .pipe(
        map(({ user, farms, products }) => ({
          user,
          farms,
          products,
          qrUrl: null,
          description: null,
        }) as ProfileVM)
      )
      .subscribe({
        next: (vm) => (this.profile = vm),
        error: (err) => {
          console.error('Error cargando datos:', err);
          this.profile = undefined;
        },
      });
  }
}
