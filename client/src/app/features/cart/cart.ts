import { Component, inject } from '@angular/core';
import { CartService } from '../../core/services/CartService';
import { CommonModule } from '@angular/common';
import { CartItem } from "./cart-item/cart-item";
import { OrderSummary } from "../../shared/components/order-summary/order-summary";
import { EmptyState } from "../../shared/components/empty-state/empty-state";

@Component({
  selector: 'app-cart',
  imports: [
    CommonModule,
    CartItem, CartItem,
    OrderSummary,
    EmptyState
],
  templateUrl: './cart.html',
  styleUrl: './cart.css',
})
export class Cart {

  cartService = inject(CartService);

  
}

