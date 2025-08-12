import { Routes } from '@angular/router';

export const PRODUCER_ROUTES: Routes = [
  {
    path: '',
    title: 'Productor',
    loadComponent: () =>
      import('./pages/producer-layout/producer-layout.component')
        .then(m => m.ProducerLayoutComponent),
    children: [
      { path: '', redirectTo: 'summary', pathMatch: 'full' },

      {
        path: 'summary',
        title: 'Resumen del productor',
        loadComponent: () =>
          import('./pages/summary/summary.component')
            .then(m => m.SummaryComponent),
      },

      {
        path: 'management',
        title: 'Gestión del productor',
        loadComponent: () =>
          import('./pages/management/management.component')
            .then(m => m.ManagementComponent),
        children: [
          { path: '', redirectTo: 'product', pathMatch: 'full' },

          {
            path: 'product',
            children: [
              {
                path: '',
                title: 'Productos',
                loadComponent: () =>
                  import('./pages/product/product-list/product-list.component')
                    .then(m => m.ProductListComponent),
              },
              {
                path: 'create',
                title: 'Nuevo producto',
                loadComponent: () =>
                  import('./pages/product/product-create/product-create.component')
                    .then(m => m.ProductCreateComponent),
              },
              // {
              //   path: 'update/:id',
              //   title: 'Editar producto',
              //   loadComponent: () =>
              //     import('./pages/product/product-update/product-update.component')
              //       .then(m => m.ProductUpdateComponent),
              // },
            ],
          },

          {
            path: 'farm',
            children: [
              {
                path: '',
                title: 'Fincas',
                loadComponent: () =>
                  import('./pages/farm/farm-list/farm-list.component')
                    .then(m => m.FarmListComponent),
              },
              {
                path: 'create',
                title: 'Nueva finca',
                loadComponent: () =>
                  import('./pages/farm/farm-create/farm-create.component')
                    .then(m => m.FarmCreateComponent),
              },
              {
                path: 'update/:id',
                title: 'Editar finca',
                loadComponent: () =>
                  import('./pages/farm/farm-update/farm-update.component')
                    .then(m => m.FarmUpdateComponent),
              },
            ],
          },
        ],
      },
    ],
  },
];
