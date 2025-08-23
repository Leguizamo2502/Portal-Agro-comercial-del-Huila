import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SidebarService {
  // Signal que representa el estado del sidebar
  sidebarOpen = signal(false); // Cambiado: iniciar siempre cerrado
  
  // Signal para detectar si estamos en móvil
  private isMobile = signal(false);

  constructor() {
    this.updateScreenSize();
  }

  /** Detectar si estamos en pantalla móvil */
  private updateScreenSize() {
    this.isMobile.set(window.innerWidth < 768);
  }

  /** Inicializar estado según el tamaño de la pantalla */
  initializeBasedOnScreenSize() {
    this.updateScreenSize();
    
    // Solo en desktop abrimos automáticamente
    if (!this.isMobile()) {
      this.sidebarOpen.set(true);
    }
    // En móvil mantenemos el estado actual (no forzamos cerrar)
  }

  /** Alternar manualmente el estado (botón hamburguesa) */
  toggle() {
    this.sidebarOpen.update(v => !v);
  }

  /** Forzar abrir */
  openSidebar() {
    this.sidebarOpen.set(true);
  }

  /** Forzar cerrar */
  closeSidebar() {
    this.sidebarOpen.set(false);
  }

  /** Cerrar sidebar solo si estamos en móvil */
  closeOnMobile() {
    if (this.isMobile()) {
      this.closeSidebar();
    }
  }

  /** Obtener estado de móvil */
  getIsMobile() {
    return this.isMobile();
  }
}