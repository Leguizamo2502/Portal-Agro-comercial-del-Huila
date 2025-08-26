import { Routes } from "@angular/router";
import { LoginComponent } from "./pages/login/login.component";
import { RegisterComponent } from "./pages/register/register.component";
import { RecoverPasswordComponent } from "./pages/recover-password/recover-password.component";

export const AUTH_ROUTES: Routes=[
    {path:'login', component: LoginComponent},
    {path: 'register', component: RegisterComponent},
    {path: 'recover-password', component:RecoverPasswordComponent},
    { path: '', pathMatch: 'full', redirectTo: 'login' }
];