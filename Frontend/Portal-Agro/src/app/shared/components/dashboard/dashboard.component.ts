import { Component, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  isCollapsed = false;

  // Estado independiente para cada submenú
  openSubmenus: { [key: string]: boolean } = {
    material: false,
    users: false,
    reports: false,
    settings: false
  };

  expandSidebar() {
    this.isCollapsed = false;
  }

  collapseSidebar() {
    this.isCollapsed = true;
  }

  toggleSubmenu(menu: string) {
    this.openSubmenus[menu] = !this.openSubmenus[menu];
  }
}
