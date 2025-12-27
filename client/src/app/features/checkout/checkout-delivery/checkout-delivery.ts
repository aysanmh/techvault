import { Component, inject, OnInit, Output, EventEmitter } from '@angular/core';
import { CheckoutService } from '../../../core/services/CheckoutService';
import { CartService } from '../../../core/services/CartService';
import { MatRadioModule } from '@angular/material/radio';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { DeliveryMethod } from '../../../shared/models/deliveryMethod';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-checkout-delivery',
  templateUrl: './checkout-delivery.html',
  styleUrls: ['./checkout-delivery.css'],
  standalone: true,
  imports: [MatRadioModule, CurrencyPipe,CommonModule]
})
export class CheckoutDelivery implements OnInit {

  checkoutService = inject(CheckoutService);
  cartService = inject(CartService);

  @Output() deliveryComplete = new EventEmitter<boolean>();

  deliveryMethods: DeliveryMethod[] = [];
  selectedDelivery: DeliveryMethod | null = null;

  ngOnInit(): void {
    this.checkoutService.getDeliveryMethods().pipe(take(1)).subscribe(methods => {
      this.deliveryMethods = methods;
      const selectedId = this.cartService.cart()?.deliveryMethodId;
      if (selectedId) {
        const method = methods.find(x => x.id === selectedId);
        if (method) {
          this.selectedDelivery = method;
          this.cartService.selectedDelivery.set(method);
          this.deliveryComplete.emit(true);
        }
      }
    });
  }

  updateDeliveryMethod(method: DeliveryMethod) {
    this.selectedDelivery = method;
    const cart = this.cartService.cart();
    if (cart) {
      cart.deliveryMethodId = method.id;
      this.cartService.setCart(cart);
      this.cartService.selectedDelivery.set(method);
      this.deliveryComplete.emit(true);
    }
  }

  trackByMethodId(index: number, method: DeliveryMethod) {
    return method.id;
  }
}
