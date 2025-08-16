import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SidebarService {
  // Signal para controlar si está abierta o cerrada
  // Por defecto true para desktop
  private _isOpen = signal(true);
  
  // Getter público para leer el estado
  get isOpen() {
    return this._isOpen.asReadonly();
  }

  // Método para abrir la sidebar
  open() {
    this._isOpen.set(true);
  }

  // Método para cerrar la sidebar
  close() {
    this._isOpen.set(false);
  }

  // Método para alternar el estado
  toggle() {
    this._isOpen.update(value => !value);
  }

  // Método para inicializar basado en el tamaño de pantalla
  initializeBasedOnScreenSize() {
    const isDesktop = window.innerWidth >= 992; // Bootstrap lg breakpoint
    this._isOpen.set(isDesktop);
  }
}