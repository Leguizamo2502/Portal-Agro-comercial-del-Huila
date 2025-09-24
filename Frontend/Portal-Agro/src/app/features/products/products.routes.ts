import { Routes } from "@angular/router";
import { ProducerProfileComponent } from "../producer/pages/producer-profile/producer-profile.component";
import { ProductDetailComponent } from "./pages/product-detail/product-detail.component";
import { ProductComponent } from "./pages/product/product.component";


export const PRODUCTS_ROUTES: Routes=[
    {path:'',component:ProductComponent},
    {path:':id', component: ProductDetailComponent},
    {path: 'profile/:code', component: ProducerProfileComponent}
];