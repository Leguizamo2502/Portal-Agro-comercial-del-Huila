import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';

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

@Component({
  selector: 'app-navbar-vertical',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule
  ],
  templateUrl: './navbar-vertical.component.html',
  styleUrls: ['./navbar-vertical.component.css']
})
export class NavbarVerticalComponent {
  user: User = {
    name: 'Vanessa Ortiz',
    email: 'vanessaortiz@gmail.com',
    phone: '310 000 0000'
  };

  userMenuItems: UserMenuItem[] = [
    { label: 'Mi Información', icon: 'person', active: true },
    { label: 'Favoritos', icon: 'favorite', active: false },
    { label: 'Pedidos', icon: 'inventory_2', active: false },
    { label: 'Productor', icon: 'agriculture', active: false },
    { label: 'Soporte', icon: 'headset_mic', active: false }
  ];

  onUserMenuItemClick(item: UserMenuItem) {
    // Desactivar todos los items
    this.userMenuItems.forEach((menuItem: UserMenuItem) => {
      menuItem.active = false;
    });
    // Activar el item seleccionado
    item.active = true;
    console.log('Sección seleccionada:', item.label);
  }}