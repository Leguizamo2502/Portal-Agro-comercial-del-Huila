import { Routes } from '@angular/router';
import { authGuard } from './Core/guards/auth/guest.guard';
import { ForbiddenComponent } from './Core/page/forbidden/forbidden.component';
import { NotFoundComponent } from './Core/page/not-found/not-found.component';
import { MainLayoutComponent } from './shared/components/layouts/main-layout/main-layout.component';
import { DesignDecorationComponent } from './shared/components/design-decoration/design-decoration.component';

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
      // { path: 'dashboard', component: DashboardComponent },
      // { path: 'product-detail', component: ProductDetailComponent },
      // { path: 'card', component: CardComponent },
      // { path: 'boton', component: ButtonComponent },
      // { path: 'navbar-bueno', component: NavbarBuenoComponent },
      // { path: 'carrusel', component: CarruselComponent },
      // { path: 'change', component: FormChangePasswordComponent },
      // { path: 'producer/:code', component: ProducerProfileComponent },
      // { path: 'summary', component: SummaryComponent },
      // { path: 'farm-detail', component: FarmDetailComponent },
      // { path: 'crm', component: ContainerCardProductorComponent }
      {path: 'test-design', component: DesignDecorationComponent}
    ],
  },

  // 404 (ajusta según tu app)
  { path: '**', redirectTo: 'notFound' },
];
