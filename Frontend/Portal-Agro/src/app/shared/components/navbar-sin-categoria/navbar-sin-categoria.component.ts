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

@Component({
  selector: 'app-navbar-sin-categoria',
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
  templateUrl: './navbar-sin-categoria.component.html',
  styleUrls: ['./navbar-sin-categoria.component.css']
})
export class NavbarSinCategoriaComponent {
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

  navigationItems: MenuItem[] = [
    { label: 'Inicio', route: '/' },
    { label: 'Productos', route: '/productos' },
    { label: 'Login/Registro', route: '/' }
  ];

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
}
