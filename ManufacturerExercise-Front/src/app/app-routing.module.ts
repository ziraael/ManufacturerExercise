import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrdersDashboardComponent } from './core/components/orders-dashboard/orders-dashboard.component';
import { CreateOrderComponent } from './core/components/create-order/create-order.component';
import { AddProductComponent } from './core/components/add-product/add-product.component';

const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: OrdersDashboardComponent },
  { path: 'create-order', component: CreateOrderComponent },
  { path: 'add-product', component: AddProductComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
