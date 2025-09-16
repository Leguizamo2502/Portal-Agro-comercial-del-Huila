import { FarmSelectModel } from './../../shared/models/farm/farm.model';
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { forkJoin, of } from 'rxjs';
import { map, catchError, switchMap, tap } from 'rxjs/operators';

import { ProductSelectModel } from '../../shared/models/product/product.model';

import { ProductService } from '../../shared/services/product/product.service';
import { FarmService } from '../../shared/services/farm/farm.service';
import { ProducerService } from '../../shared/services/producer/producer.service';

import { CardFarmComponent } from '../../shared/components/cards/card-farm/card-farm.component';
import { ContainerCardComponent } from '../../shared/components/cards/container-card/container-card.component'; // 👈 nuevo
import { ProducerSelectModel } from '../../shared/models/producer/producer.model';
import { ContainerCardProductorComponent } from "../../shared/components/cards/container-card-productor/container-card-productor.component";

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
  imports: [
    CommonModule,
    CardFarmComponent,
    ContainerCardProductorComponent
],
  templateUrl: './producer-profile.component.html',
  styleUrls: ['./producer-profile.component.css'],
})
export class ProducerProfileComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private productService = inject(ProductService);
  products:ProductSelectModel[] = [];
  private farmService = inject(FarmService);
  famrs : FarmSelectModel[] = [];
  private producerService = inject(ProducerService);
  producer?:ProducerSelectModel
  
  
  code: string ='';
  
  
  
  ngOnInit(): void {
    this.code = String(this.route.snapshot.paramMap.get('code'));
    if (!this.code) return;
    this.loadFarm();
    this.loadProduct();
    this.loadproducer();
  }

  loadproducer(){
    this.producerService.getByCodeProducer(this.code).subscribe((data)=>{
      this.producer = data;
    })
  }

  loadProduct(){
    this.productService.getProductByCodeProducer(this.code).subscribe((data)=>{
      this.products = data;
    })
  }

  loadFarm(){
    this.farmService.getFarmByCodeProducer(this.code).subscribe((data)=>{
      this.famrs = data;
    })
  }
}
