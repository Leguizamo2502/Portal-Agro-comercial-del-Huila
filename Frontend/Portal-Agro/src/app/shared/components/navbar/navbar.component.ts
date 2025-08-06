import { Component, inject } from '@angular/core';
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
import { Router } from '@angular/router';

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
    MatDividerModule,
    
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  router = inject(Router);



  searchTerm = '';
  cartCount = 2;
  showSuggestions = false;

  // Sugerencias mejoradas de búsqueda
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

  onSearch() {
    if (this.searchTerm.trim()) {
      console.log('Buscando:', this.searchTerm);
      this.showSuggestions = false;
    }
  }

  onKeyUp(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.onSearch();
    } else if (event.key === 'Escape') {
      this.showSuggestions = false;
    } else {
      this.updateSuggestions();
    }
  }

  updateSuggestions() {
    if (this.searchTerm.trim().length > 0) {
      this.searchSuggestions = this.allSuggestions
        .filter(suggestion => 
          suggestion.toLowerCase().includes(this.searchTerm.toLowerCase())
        )
        .slice(0, 5);
      this.showSuggestions = this.searchSuggestions.length > 0;
    } else {
      this.searchSuggestions = [];
      this.showSuggestions = false;
    }
  }

  onSearchFocus() {
    this.updateSuggestions();
  }

  onSearchBlur() {
    // Delay para permitir click en sugerencias
    setTimeout(() => {
      this.showSuggestions = false;
    }, 200);
  }

  clearSearch() {
    this.searchTerm = '';
    this.searchSuggestions = [];
    this.showSuggestions = false;
  }

  selectSuggestion(suggestion: string) {
    this.searchTerm = suggestion;
    this.showSuggestions = false;
    this.onSearch();
  }

  onNavigationClick(item: MenuItem) {
    console.log('Navegando a:', item.route);
  }

  onCategoryClick(category: string) {
    console.log('Categoría seleccionada:', category);
  }

  onCartClick() {
    console.log('Abriendo carrito');
  }

  onUserOptionClick(option: string) {
    console.log('Opción seleccionada:', option);
    if (option === 'Mi Cuenta') {
      this.router.navigate([''])
    }
  }

  markAsRead(notification: Notification) {
    notification.read = true;
    console.log('Notificación leída:', notification.title);
  }
}
