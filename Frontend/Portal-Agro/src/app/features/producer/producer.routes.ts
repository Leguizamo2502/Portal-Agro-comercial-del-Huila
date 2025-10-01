import { Routes } from '@angular/router';
import { roleActivateGuard } from '../../Core/guards/role-match/role-match.guard';

export const PRODUCER_ROUTES: Routes = [
  {
    path: '',
    title: 'Productor',
    canActivate: [roleActivateGuard],
    loadComponent: () =>
      import('./pages/producer-layout/producer-layout.component').then(
        (m) => m.ProducerLayoutComponent
      ),
    children: [
      { path: '', redirectTo: 'summary', pathMatch: 'full' },

      {
        path: 'summary',
        title: 'Resumen del productor',
        loadComponent: () =>
          import('./pages/summary/summary.component').then(
            (m) => m.SummaryComponent
          ),
      },

      {
        path: 'orders',
        title: 'Ordenes',
        loadComponent: () =>
          import(
            './pages/producer-orders-list/producer-orders-list.component'
          ).then((m) => m.ProducerOrdersListComponent),
      },
      {
        path: 'orders/:code',
        title: 'Detalle Orden',
        loadComponent: () =>
          import(
            './pages/producer-order-detail/producer-order-detail.component'
          ).then((m) => m.ProducerOrderDetailComponent),
      },

      {
        path: 'management',
        title: 'Gestión del productor',
        loadComponent: () =>
          import('./pages/management/management.component').then(
            (m) => m.ManagementComponent
          ),
        children: [
          { path: '', redirectTo: 'product', pathMatch: 'full' },

          {
            path: 'product',
            children: [
              {
                path: '',
                title: 'Productos',
                loadComponent: () =>
                  import(
                    './pages/product/product-list/product-list.component'
                  ).then((m) => m.ProductListComponent),
              },
              {
                path: 'create',
                title: 'Nuevo producto',
                loadComponent: () =>
                  import(
                    './pages/product/product-form/product-form.component'
                  ).then((m) => m.ProductFormComponent),
              },
              {
                path: 'update/:id',
                title: 'Editar producto',
                loadComponent: () =>
                  import(
                    './pages/product/product-form/product-form.component'
                  ).then((m) => m.ProductFormComponent),
              },
            ],
          },

          {
            path: 'farm',
            children: [
              {
                path: '',
                title: 'Fincas',
                loadComponent: () =>
                  import('./pages/farm/farm-list/farm-list.component').then(
                    (m) => m.FarmListComponent
                  ),
              },
              {
                path: 'create',
                title: 'Nueva finca',
                loadComponent: () =>
                  import('./pages/farm/farm-create/farm-create.component').then(
                    (m) => m.FarmCreateComponent
                  ),
              },
              {
                path: 'update/:id',
                title: 'Editar finca',
                loadComponent: () =>
                  import('./pages/farm/farm-form/farm-form.component').then(
                    (m) => m.FarmFormComponent
                  ),
              },
            ],
          },
        ],
      },
    ],
  },
];
