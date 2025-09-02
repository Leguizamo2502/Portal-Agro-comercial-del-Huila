import { Routes } from "@angular/router";
import { ProductDetailComponent } from "../../shared/components/product-detail/product-detail.component";
import { ProductComponent } from "./pages/product/product.component";
import { ProducerProfileComponent } from "../producer-profile/producer-profile.component";

export const PRODUCTS_ROUTES: Routes=[
    {path:'',component:ProductComponent},
    {path:':id', component: ProductDetailComponent},
    {path: 'profile/:code', component: ProducerProfileComponent}
];