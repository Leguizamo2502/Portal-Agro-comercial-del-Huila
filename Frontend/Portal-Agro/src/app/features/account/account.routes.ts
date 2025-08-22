import { Routes } from '@angular/router';
import { InfoComponent } from './components/info/info.component';

export const ACCOUNT_ROUTES: Routes = [
  {
    path: '',
    // ❌ NO component: MainLayoutComponent aquí (ya está en el nivel superior)
    children: [
      { path: '', redirectTo: 'info', pathMatch: 'full' },
      { path: 'info', component: InfoComponent },
      {
        path: 'producer',
        loadChildren: () =>
          import('../producer/producer.routes').then((m) => m.PRODUCER_ROUTES),
      },
      {
        path: 'security',
        loadChildren: () =>
          import('../security/security.routes').then((m) => m.SECURITY_ROUTES),
      },
      {
        path: 'parameters',
        loadChildren: () =>
          import('../parameters/parameters.routes').then((m) => m.PARAMETERS_ROUTES),
      },
    ],
  },
];