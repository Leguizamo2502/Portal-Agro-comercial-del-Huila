import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { ActivatedRoute, Router } from '@angular/router';

interface UserMenuItem {
  label: string;
  icon: string;
  active: boolean;
  children?: UserMenuItem[];
  expanded?: boolean; // solo si tiene hijos
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
  imports: [CommonModule, MatButtonModule, MatIconModule, MatDividerModule],
  templateUrl: './navbar-vertical.component.html',
  styleUrls: ['./navbar-vertical.component.css'],
})
export class NavbarVerticalComponent {
  router = inject(Router);
  route = inject(ActivatedRoute);
  activePath = '';


  user: User = {
    name: 'Vanessa Ortiz',
    email: 'vanessaortiz@gmail.com',
    phone: '310 000 0000',
  };

  navigateTo(path: string) {
    this.router.navigate([path], { relativeTo: this.route });
    this.activePath = path;
    console.log("Hola")
  }

  
}

