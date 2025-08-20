import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { MatTabsModule } from '@angular/material/tabs';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-producer-layout',
  imports: [MatTabsModule, RouterOutlet, RouterLink, RouterLinkActive,CommonModule],
  templateUrl: './producer-layout.component.html',
  styleUrl: './producer-layout.component.css'
})
export class ProducerLayoutComponent {
  tabs = [
    { label: 'Resumen', path: 'summary' },
    { label: 'Gestión', path: 'management' },
  ];
}
