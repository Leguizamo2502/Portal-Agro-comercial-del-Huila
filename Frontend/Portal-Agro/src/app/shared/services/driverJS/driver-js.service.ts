import { Injectable } from '@angular/core';
import { driver, DriveStep } from 'driver.js';

@Injectable({
  providedIn: 'root'
})
export class DriverJsService {
  private openedElements: HTMLElement[] = [];
  private originalStates: Map<HTMLElement, boolean> = new Map();
  private driverInstance: any;

  /**
   * Inicia el tour y gestiona la apertura temporal de elementos.
   */
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
      onDestroyed: () => this.restoreTourElements(), // restaurar al finalizar
      onCloseClick: () => this.restoreTourElements()
    });

    this.driverInstance.drive();
  }

  /**
   *Busca todos los elementos con el atributo [data-tour-open]
   * y los muestra o activa temporalmente mientras dura el tour.
   */
  private prepareTourElements() {
    const elements = document.querySelectorAll('[data-tour-open]');

    elements.forEach(el => {
      const element = el as HTMLElement;
      const isHidden = element.offsetParent === null;

      // Guarda el estado original
      this.originalStates.set(element, isHidden);

      // Si está oculto, mostrarlo temporalmente
      if (isHidden) {
        element.classList.add('tour-temp-visible');
        element.style.display = 'block';
        element.style.opacity = '1';
      }

      // Si tiene [data-auto-click], ejecutar un clic
      if (element.hasAttribute('data-auto-click')) {
        setTimeout(() => (element as HTMLElement).click(), 300);
      }

      this.openedElements.push(element);
    });
  }

  /**
   * Restaura el estado original de los elementos abiertos durante el tour.
   */
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
