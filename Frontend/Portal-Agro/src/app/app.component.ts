import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthState } from './Core/services/auth/auth.state';
import { MainLayoutComponent } from "./shared/components/layouts/main-layout/main-layout.component";

import { NavbarBuenoComponent } from './shared/components/navs/navbar-bueno/navbar-bueno.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule, MainLayoutComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  title = 'Portal-Agro';
  router = inject(Router);
  private authState = inject(AuthState);
   ngOnInit() {
    this.authState.hydrateFromStorage();
    // Opcional: refrescar desde el backend si hay cookie/sesión
    // this.authState.loadMe().pipe(take(1)).subscribe({ next: () => {} , error: () => {} });
  }
}
