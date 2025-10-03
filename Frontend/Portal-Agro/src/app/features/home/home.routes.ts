import { Routes } from "@angular/router";
import { HomeComponent } from "./pages/home/home.component";

export const 
HOME_ROUTES: Routes=[
    {path:'',redirectTo:'',pathMatch:'full'},
    
    {path:'', component: HomeComponent},
    {path: 'product', loadChildren: () => import('../products/products.routes').then(m => m.PRODUCTS_ROUTES)}

];