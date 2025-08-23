import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SidebarService } from '../../../services/sidebar/sidebar.service';
import { DashboardComponent } from '../../dashboard/dashboard.component';
import { NavbarBuenoComponent } from '../../navs/navbar-bueno/navbar-bueno.component';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    DashboardComponent,
    NavbarBuenoComponent
  ],
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.css']
})
export class MainLayoutComponent implements OnInit, OnDestroy {

  constructor(public sidebarService: SidebarService) {}

  ngOnInit(): void {
    this.sidebarService.initializeBasedOnScreenSize();
  }

  ngOnDestroy(): void {
    // Cleanup si es necesario
  }

  // Función para alternar la visibilidad de la barra lateral
  toggleSidebar() {
    this.sidebarService.toggle();
  }

  // Listener para cambios de tamaño de ventana con debounce
  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    const wasMobile = this.sidebarService.getIsMobile();
    this.sidebarService.initializeBasedOnScreenSize();
    const isMobileNow = this.sidebarService.getIsMobile();
    
    // Si cambiamos de móvil a desktop, abrir sidebar
    if (wasMobile && !isMobileNow) {
      this.sidebarService.openSidebar();
    }
  }

  // Cerrar sidebar cuando se hace clic fuera (solo en móvil)
  onBackdropClick() {
    this.sidebarService.closeOnMobile();
  }
}
