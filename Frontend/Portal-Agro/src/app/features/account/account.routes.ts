import { Routes, CanActivateFn } from '@angular/router';
import { InfoComponent } from './components/info/info.component';
import { SummaryComponent } from '../producer/pages/summary/summary.component';
import { AccountComponent } from './pages/account/account.component';
import { roleMatchGuard } from '../../Core/guards/role-match/role-match.guard';

export const ACCOUNT_ROUTES: Routes = [
  {
    path: '',
    component: AccountComponent,
    children: [
      // default
      { path: '', redirectTo: 'info', pathMatch: 'full' },
      // Home info
      { path: 'info', component: InfoComponent },

      // --- PRODUCER ---
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
          import('../parameters/parameters.routes').then(
            (m) => m.PARAMETERS_ROUTES
          ),
      },

    ],
  },
];
