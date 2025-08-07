
import { Routes } from "@angular/router";
import { FormListComponent } from "../security/pages/form/form-list/form-list.component";
import { AccountComponent } from "./pages/account/account.component";
import { ModuleListComponent } from "../security/pages/module/module-list/module-list.component";
import { PermissionListComponent } from "../security/pages/permission/permission-list/permission-list.component";
import { FomrUpdateComponent } from "../security/pages/form/fomr-update/fomr-update.component";
import { FormCreateComponent } from "../security/pages/form/form-create/form-create.component";
import { PermissionCreateComponent } from "../security/pages/permission/permission-create/permission-create.component";
import { PermissionUpdateComponent } from "../security/pages/permission/permission-update/permission-update.component";
import { CategoryListComponent } from "../parameters/pages/category/category-list/category-list.component";
import { CategoryCreateComponent } from "../parameters/pages/category/category-create/category-create.component";
import { CategoryUpdateComponent } from "../parameters/pages/category/category-update/category-update.component";
import { ModuleUpdateComponent } from "../security/pages/module/module-update/module-update.component";
import { ModuleCreateComponent } from "../security/pages/module/module-create/module-create.component";
import { InfoComponent } from "./components/info/info.component";
import { DeparmentListComponent } from "../parameters/pages/department/deparment-list/deparment-list.component";
import { CityListComponent } from "../parameters/pages/city/city-list/city-list.component";

export const ACCOUNT_ROUTES: Routes=[
    // {path:'info', component: LoginComponent},
    // {path: 'register', component: RegisterComponent}
    // {path:'',component:AccountComponent},
    {path: '',
    component: AccountComponent,
    children: [

      //account
      {path:'info',component:InfoComponent},

      //security
      { path: 'security/form', component: FormListComponent },
      { path: 'security/form/update/:id', component: FomrUpdateComponent },
      { path: 'security/form/create', component: FormCreateComponent },

      { path: 'security/module/update/:id', component: ModuleUpdateComponent },
      { path: 'security/module/create', component: ModuleCreateComponent },


      { path: 'security/module', component: ModuleListComponent },
      { path: 'security/permission', component: PermissionListComponent },
      { path: 'security/permission/create', component: PermissionCreateComponent },
      { path: 'security/permission/update/:id', component: PermissionUpdateComponent },


      //Parameters

      {path:'parameters/category',component:CategoryListComponent},
      {path:'parameters/category/create',component:CategoryCreateComponent},
      {path:'parameters/category/update/:id',component:CategoryUpdateComponent},

      {path:'parameters/department',component:DeparmentListComponent},

      {path:'parameters/city',component:CityListComponent},





      
    //   { path: 'security/form', component: FormListComponent },



      { path: '', redirectTo: 'info', pathMatch: 'full' }
    ]}
];