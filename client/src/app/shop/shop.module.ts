import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { SharedModule } from '../shared/shared.module';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { ShopRoutingModule } from './shop-routing.module';
import { ProductItemComponent } from './product-item/product-item.component';


@NgModule({
  declarations: [
    ShopComponent,
    ProductDetailsComponent,
    ProductItemComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ShopRoutingModule
  ]
})
export class ShopModule { }
