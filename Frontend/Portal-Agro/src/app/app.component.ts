import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NavbarBuenoComponent } from "./shared/components/navbar-bueno/navbar-bueno.component";
import { AuthState } from './Core/services/auth/auth.state';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule, NavbarBuenoComponent],
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
