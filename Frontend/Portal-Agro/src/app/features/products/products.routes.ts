import { Routes } from "@angular/router";
import { ProductDetailComponent } from "../../shared/components/product-detail/product-detail.component";

export const PRODUCTS_ROUTES: Routes=[
    {path:':id', component: ProductDetailComponent},
];