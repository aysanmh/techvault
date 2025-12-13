import { Component, inject, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CartItemType } from '../../../shared/models/cart';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { CurrencyPipe } from '@angular/common';
import { CartService } from '../../../core/services/CartService';

@Component({
  selector: 'app-cart-item',
  imports: [RouterLink, MatButtonModule, MatIcon,CurrencyPipe],
  templateUrl: './cart-item.html',
  styleUrl: './cart-item.css',
})
export class CartItem {

  item = input.required<CartItemType>();

  cartService = inject(CartService);

  incrementQuantity(){

    this.cartService.addItemToCart(this.item());

  }
  decrementQuantity(){

    this.cartService.removeItemFromCart(this.item().deviceId);
  }

  removeItemFromCart(){
    this.cartService.removeItemFromCart(this.item().deviceId, this.item().quantity);
  }
}
