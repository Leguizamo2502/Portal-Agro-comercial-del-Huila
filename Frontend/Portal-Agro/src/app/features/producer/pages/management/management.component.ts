import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-management',
  imports: [MatTabsModule, RouterOutlet, RouterLink, RouterLinkActive,CommonModule],
  templateUrl: './management.component.html',
  styleUrl: './management.component.css'
})
export class ManagementComponent {
 tabs = [
    { label: 'Productos', path: 'product' },
    { label: 'Fincas', path: 'farm' }
  ];
}
