import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ShopComponent } from './shop.component';
import { ProductDetailsComponent } from './product-details/product-details.component';

//lazey loading for routing is activated
const routes: Routes = [
  { path: '', component: ShopComponent },
  //using an alias that breadcrumb will get the name of product from it
  { path: ':id', component: ProductDetailsComponent, data:{breadcrumb: {alias: 'productDetails'}}},
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ShopRoutingModule {}
