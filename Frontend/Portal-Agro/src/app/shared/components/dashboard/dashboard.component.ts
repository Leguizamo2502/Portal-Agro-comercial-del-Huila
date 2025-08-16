import { Component, inject, OnInit, OnDestroy } from '@angular/core';
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
  
  isOpen = this.sidebarService.isOpen;
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

  ngOnInit() {
    this.sidebarService.initializeBasedOnScreenSize();
    
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
    this.router.navigate([path], { relativeTo: this.route });
    this.activePath = path;
    console.log("Navegando a:", path);
  }

  toggleSubmenu(menu: string) {
    this.openSubmenus[menu] = !this.openSubmenus[menu];
  }
}