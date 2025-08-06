import { Routes } from '@angular/router';
import { ButtonComponent } from './shared/components/button/button.component';
import { CardComponent } from './shared/components/card/card.component';
import { NavbarComponent } from './shared/components/navbar/navbar.component';
import { NavbarVerticalComponent } from './shared/components/navbar-vertical/navbar-vertical.component';
import { NavbarSinCategoriaComponent } from './shared/components/navbar-sin-categoria/navbar-sin-categoria.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'auth/login', 
    pathMatch: 'full',
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('./features/auth/auth.routes').then((m) => m.AUTH_ROUTES),
  },
  {
    path: '',
    loadChildren: () =>
      import('./features/products/products.routes').then((m) => m.PRODUCTS_ROUTES),
  },
  {
    path: 'home',
    loadChildren: () =>
      import('./features/home/home.routes').then((m) => m.HOME_ROUTES),
  },

  {
    path: 'account',
    loadChildren: ()=>
      import('./features/account/account.routes').then((r)=>r.ACCOUNT_ROUTES),
  },

  
  { path: 'card', component: CardComponent },
  { path: 'boton', component: ButtonComponent },
  { path: 'navbar', component: NavbarComponent },
  { path: 'navbar-vertical', component: NavbarVerticalComponent},
  { path: 'navbar-sin-categoria', component: NavbarSinCategoriaComponent},
];
