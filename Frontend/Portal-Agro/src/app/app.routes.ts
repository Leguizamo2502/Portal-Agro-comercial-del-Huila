import { Routes } from '@angular/router';
import { ButtonComponent } from './shared/components/button/button.component';
import { CardComponent } from './shared/components/card/card.component';
import { NavbarComponent } from './shared/components/navbar/navbar.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'Auth/login', 
    pathMatch: 'full',
  },
  {
    path: 'Auth',
    loadChildren: () =>
      import('./features/auth/auth.routes').then((m) => m.AUTH_ROUTES),
  },
  {
    path: '',
    loadChildren: () =>
      import('./features/products/products.routes').then((m) => m.PRODUCTS_ROUTES),
  },
  {
    path: 'Home',
    loadChildren: () =>
      import('./features/home/home.routes').then((m) => m.HOME_ROUTES),
  },


  { path: 'card', component: CardComponent },
  { path: 'boton', component: ButtonComponent },
  { path: 'navbar', component: NavbarComponent },
];
