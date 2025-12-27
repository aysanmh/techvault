import { Component, inject, NgZone, OnDestroy, OnInit, signal } from '@angular/core';
import { OrderSummary } from '../../shared/components/order-summary/order-summary';
import { MatStepper, MatStepperModule } from '@angular/material/stepper';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatAnchor } from '@angular/material/button';
import { SnackbarService } from '../../core/services/SnackbarService';
import { StripeService } from '../../core/services/StripeService';
import {
  ConfirmationToken,
  StripeAddressElement,
  StripeAddressElementChangeEvent,
  StripePaymentElement,
  StripePaymentElementChangeEvent,
} from '@stripe/stripe-js';
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { Address } from '../../shared/models/user';
import { firstValueFrom } from 'rxjs';
import { AccountService } from '../../core/services/AccountService';
import { CheckoutDelivery } from './checkout-delivery/checkout-delivery';
import { CheckoutReview } from './checkout-review/checkout-review';
import { CartService } from '../../core/services/CartService';
import { CurrencyPipe } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-checkout',
  imports: [
    OrderSummary,
    MatStepperModule,
    FormsModule,
    RouterLink,
    MatAnchor,
    MatCheckboxModule,
    CheckoutDelivery,
    CheckoutReview,
    CurrencyPipe,
    MatProgressSpinnerModule,
    CommonModule
  ],
  templateUrl: './checkout.html',
  styleUrl: './checkout.css',
})
export class Checkout implements OnInit, OnDestroy {
  private stripeService = inject(StripeService);
  private snackbar = inject(SnackbarService);
  private accountService = inject(AccountService);
  private router = inject(Router);
  private zone = inject(NgZone);
  cartService = inject(CartService);

  addressElement?: StripeAddressElement;
  paymentElement?: StripePaymentElement;
  saveAddress = false;

  completionStatus = signal<{ address: boolean; card: boolean; delivery: boolean }>({
    address: false,
    card: false,
    delivery: false,
  });

  confirmationToken?: ConfirmationToken;
  loading = false;

  async ngOnInit() {
    try {
      await firstValueFrom(this.stripeService.createOrUpdatePaymentIntent());

      this.addressElement = await this.stripeService.createAddressElement();
      // Mount outside of mat-form-field to prevent Angular Material error
      this.addressElement.mount('#address-element');
      this.addressElement.on('change', this.handleAddressChange);

      this.paymentElement = await this.stripeService.createPaymentElement();
      // Mount outside of mat-form-field to prevent Angular Material error
      this.paymentElement.mount('#payment-element');
      this.paymentElement.on('change', this.handlePaymentChange);
    } catch (error: any) {
      this.snackbar.error(error.message);
    }
  }

  handleAddressChange = (event: StripeAddressElementChangeEvent) => {
    this.zone.run(() => {
      this.completionStatus.update(state => ({
        ...state,
        address: event.complete,
      }));
    });
  };

  handlePaymentChange = (event: StripePaymentElementChangeEvent) => {
    this.zone.run(() => {
      this.completionStatus.update(state => ({
        ...state,
        card: event.complete,
      }));
    });
  };

  handleDeliveryChange(event: boolean) {
    this.completionStatus.update(state => ({
      ...state,
      delivery: event,
    }));
  }

  async getConfirmationToken() {
    try {
      if (Object.values(this.completionStatus()).every(Boolean)) {
        const result = await this.stripeService.createConfirmationToken();
        if (result.error) throw new Error(result.error.message);
        this.confirmationToken = result.confirmationToken;
      }
    } catch (error: any) {
      this.snackbar.error(error.message);
    }
  }

  async onStepChange(event: StepperSelectionEvent) {
    if (event.selectedIndex === 1 && this.saveAddress) {
      const address = await this.getAddressFromStripeAddress();
      address && firstValueFrom(this.accountService.updateAddress(address));
    }

    if (event.selectedIndex === 3) {
      await this.getConfirmationToken();
    }
  }

  async confirmPayment(stepper: MatStepper) {
    this.loading = true;
    try {
      if (!this.confirmationToken) throw new Error('Missing confirmation token');

      const result = await this.stripeService.confirmPayment(this.confirmationToken);
      if (result.error) throw new Error(result.error.message);

      this.cartService.deleteCart();
      this.cartService.selectedDelivery.set(null);
      this.router.navigateByUrl('checkout/success');
    } catch (error: any) {
      this.snackbar.error(error.message || 'Something went wrong');
      stepper.previous();
    } finally {
      this.loading = false;
    }
  }

  private async getAddressFromStripeAddress(): Promise<Address | null> {
    const result = await this.addressElement?.getValue();
    const address = result?.value.address;

    if (!address) return null;

    return {
      line1: address.line1,
      line2: address.line2 || undefined,
      city: address.city,
      country: address.country,
      state: address.state,
      postalCard: address.postal_code,
    };
  }

  onSaveAddressCheckboxChange(event: MatCheckboxChange) {
    this.saveAddress = event.checked;
  }

  ngOnDestroy() {
    this.stripeService.disposeElements();
  }
}
