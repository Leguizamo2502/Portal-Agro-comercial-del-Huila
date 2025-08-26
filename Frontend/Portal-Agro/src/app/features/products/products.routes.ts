import { Routes } from "@angular/router";
import { ProductDetailComponent } from "../../shared/components/product-detail/product-detail.component";
import { ProductComponent } from "./pages/product/product.component";

export const PRODUCTS_ROUTES: Routes=[
    {path:'product',component:ProductComponent},
    {path:':id', component: ProductDetailComponent},
];