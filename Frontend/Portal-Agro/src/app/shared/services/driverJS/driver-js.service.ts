import { Injectable } from '@angular/core';
import { driver, DriveStep } from 'driver.js';
import 'driver.js/dist/driver.css';

@Injectable({
  providedIn: 'root'
})
export class DriverJsService {
  private openedElements: HTMLElement[] = [];
  private originalStates: Map<HTMLElement, boolean> = new Map();
  private driverInstance: any;

  // Referencia al handler para poder removerlo después
  private documentClickHandler: ((e: MouseEvent) => void) | null = null;

  startTour(steps: DriveStep[]) {
    this.prepareTourElements();

    this.driverInstance = driver({
      showProgress: true,
      animate: true,
      overlayOpacity: 0.6,
      allowClose: true,
      nextBtnText: 'Siguiente',
      prevBtnText: 'Anterior',
      doneBtnText: 'Finalizar',
      steps,
      onDestroyed: () => this.stopTour(), // limpieza si driver la dispara
    });

    this.driverInstance.drive();

    // Instala el listener delegado en document (capturing)
    this.attachGlobalCloseListener();
  }

  /**
   * Listener global para detectar clicks en el botón "X" generado por driver.js.
   * Usa delegación y varios selectores posibles para cubrir distintas versiones
   * de la librería y distintos DOMs.
   */
  private attachGlobalCloseListener() {
    // Si ya existe un handler registrado, no lo registramos de nuevo
    if (this.documentClickHandler) return;

    this.documentClickHandler = (event: MouseEvent) => {
      const target = event.target as HTMLElement | null;
      if (!target) return;

      // Lista de selectores candidatos (pueden variar entre versiones)
      const selectors = [
        '.driver-popover-close-btn', // variante que usaste antes
        '.driver-close-btn',
        '.driverjs-close',
        '.driver-close', 
        '.driver-popover .close', // fallback genérico
        '[aria-label="close"]',
        '[aria-label="Close"]',
      ];

      // Si cualquier ancestro del target coincide con uno de los selectores, cerrar
      for (const sel of selectors) {
        if (target.closest(sel)) {
          // Llamamos a stopTour() y prevenimos que haya efectos dobles
          this.stopTour();
          // Si queremos evitar que otros handlers reciban este click:
          // event.stopPropagation(); event.preventDefault();
          return;
        }
      }

      // Adicional: algunos botones pueden ser un <button> con contenido "×"
      // (solo como último recurso)
      const btn = target.closest('button');
      if (btn && btn.textContent && btn.textContent.trim() === '×') {
        this.stopTour();
      }
    };

    // Usamos capture = true para interceptar antes que driver.js en algunos casos
    document.addEventListener('click', this.documentClickHandler, true);
  }

  stopTour() {
    // Destruir instancia driver (si existe)
    if (this.driverInstance) {
      try {
        // algunas versiones exponen destroy()
        if (typeof this.driverInstance.destroy === 'function') {
          this.driverInstance.destroy();
        } else if (typeof this.driverInstance.reset === 'function') {
          this.driverInstance.reset();
        }
      } catch (err) {
        // ignorar errores silenciosos
      }
      this.driverInstance = null;
    }

    // Restaurar elementos que abrimos
    this.restoreTourElements();

    // Quitar listener global si existe
    if (this.documentClickHandler) {
      document.removeEventListener('click', this.documentClickHandler, true);
      this.documentClickHandler = null;
    }
  }

  private prepareTourElements() {
    const elements = document.querySelectorAll('[data-tour-open]');
    elements.forEach(el => {
      const element = el as HTMLElement;
      const isHidden = element.offsetParent === null;

      this.originalStates.set(element, isHidden);

      if (isHidden) {
        element.classList.add('tour-temp-visible');
        element.style.display = 'block';
        element.style.opacity = '1';
      }

      if (element.hasAttribute('data-auto-click')) {
        setTimeout(() => element.click(), 300);
      }

      this.openedElements.push(element);
    });
  }

  private restoreTourElements() {
    this.openedElements.forEach(el => {
      const wasHidden = this.originalStates.get(el);
      if (wasHidden) {
        el.classList.remove('tour-temp-visible');
        el.style.display = 'none';
        el.style.opacity = '0';
      }
    });
    this.openedElements = [];
    this.originalStates.clear();
  }
}
