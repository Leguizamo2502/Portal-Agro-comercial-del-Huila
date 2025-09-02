import { Routes } from '@angular/router';

// Páginas sin layout:
import { ForbiddenComponent } from './Core/page/forbidden/forbidden.component';

// Layout:
import { MainLayoutComponent } from './shared/components/layouts/main-layout/main-layout.component';

// “Showcase”/demos (si quieres que usen layout, van como hijos del layout):
import { ButtonComponent } from './shared/components/button/button.component';
import { NavbarVerticalComponent } from './shared/components/navs/navbar-vertical/navbar-vertical.component';
import { NavbarBuenoComponent } from './shared/components/navs/navbar-bueno/navbar-bueno.component';
import { CarruselComponent } from './shared/components/carrusel/carrusel.component';
import { CardComponent } from './shared/components/cards/card/card.component';
import { ProductDetailComponent } from './shared/components/product-detail/product-detail.component';
import { DashboardComponent } from './shared/components/dashboard/dashboard.component';
import { NotFoundComponent } from './Core/page/not-found/not-found.component';
import { authGuard, guestGuard } from './Core/guards/auth/guest.guard';
import { FormChangePasswordComponent } from './features/account/components/form-change-password/form-change-password.component';
import { ProducerProfileComponent } from './features/producer-profile/producer-profile.component';
import { SummaryComponent } from './features/producer/pages/summary/summary.component';
import { FarmDetailComponent } from './features/farm-detail/farm-detail.component';

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
      {path:'notFound',component:NotFoundComponent},

      // Demos / componentes sueltos (si quieres que usen el layout)
      { path: 'dashboard', component: DashboardComponent },
      { path: 'product-detail', component: ProductDetailComponent },
      { path: 'card', component: CardComponent },
      { path: 'boton', component: ButtonComponent },
      { path: 'navbar-vertical', component: NavbarVerticalComponent },
      { path: 'navbar-bueno', component: NavbarBuenoComponent },
      { path: 'carrusel', component: CarruselComponent },
      {path: 'change', component:FormChangePasswordComponent},
      { path: 'producer/:code', component: ProducerProfileComponent },
      {path: 'summary', component:SummaryComponent},
       {path: 'farm-detail', component:FarmDetailComponent}
    ],
  },

  // 404 (ajusta según tu app)
  // { path: '**', redirectTo: 'notFound' },
];
