import { Routes } from '@angular/router';
import { NavbarComponent } from './shared/shared/components/navbar/navbar.component';
import { ButtonComponent } from './shared/shared/components/button/button.component';
import { CardComponent } from './shared/shared/components/card/card.component';

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


  { path: 'card', component: CardComponent },
  { path: 'boton', component: ButtonComponent },
  { path: 'navbar', component: NavbarComponent },
];
