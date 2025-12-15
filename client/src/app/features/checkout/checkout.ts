import { Component, inject } from '@angular/core';
import { OrderSummary } from "../../shared/components/order-summary/order-summary";
import { MatStepperModule } from '@angular/material/stepper';
import { FormsModule } from '@angular/forms';
import { RouterLink } from "@angular/router";
import { MatAnchor } from "@angular/material/button";
import { PaymentService } from '../../core/services/PaymentService';
import { SnackbarService } from '../../core/services/SnackbarService';

@Component({
  selector: 'app-checkout',
  imports: [
    OrderSummary,
    MatStepperModule,
    FormsModule,
    RouterLink,
    MatAnchor
  ],
  templateUrl: './checkout.html',
  styleUrl: './checkout.css',
})
export class Checkout {
  private paymentService = inject(PaymentService);
  private snackbar = inject(SnackbarService);

  address = {
    fullName: '',
    street: '',
    city: '',
    state: '',
    zip: '',
    country: ''
  };
}
