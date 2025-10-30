import { Component, OnInit, inject } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common';
import { filter } from 'rxjs/operators';
import { DriverJsService } from '../../services/driverJS/driver-js.service';

@Component({
  selector: 'app-driver-tour',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './driver-tour.component.html',
  styleUrls: ['./driver-tour.component.css']
})
export class DriverTourComponent implements OnInit {
  private router = inject(Router);
  private driverService = inject(DriverJsService);

  ngOnInit(): void {
    // Escucha los cambios de ruta y ejecuta el tour automáticamente
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        this.runTour(event.urlAfterRedirects);
      });
  }

  /**
   * Ejecuta el tour según la ruta actual
   */
  private runTour(currentUrl: string) {
    const currentRoute = currentUrl.split('?')[0];
    // El servicio ahora maneja la lógica de obtener los pasos
    this.driverService.startTour();
  }

  /**
   * Permite ejecutar el tour manualmente (por ejemplo, desde la navbar)
   */
  launchTour() {
    this.driverService.startTour();
  }
}
