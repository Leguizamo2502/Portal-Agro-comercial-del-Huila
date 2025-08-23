import { SidebarService } from './../../../services/sidebar/sidebar.service';
import { Component, OnInit, OnDestroy, HostListener, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from '../../dashboard/dashboard.component';
import { NavbarBuenoComponent } from '../../navs/navbar-bueno/navbar-bueno.component';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, DashboardComponent, NavbarBuenoComponent],
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.css']
})
export class MainLayoutComponent implements OnInit, OnDestroy {

  sidebarService = inject(SidebarService);
  router = inject(Router);

  /** Solo verdadero cuando estamos en /account (o rutas hijas) */
  get isAccountRoute(): boolean {
    // Ajusta el prefijo según tus rutas: '/account'
    return this.router.url.startsWith('/account');
  }

  /** En desktop mostramos el margen solo si: ruta = /account + sidebar abierto + no es móvil */
  get showSidebarOnDesktop(): boolean {
    return this.isAccountRoute && !this.sidebarService.getIsMobile() && this.sidebarService.sidebarOpen();
  }

  ngOnInit(): void {
    this.sidebarService.initializeBasedOnScreenSize();
  }

  ngOnDestroy(): void {}

  toggleSidebar(): void {
    this.sidebarService.toggle();
  }

  @HostListener('window:resize')
  onResize(): void {
    const wasMobile = this.sidebarService.getIsMobile();
    this.sidebarService.initializeBasedOnScreenSize();
    const isMobileNow = this.sidebarService.getIsMobile();

    // Si cambiamos de móvil a desktop, abrir sidebar para ver el menú
    if (wasMobile && !isMobileNow && this.isAccountRoute) {
      this.sidebarService.openSidebar();
    }
  }

  onBackdropClick(): void {
    this.sidebarService.closeOnMobile();
  }
}
