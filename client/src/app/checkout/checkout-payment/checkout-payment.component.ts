import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/basket.service';
import { CheckoutService } from '../checkout.service';
import { ToastrService } from 'ngx-toastr';
import { Basket } from 'src/app/shared/models/basket';
import { Address } from 'src/app/shared/models/user';
import { OrderToCreat } from 'src/app/shared/models/order';
import { NavigationExtras, Router } from '@angular/router';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent {
  @Input() checkoutForm?: FormGroup

  constructor(private basketService: BasketService, private checkoutService: CheckoutService, private toatr: ToastrService, private router: Router) { }

  submitOrder(){
    const basket = this.basketService.getCurrentBasketValue();
    if(!basket) return;
    const orderToCreat = this.getOrderToCreate(basket);
    if(!orderToCreat) return;
    this.checkoutService.createOrder(orderToCreat).subscribe({
      next: order => {
        this.toatr.success("Order created successfully!");
        this.basketService.deleteLocalBasket();
        const navigationExtras: NavigationExtras = {state: order};
        this.router.navigate(['checkout/success'], navigationExtras);
      }
    })
  }

  getOrderToCreate(basket: Basket){
    const deliveryMethodId = this.checkoutForm?.get("deliveryForm")?.get("deliveryMethod")?.value as number;
    const shipToAddress = this.checkoutForm?.get("addressForm")?.value as Address;
    if(!deliveryMethodId || !shipToAddress) return;
    return {
      basketId: basket.id,
      deliveryMethodId: deliveryMethodId,
      shipToAddress: shipToAddress
    } 
  }
}
