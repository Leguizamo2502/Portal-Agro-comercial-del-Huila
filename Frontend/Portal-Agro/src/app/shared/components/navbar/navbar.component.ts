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

interface User {
  name: string;
  email: string;
  phone: string;
  avatar?: string;
}

interface Notification {
  id: number;
  title: string;
  description: string;
  time: string;
  read: boolean;
  type: 'accepted' | 'info' | 'warning';
  icon: string;
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
  cartCount = 2;
  showSuggestions = false;

  allSuggestions = [
    'Semillas de maíz híbrido',
    'Fertilizante orgánico compost',
    'Herramientas de jardín podadoras',
    'Abono para cultivos ecológico',
    'Pesticidas naturales bio',
    'Maquinaria agrícola tractores',
    'Semillas de tomate cherry',
    'Fertilizante NPK completo',
    'Sistema de riego goteo'
  ];

  searchSuggestions: string[] = [];

  user: User = {
    name: 'Vanessa Ortiz',
    email: 'vanessaortiz@gmail.com',
    phone: '310 000 0000'
  };

  userOptions = [
    { label: 'Perfil', icon: 'person' },
    { label: 'Mis pedidos', icon: 'shopping_bag' },
    { label: 'Configuración', icon: 'settings' }
  ];

  categories = ['Semillas', 'Fertilizantes', 'Herramientas', 'Maquinaria', 'Productos Orgánicos'];

  navigationItems: MenuItem[] = [
    { label: 'Inicio', route: '/' },
    { label: 'Productos', route: '/productos' }
  ];

  notifications: Notification[] = [
    {
      id: 1,
      title: 'Pedido Aceptado',
      description: 'Tu pedido por $450,000 ha sido aceptado',
      time: 'Hace 5 minutos',
      read: false,
      type: 'accepted',
      icon: 'check_circle'
    },
    {
      id: 2,
      title: 'Nuevo Mensaje',
      description: 'Tienes un mensaje del proveedor',
      time: 'Hace 2 horas',
      read: false,
      type: 'info',
      icon: 'message'
    }
  ];

  get unreadNotifications(): number {
    return this.notifications.filter(n => !n.read).length;
  }

  onSearch(): void {
    if (this.searchTerm.trim()) {
      console.log('Buscando:', this.searchTerm);
      this.showSuggestions = false;
    }
  }

  onKeyUp(event: KeyboardEvent): void {
    switch (event.key) {
      case 'Enter':
        this.onSearch();
        break;
      case 'Escape':
        this.showSuggestions = false;
        break;
      default:
        this.updateSuggestions();
    }
  }

  updateSuggestions(): void {
    const term = this.searchTerm.trim().toLowerCase();
    this.searchSuggestions = term
      ? this.allSuggestions.filter(s => s.toLowerCase().includes(term)).slice(0, 5)
      : [];
    this.showSuggestions = this.searchSuggestions.length > 0;
  }

  onSearchFocus(): void {
    this.updateSuggestions();
  }

  onSearchBlur(): void {
    setTimeout(() => this.showSuggestions = false, 200);
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.searchSuggestions = [];
    this.showSuggestions = false;
  }

  selectSuggestion(suggestion: string): void {
    this.searchTerm = suggestion;
    this.onSearch();
  }

  onNavigationClick(item: MenuItem): void {
    console.log('Navegando a:', item.route);
  }

  onCategoryClick(category: string): void {
    console.log('Categoría seleccionada:', category);
  }

  onCartClick(): void {
    console.log('Abriendo carrito');
  }

  onUserOptionClick(option: string): void {
    console.log('Opción seleccionada:', option);
    if (option === 'Cerrar Sesión') {
      console.log('Cerrando sesión...');
    }
  }

  markAsRead(notification: Notification): void {
    notification.read = true;
    console.log('Notificación leída:', notification.title);
  }
}