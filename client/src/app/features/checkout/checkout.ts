import { Component, inject, NgZone, OnDestroy, OnInit, signal } from '@angular/core';
import { OrderToCreate, ShippingAddress } from '../../shared/models/order';
import { CartService } from '../../core/services/CartService';
import { StripeService } from '../../core/services/StripeService';
import { AccountService } from '../../core/services/AccountService';
import { OrderService } from '../../core/services/OrderService';
import { SnackbarService } from '../../core/services/SnackbarService';
import { Router, RouterLink } from '@angular/router';
import { MatStepper, MatStepperModule } from '@angular/material/stepper';
import { CheckoutDelivery } from './checkout-delivery/checkout-delivery';
import { CheckoutReview } from './checkout-review/checkout-review';
import { OrderSummary } from '../../shared/components/order-summary/order-summary';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe, CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import {
  ConfirmationToken,
  StripeAddressElement,
  StripeAddressElementChangeEvent,
  StripePaymentElement,
  StripePaymentElementChangeEvent,
} from '@stripe/stripe-js';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { MatAnchor } from '@angular/material/button';
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox';
import { firstValueFrom } from 'rxjs';
import { SignalrService } from '../../core/services/SignalrService';

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
    CommonModule,
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
  private orderService = inject(OrderService);
  private signalrService = inject(SignalrService);
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
      const cart = await firstValueFrom(this.stripeService.createOrUpdatePaymentIntent());
      if (!cart.clientSecret) throw new Error('PaymentIntent clientSecret missing');

      this.cartService.setCart(cart);

      if (!this.addressElement) {
        this.addressElement = await this.stripeService.createAddressElement();
        this.addressElement.mount('#address-element');
        this.addressElement.on('change', this.handleAddressChange);
      }

      if (!this.paymentElement) {
        this.paymentElement = await this.stripeService.createPaymentElement();
        this.paymentElement.mount('#payment-element');
        this.paymentElement.on('change', this.handlePaymentChange);
      }
    } catch (error: any) {
      this.snackbar.error(error.message);
    }
  }

  handleAddressChange = (event: StripeAddressElementChangeEvent) => {
    this.zone.run(() => {
      this.completionStatus.update((state) => ({
        ...state,
        address: event.complete,
      }));
    });
  };

  handlePaymentChange = (event: StripePaymentElementChangeEvent) => {
    this.zone.run(() => {
      this.completionStatus.update((state) => ({
        ...state,
        card: event.complete,
      }));
    });
  };

  handleDeliveryChange(method: any) {
    this.zone.run(() => {
      this.completionStatus.update((state) => ({
        ...state,
        delivery: true,
      }));
    });

    const cart = this.cartService.cart();
    if (cart && method?.id) {
      cart.deliveryMethodId = method.id;
      this.cartService.setCart(cart);
    }
  }

  async getConfirmationToken() {
    if (!Object.values(this.completionStatus()).every(Boolean)) return;
    const result = await this.stripeService.createConfirmationToken();
    this.confirmationToken = result.confirmationToken;
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

      const order = await this.createOrderModel();
      console.log('Order payload:', order);

      const orderResult = await firstValueFrom(this.orderService.createOrder(order));
      if (!orderResult) throw new Error('Order creation failed');

      this.orderService.orderComplete = true;
      this.cartService.deleteCart();
      this.cartService.selectedDelivery.set(null);

      this.signalrService.orderSignal.set(orderResult);

      this.router.navigateByUrl('checkout/success');
    } catch (error: any) {
      this.snackbar.error(error.message || 'Something went wrong');
      stepper.previous();
    } finally {
      this.loading = false;
    }
  }

  private async createOrderModel(): Promise<OrderToCreate> {
    const cart = this.cartService.cart();
    const shippingAddress = (await this.getAddressFromStripeAddress()) as ShippingAddress;
    const card = this.confirmationToken?.payment_method_preview.card;

    if (!cart?.id || !cart.deliveryMethodId || !card || !shippingAddress)
      throw new Error('Problem creating order');

    return {
      cartId: cart.id,
      paymentSummary: {
        last4: parseInt(card.last4, 10),
        brand: card.brand,
        expMonth: card.exp_month,
        expYear: card.exp_year,
      },
      deliveryMethodId: cart.deliveryMethodId,
      shippingAddress,
    };
  }

  private async getAddressFromStripeAddress() {
    const result = await this.addressElement?.getValue();
    const address = result?.value.address;
    if (!address) return null;
    return {
      name: result.value.name,
      line1: address.line1,
      line2: address.line2 || undefined,
      city: address.city,
      state: address.state,
      postalCode: address.postal_code,
      country: address.country,
    };
  }

  onSaveAddressCheckboxChange(event: MatCheckboxChange) {
    this.saveAddress = event.checked;
  }

  ngOnDestroy() {
    this.stripeService.disposeElements();
  }
}
