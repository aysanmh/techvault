import { Component, inject } from '@angular/core';
import { RouterLink } from "@angular/router";
import { MatAnchor } from "@angular/material/button";
import { MatFormField, MatLabel } from "@angular/material/select";
import { CartService } from '../../../core/services/CartService';
import { CurrencyPipe, Location } from '@angular/common';

@Component({
  selector: 'app-order-summary',
  imports: [RouterLink, MatAnchor, MatFormField, MatLabel , CurrencyPipe],
  templateUrl: './order-summary.html',
  styleUrl: './order-summary.css',
})
export class OrderSummary {

  cartService = inject(CartService);
  location = inject(Location);

}
