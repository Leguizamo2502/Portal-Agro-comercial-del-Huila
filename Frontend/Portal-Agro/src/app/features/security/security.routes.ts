import { Routes } from '@angular/router';

export const SECURITY_ROUTES: Routes = [
  { path: '', redirectTo: 'form', pathMatch: 'full' },

  {
    path: 'form',
    children: [
      {
        path: '',
        title: 'Formularios',
        loadComponent: () =>
          import('./pages/form/form-list/form-list.component').then(m => m.FormListComponent),
      },
      {
        path: 'create',
        title: 'Crear formulario',
        loadComponent: () =>
          import('./pages/form/form-create/form-create.component').then(m => m.FormCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar formulario',
        loadComponent: () =>
          import('./pages/form/fomr-update/fomr-update.component').then(m => m.FomrUpdateComponent),
      },
    ],
  },
  

  {
    path: 'user',
    children: [
      {
        path: '',
        title: 'Usuarios',
        loadComponent: () =>
          import('./pages/user/user-list/user-list.component').then(m => m.UserListComponent),
      },
      {
        path: 'create',
        title: 'Crear usuario',
        loadComponent: () =>
          import('./pages/user/user-create/user-create.component').then(m => m.UserCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar usuario',
        loadComponent: () =>
          import('./pages/user/user-update/user-update.component').then(m => m.UserUpdateComponent),
      },
    ],
  },

  {
    path: 'rol',
    children: [
      {
        path: '',
        title: 'Roles',
        loadComponent: () =>
          import('./pages/rol/rol-list/rol-list.component').then(m => m.RolListComponent),
      },
      {
        path: 'create',
        title: 'Crear rol',
        loadComponent: () =>
          import('./pages/rol/rol-create/rol-create.component').then(m => m.RolCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar rol',
        loadComponent: () =>
          import('./pages/rol/rol-update/rol-update.component').then(m => m.RolUpdateComponent),
      },
    ],
  },

  {
    path: 'module',
    children: [
      {
        path: '',
        title: 'Módulos',
        loadComponent: () =>
          import('./pages/module/module-list/module-list.component').then(m => m.ModuleListComponent),
      },
      {
        path: 'create',
        title: 'Crear módulo',
        loadComponent: () =>
          import('./pages/module/module-create/module-create.component').then(m => m.ModuleCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar módulo',
        loadComponent: () =>
          import('./pages/module/module-update/module-update.component').then(m => m.ModuleUpdateComponent),
      },
    ],
  },

  {
    path: 'rolUser',
    children: [
      {
        path: '',
        title: 'Roles y Usuarios',
        loadComponent: () =>
          import('./pages/rolUser/rol-user-list/rol-user-list.component').then(m => m.RolUserListComponent),
      },
      {
        path: 'create',
        title: 'Crear Roles y Usuarios',
        loadComponent: () =>
          import('./pages/rolUser/rol-user-create/rol-user-create.component').then(m => m.RolUserCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar Roles y Usuarios',
        loadComponent: () =>
          import('./pages/rolUser/rol-user-update/rol-user-update.component').then(m => m.RolUserUpdateComponent),
      },
    ],
  },

    {
    path: 'formModule',
    children: [
      {
        path: '',
        title: 'Roles y Usuarios',
        loadComponent: () =>
          import('./pages/formModule/form-module-list/form-module-list.component').then(m => m.FormModuleListComponent),
      },
      {
        path: 'create',
        title: 'Crear Roles y Usuarios',
        loadComponent: () =>
          import('./pages/formModule/form-module-create/form-module-create.component').then(m => m.FormModuleCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar Roles y Usuarios',
        loadComponent: () =>
          import('./pages/formModule/form-module-update/form-module-update.component').then(m => m.FormModuleUpdateComponent),
      },
    ],
  },

  {
    path: 'permission',
    children: [
      {
        path: '',
        title: 'Permisos',
        loadComponent: () =>
          import('./pages/permission/permission-list/permission-list.component').then(m => m.PermissionListComponent),
      },
      {
        path: 'create',
        title: 'Crear permiso',
        loadComponent: () =>
          import('./pages/permission/permission-create/permission-create.component').then(m => m.PermissionCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar permiso',
        loadComponent: () =>
          import('./pages/permission/permission-update/permission-update.component').then(m => m.PermissionUpdateComponent),
      },
    ],
  },
];
