import { Routes } from '@angular/router';

export const PARAMETERS_ROUTES: Routes = [
  { path: '', redirectTo: 'category', pathMatch: 'full' },

  {
    path: 'category',
    children: [
      {
        path: '',
        title: 'Categorías',
        loadComponent: () =>
          import('./pages/category/category-list/category-list.component').then(m => m.CategoryListComponent),
      },
      {
        path: 'create',
        title: 'Crear categoría',
        loadComponent: () =>
          import('./pages/category/category-create/category-create.component').then(m => m.CategoryCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar categoría',
        loadComponent: () =>
          import('./pages/category/category-update/category-update.component').then(m => m.CategoryUpdateComponent),
      },
    ],
  },

  {
    path: 'department',
    children: [
      {
        path: '',
        title: 'Departamentos',
        loadComponent: () =>
          import('./pages/department/deparment-list/deparment-list.component').then(m => m.DeparmentListComponent),
      },
      {
        path: 'create',
        title: 'Crear departamento',
        loadComponent: () =>
          import('./pages/department/deparment-create/deparment-create.component').then(m => m.DepartmentCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar departamento',
        loadComponent: () =>
          import('./pages/department/department-update/department-updte.component').then(m => m.DepartmentUpdateComponent),
      },
    ],
  },

  {
    path: 'city',
    children: [
      {
        path: '',
        title: 'Ciudades',
        loadComponent: () =>
          import('./pages/city/city-list/city-list.component').then(m => m.CityListComponent),
      },
      {
        path: 'create',
        title: 'Crear ciudad',
        loadComponent: () =>
          import('./pages/city/city-create/city-create.component').then(m => m.CityCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar ciudad',
        loadComponent: () =>
          import('./pages/city/city-update/city-update.component').then(m => m.CityUpdateComponent),
      },
    ],
  },
];
