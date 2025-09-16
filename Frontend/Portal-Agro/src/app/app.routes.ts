import { Routes } from '@angular/router';
import { authGuard } from './Core/guards/auth/guest.guard';
import { ForbiddenComponent } from './Core/page/forbidden/forbidden.component';
import { NotFoundComponent } from './Core/page/not-found/not-found.component';
import { FormChangePasswordComponent } from './features/account/components/form-change-password/form-change-password.component';
import { ProducerProfileComponent } from './features/producer-profile/producer-profile.component';
import { SummaryComponent } from './features/producer/pages/summary/summary.component';
import { FarmDetailComponent } from './features/farm-detail/farm-detail.component';
import { ContainerCardProductorComponent } from './shared/components/cards/container-card-productor/container-card-productor.component';
import { ProductDetailComponent } from './features/products/pages/product-detail/product-detail.component';
import { ButtonComponent } from './shared/components/button/button.component';
import { CardComponent } from './shared/components/cards/card/card.component';
import { CarruselComponent } from './shared/components/carrusel/carrusel.component';
import { DashboardComponent } from './shared/components/dashboard/dashboard.component';
import { MainLayoutComponent } from './shared/components/layouts/main-layout/main-layout.component';
import { NavbarBuenoComponent } from './shared/components/navs/navbar-bueno/navbar-bueno.component';



export const routes: Routes = [
  // Redirección inicial
  { path: '', redirectTo: 'auth/login', pathMatch: 'full' },

  // ===== RUTAS SIN LAYOUT =====
  {
    path: 'auth',
    // canMatch: [guestGuard],
    loadChildren: () =>
      import('./features/auth/auth.routes').then((m) => m.AUTH_ROUTES),
  },

  // ===== RUTAS CON LAYOUT =====
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      // Lazy modules que SÍ deben usar layout
      {
        path: 'home',
        // canMatch: [authGuard],
        loadChildren: () =>
          import('./features/home/home.routes').then((m) => m.HOME_ROUTES),
      },
      {
        path: 'account',
        canMatch: [authGuard],
        loadChildren: () =>
          import('./features/account/account.routes').then(
            (r) => r.ACCOUNT_ROUTES
          ),
      },
      { path: 'forbidden', component: ForbiddenComponent },
      { path: 'notFound', component: NotFoundComponent },

      // Demos / componentes sueltos (si quieres que usen el layout)
      { path: 'dashboard', component: DashboardComponent },
      { path: 'product-detail', component: ProductDetailComponent },
      { path: 'card', component: CardComponent },
      { path: 'boton', component: ButtonComponent },
      { path: 'navbar-bueno', component: NavbarBuenoComponent },
      { path: 'carrusel', component: CarruselComponent },
      { path: 'change', component: FormChangePasswordComponent },
      { path: 'producer/:code', component: ProducerProfileComponent },
      { path: 'summary', component: SummaryComponent },
      { path: 'farm-detail', component: FarmDetailComponent },
      { path: 'crm', component: ContainerCardProductorComponent }
    ],
  },

  // 404 (ajusta según tu app)
  { path: '**', redirectTo: 'notFound' },
];
