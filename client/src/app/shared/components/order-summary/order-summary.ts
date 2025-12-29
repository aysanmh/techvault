import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatAnchor } from '@angular/material/button';
import { MatFormField, MatLabel } from '@angular/material/select';
import { CartService } from '../../../core/services/CartService';
import { CommonModule, CurrencyPipe, Location } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-order-summary',
  imports: [
    RouterLink,
    MatAnchor,
    MatFormField,
    MatLabel,
    CurrencyPipe,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    FormsModule,
    CommonModule,
  ],
  templateUrl: './order-summary.html',
  styleUrl: './order-summary.css',
})
export class OrderSummary {
  cartService = inject(CartService);
  location = inject(Location);
}
