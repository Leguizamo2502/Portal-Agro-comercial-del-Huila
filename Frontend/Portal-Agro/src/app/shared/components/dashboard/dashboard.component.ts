import { Component, inject, OnInit, OnDestroy, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { SidebarService } from '../../services/sidebar/sidebar.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, OnDestroy {
  sidebarService = inject(SidebarService);
  router = inject(Router);
  route = inject(ActivatedRoute);

  isOpen = false;  // Control de si la barra lateral está abierta

  activePath = '';

  openSubmenus: { [key: string]: boolean } = {
    security: false,
    parameters: false
  };

  private resizeListener?: () => void;

  user = {
    name: 'Vanessa Ortiz',
    email: 'vanessaortiz@gmail.com'
  };

  constructor() {
    // Usamos efecto para reaccionar a los cambios de estado de la barra lateral
    effect(() => {
      this.isOpen = this.sidebarService.sidebarOpen();
    });
  }

  ngOnInit() {
    // Obtener la ruta activa al inicializar
    this.activePath = this.router.url.split('/').pop() || '';

    this.resizeListener = () => {
      this.sidebarService.initializeBasedOnScreenSize();
    };
    window.addEventListener('resize', this.resizeListener);
  }

  ngOnDestroy() {
    if (this.resizeListener) {
      window.removeEventListener('resize', this.resizeListener);
    }
  }

  navigateTo(path: string) {
    // Navegar dentro de la ruta de cuenta
    this.router.navigate(['/account/' + path]);
    this.activePath = path;
    console.log("Navegando a:", '/account/' + path);
    
    // Cerrar la barra lateral en móvil después de navegar
    this.sidebarService.closeOnMobile();
  }

  toggleSubmenu(menu: string) {
    this.openSubmenus[menu] = !this.openSubmenus[menu];
  }
}
