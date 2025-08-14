import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  isSidebarOpen = false;
  activePath = 'info';

  user = {
    name: 'Daniel Bata',
    email: 'daniel@example.com'
  };

  openSubmenus: { [key: string]: boolean } = {
    material: false,
    users: false,
    reports: false,
    settings: false
  };

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  toggleSubmenu(menu: string) {
    this.openSubmenus[menu] = !this.openSubmenus[menu];
  }

  navigateTo(path: string) {
    this.activePath = path;
  }
}
