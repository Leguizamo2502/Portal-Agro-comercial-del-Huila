import { Component, inject, OnInit, OnDestroy, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { SidebarService } from '../../services/sidebar/sidebar.service';
import { AuthService } from '../../../Core/services/auth/auth.service';
import { UserSelectModel } from '../../../Core/Models/user.model';
import { HasRoleDirective } from '../../../Core/directives/has-role.directive';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, HasRoleDirective],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit, OnDestroy {
  sidebarService = inject(SidebarService);
  router = inject(Router);
  route = inject(ActivatedRoute);
  authService = inject(AuthService);
  user?: UserSelectModel;

  isOpen = false; // Control de si la barra lateral está abierta

  activePath = '';

  openSubmenus: { [key: string]: boolean } = {
    security: false,
    parameters: false,
  };

  private resizeListener?: () => void;

  loadUser(){
    this.authService.GetDataBasic().subscribe((data)=>{
      this.user= data;
    })
  }

  constructor() {
    // Usamos efecto para reaccionar a los cambios de estado de la barra lateral
    effect(() => {
      this.isOpen = this.sidebarService.sidebarOpen();
    });
    this.loadUser();
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
    this.router.navigate(['/account/' + path]);
    this.activePath = path;

    this.sidebarService.closeOnMobile();
  }

  toggleSubmenu(menu: string) {
    this.openSubmenus[menu] = !this.openSubmenus[menu];
  }
}
