import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FarmSelectModel } from '../../../models/farm/farm.model';
import { MatCardModule } from "@angular/material/card";
import { MatIconModule } from "@angular/material/icon";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-card-farm',
  imports: [MatCardModule, MatIconModule,CommonModule],
  templateUrl: './card-farm.component.html',
  styleUrl: './card-farm.component.css'
})
export class CardFarmComponent {
  @Input({ required: true }) farm!: FarmSelectModel;
  @Input() showActions = false; // controla si se muestran los iconos

  @Output() edit = new EventEmitter<FarmSelectModel>();
  @Output() delete = new EventEmitter<FarmSelectModel>();

  private readonly placeholder = 'img/cargaImagen.png';

  get imageUrl(): string {
    const url = this.farm?.images?.[0]?.imageUrl;
    return url && url.trim() ? url : this.placeholder;
  }

  onImgError(ev: Event) {
    (ev.target as HTMLImageElement).src = this.placeholder;
  }
}
