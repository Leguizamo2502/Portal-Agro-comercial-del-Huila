import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

// Material
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatIconModule } from '@angular/material/icon';

import { ProductService } from '../../../../shared/services/product/product.service';
import { FarmService } from '../../../../shared/services/farm/farm.service';
import { ProducerService } from '../../../../shared/services/producer/producer.service';

import { ProductSelectModel } from '../../../../shared/models/product/product.model';
import { FarmSelectModel } from '../../../../shared/models/farm/farm.model';
import { ProducerSelectModel } from '../../../../shared/models/producer/producer.model';
import { ContainerCardProductorComponent } from '../../../../shared/components/cards/container-card-productor/container-card-productor.component';
import { SocialNetworkIconPipe } from '../../../../shared/pipes/social-network-icon/social-network-icon.pipe';
import { SocialNetworkLabelPipe } from '../../../../shared/pipes/social-network-label/social-network-label.pipe';

@Component({
  selector: 'app-producer-profile',
  standalone: true,
  imports: [
    CommonModule,
    ContainerCardProductorComponent,
    MatChipsModule,
    MatTooltipModule,
    MatIconModule,
    SocialNetworkIconPipe,
  ],
  templateUrl: './producer-profile.component.html',
  styleUrls: ['./producer-profile.component.css'],
})
export class ProducerProfileComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private productService = inject(ProductService);
  private farmService = inject(FarmService);
  private producerService = inject(ProducerService);

  products: ProductSelectModel[] = [];
  famrs: FarmSelectModel[] = [];
  producer?: ProducerSelectModel;

  saleNumber = 0;

  code: string = '';

  ngOnInit(): void {
    this.code = String(this.route.snapshot.paramMap.get('code'));
    if (!this.code) return;

    this.loadFarm();
    this.loadProduct();
    this.loadproducer();
    this.loadSalenumber();
  }

  loadproducer() {
    this.producerService.getByCodeProducer(this.code).subscribe((data) => {
      this.producer = data;
    });
  }

  loadProduct() {
    this.productService
      .getProductByCodeProducer(this.code)
      .subscribe((data) => {
        this.products = data;
      });
  }

  loadFarm() {
    this.farmService.getFarmByCodeProducer(this.code).subscribe((data) => {
      this.famrs = data;
    });
  }

  loadSalenumber() {
    this.producerService.getSalesNumberByCode(this.code).subscribe((data) => {
      this.saleNumber = data ?? 0;
      // console.log(this.saleNumber);
    });
  }

  trustLevelClass(n: number): string {
    if (n >= 50) return 'trust-high';
    if (n >= 10) return 'trust-mid';
    return 'trust-low';
  }

  /** Formateo compacto (1.2k, 3.4M). Fallback si Intl no soporta compact. */
  compact(n: number): string {
    try {
      return new Intl.NumberFormat('es-CO', {
        notation: 'compact',
        maximumFractionDigits: 1,
      }).format(n);
    } catch {
      if (n >= 1_000_000)
        return (n / 1_000_000).toFixed(1).replace(/\.0$/, '') + 'M';
      if (n >= 1_000) return (n / 1_000).toFixed(1).replace(/\.0$/, '') + 'k';
      return String(n);
    }
  }

  get avg(): number {
    return this.producer?.averageRating ?? 0;
  }

  /** 1..5: retorna el icono adecuado según el promedio (★, ☆ y medio) */
  iconForStar(pos: number): string {
    const v = this.avg;
    if (v >= pos) return 'star'; // llena
    if (v >= pos - 0.5) return 'star_half'; // media
    return 'star_border'; // vacía
  }

  /** 1 decimal, ej. 4.2 */
  formatAvg(): string {
    return this.avg.toFixed(1);
  }
}
