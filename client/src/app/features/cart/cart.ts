import { Component, inject } from '@angular/core';
import { CartService } from '../../core/services/CartService';
import { CommonModule } from '@angular/common';
import { CartItem } from "./cart-item/cart-item";
import { OrderSummary } from "../../shared/components/order-summary/order-summary";

@Component({
  selector: 'app-cart',
  imports: [
    CommonModule,
    CartItem, CartItem,
    OrderSummary
],
  templateUrl: './cart.html',
  styleUrl: './cart.css',
})
export class Cart {

  cartService = inject(CartService);

  
}

