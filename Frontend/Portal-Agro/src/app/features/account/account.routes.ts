
import { Routes } from "@angular/router";
import { FormListComponent } from "../security/pages/form/form-list/form-list.component";
import { AccountComponent } from "./pages/account/account.component";
import { ModuleListComponent } from "../security/pages/module/module-list/module-list.component";
import { PermissionListComponent } from "../security/pages/permission/permission-list/permission-list.component";
import { FomrUpdateComponent } from "../security/pages/form/fomr-update/fomr-update.component";
import { FormCreateComponent } from "../security/pages/form/form-create/form-create.component";

export const ACCOUNT_ROUTES: Routes=[
    // {path:'info', component: LoginComponent},
    // {path: 'register', component: RegisterComponent}
    // {path:'',component:AccountComponent},
    {path: '',
    component: AccountComponent,
    children: [
      { path: 'security/form', component: FormListComponent },
      { path: 'security/form/update/:id', component: FomrUpdateComponent },
      { path: 'security/form/create', component: FormCreateComponent },


      { path: 'security/module', component: ModuleListComponent },
      { path: 'security/permission', component: PermissionListComponent },
      
    //   { path: 'security/form', component: FormListComponent },



    //   { path: '', redirectTo: 'security/form', pathMatch: 'full' }
    ]}
];