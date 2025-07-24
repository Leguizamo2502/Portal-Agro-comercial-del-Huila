import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatBadgeModule } from '@angular/material/badge';
import { MatDividerModule } from '@angular/material/divider';

interface MenuItem {
  label: string;
  route: string;
}

interface UserMenuItem {
  label: string;
  icon: string;
  active: boolean;
}

interface User {
  name: string;
  email: string;
}

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatToolbarModule,
    MatButtonModule,
    MatMenuModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatBadgeModule,
    MatDividerModule
  ],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  searchTerm = '';
  notificationCount = 3;
  cartCount = 2;

  user: User = {
    name: 'Vanessa Ortiz',
    email: 'vanessaortiz@gmail.com'
  };

  categories = ['Semillas', 'Fertilizantes', 'Herramientas', 'Maquinaria', 'Productos Orgánicos'];

  navigationItems: MenuItem[] = [
    { label: 'Inicio', route: '/' },
    { label: 'Productos', route: '/productos' }
  ];

  userMenuItems: UserMenuItem[] = [
    { label: 'Mi Información', icon: 'person', active: false },
    { label: 'Favoritos', icon: 'favorite', active: false },
    { label: 'Pedidos', icon: 'inventory_2', active: false },
    { label: 'Productor', icon: 'agriculture', active: true },
    { label: 'Soporte', icon: 'headset_mic', active: false }
  ];

  onSearch() {
    if (this.searchTerm.trim()) {
      console.log('Buscando:', this.searchTerm);
    }
  }

  onKeyUp(event: KeyboardEvent) {
    if (event.key === 'Enter') this.onSearch();
  }

  onNavigationClick(item: MenuItem) {
    console.log('Ir a:', item.route);
  }

  onCategoryClick(category: string) {
    console.log('Categoría seleccionada:', category);
  }

  onNotificationClick() {
    console.log('Ver notificaciones');
  }

  onCartClick() {
    console.log('Ir al carrito');
  }

  onUserMenuItemClick(item: UserMenuItem) {
    console.log('Menú usuario:', item.label);
  }

  onUserOptionClick(option: string) {
    console.log('Opción usuario:', option);
  }
}
