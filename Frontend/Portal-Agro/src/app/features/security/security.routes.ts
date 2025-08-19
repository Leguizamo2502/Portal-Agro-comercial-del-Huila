import { Routes } from '@angular/router';
import { permissionGuard } from '../../Core/guards/permission.guard';

export const SECURITY_ROUTES: Routes = [
  { path: '', redirectTo: 'form', pathMatch: 'full' },

  {
    path: 'form',
    canActivateChild: [permissionGuard],
    runGuardsAndResolvers: 'always',
    data: { permissionKey: '/account/security/form' }, 
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
        data: { perm: 'crear' },  
        loadComponent: () =>
          import('./pages/form/form-create/form-create.component').then(m => m.FormCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar formulario',
        data: { perm: 'actualizar' }, 
        loadComponent: () =>
          import('./pages/form/fomr-update/fomr-update.component').then(m => m.FomrUpdateComponent),
      },
    ],
  },
  

  {
    path: 'user',
    canActivateChild: [permissionGuard],
    runGuardsAndResolvers: 'always',
    data: { permissionKey: '/account/security/user' }, 

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
        data: { perm: 'crear' },  
        loadComponent: () =>
          import('./pages/user/user-create/user-create.component').then(m => m.UserCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar usuario',
        data: { perm: 'actualizar' }, 
        loadComponent: () =>
          import('./pages/user/user-update/user-update.component').then(m => m.UserUpdateComponent),
      },
    ],
  },

  {
    path: 'rol',
    canActivateChild: [permissionGuard],
    runGuardsAndResolvers: 'always',
    data: { permissionKey: '/account/security/rol' }, 
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
        data: { perm: 'crear' },  
        loadComponent: () =>
          import('./pages/rol/rol-create/rol-create.component').then(m => m.RolCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar rol',
        data: { perm: 'actualizar' },
        loadComponent: () =>
          import('./pages/rol/rol-update/rol-update.component').then(m => m.RolUpdateComponent),
      },
    ],
  },

  {
    path: 'module',
    canActivateChild: [permissionGuard],
    runGuardsAndResolvers: 'always',
    data: { permissionKey: '/account/security/module' }, 
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
        data: { perm: 'crear' },  
        loadComponent: () =>
          import('./pages/module/module-create/module-create.component').then(m => m.ModuleCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar módulo',
        data: { perm: 'actualizar' },
        loadComponent: () =>
          import('./pages/module/module-update/module-update.component').then(m => m.ModuleUpdateComponent),
      },
    ],
  },

  {
    path: 'rolUser',
    canActivateChild: [permissionGuard],
    runGuardsAndResolvers: 'always',
    data: { permissionKey: '/account/security/rolUser' }, 
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
        data: { perm: 'crear' },  
        loadComponent: () =>
          import('./pages/rolUser/rol-user-create/rol-user-create.component').then(m => m.RolUserCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar Roles y Usuarios',
        data: { perm: 'actualizar' },
        loadComponent: () =>
          import('./pages/rolUser/rol-user-update/rol-user-update.component').then(m => m.RolUserUpdateComponent),
      },
    ],
  },

    {
    path: 'formModule',
    canActivateChild: [permissionGuard],
    runGuardsAndResolvers: 'always',
    data: { permissionKey: '/account/security/formModule' }, 
    children: [
      {
        path: '',
        title: 'Modulos y Formularios',
        loadComponent: () =>
          import('./pages/formModule/form-module-list/form-module-list.component').then(m => m.FormModuleListComponent),
      },
      {
        path: 'create',
        title: 'Crear Modulos y Formularios',
        data: { perm: 'crear' },  
        loadComponent: () =>
          import('./pages/formModule/form-module-create/form-module-create.component').then(m => m.FormModuleCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar Modulos y Formularios',
        data: { perm: 'actualizar' },
        loadComponent: () =>
          import('./pages/formModule/form-module-update/form-module-update.component').then(m => m.FormModuleUpdateComponent),
      },
    ],
  },

  {
    path: 'rolFormPermission',
    canActivateChild: [permissionGuard],
    runGuardsAndResolvers: 'always',
    data: { permissionKey: '/account/security/rolFormPermission' }, 
    children: [
      {
        path: '',
        title: 'Rol, Formulario y Permiso',
        loadComponent: () =>
          import('./pages/rolFormPermission/rol-form-permission-list/rol-form-permission-list.component').then(m => m.RolFormPermissionListComponent),
      },
      {
        path: 'create',
        title: 'Crear Rol, Formulario y Permiso',
        data: { perm: 'crear' },  
        loadComponent: () =>
          import('./pages/rolFormPermission/rol-form-permission-create/rol-form-permission-create.component').then(m => m.RolFormPermissionCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar Rol, Formulario y Permiso',
        data: { perm: 'actualizar' },
        loadComponent: () =>
          import('./pages/rolFormPermission/rol-form-permission-update/rol-form-permission-update.component').then(m => m.RolFormPermissionUpdateComponent),
      },
    ],
  },

  {
    path: 'permission',
    canActivateChild: [permissionGuard],
    runGuardsAndResolvers: 'always',
    data: { permissionKey: '/account/security/permission' }, 
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
        data: { perm: 'crear' },  
        loadComponent: () =>
          import('./pages/permission/permission-create/permission-create.component').then(m => m.PermissionCreateComponent),
      },
      {
        path: 'update/:id',
        title: 'Editar permiso',
        data: { perm: 'actualizar' }, 
        loadComponent: () =>
          import('./pages/permission/permission-update/permission-update.component').then(m => m.PermissionUpdateComponent),
      },
    ],
  },
];
