import { NavbarVerticalComponent } from './shared/components/navbar-vertical/navbar-vertical.component';
import { Routes } from '@angular/router';
import { ButtonComponent } from './shared/components/button/button.component';
import { NavbarComponent } from './shared/components/navbar/navbar.component';
import { NavbarSinCategoriaComponent } from './shared/components/navbar-sin-categoria/navbar-sin-categoria.component';
export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login', 
    pathMatch: 'full',
  },
  {
    path: '',
    loadChildren: () =>
      import('./features/auth/auth.routes').then((m) => m.AUTH_ROUTES),
  },
  {
    path: '',
    loadChildren: () =>
      import('./features/products/products.routes').then((m) => m.PRODUCTS_ROUTES),
  },


  { path: 'boton', component: ButtonComponent },
  { path: 'navbar', component: NavbarComponent },
  { path: 'navbar-vertical', component: NavbarVerticalComponent },
  { path: 'navbar-sinCategoria', component: NavbarSinCategoriaComponent },
];
