import { inject, Injectable } from '@angular/core';
import {
  ConfirmationToken,
  loadStripe,
  Stripe,
  StripeAddressElement,
  StripeAddressElementOptions,
  StripeElements,
  StripePaymentElement,
} from '@stripe/stripe-js';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CartService } from './CartService';
import { Cart } from '../../shared/models/cart';
import { firstValueFrom, map } from 'rxjs';
import { AccountService } from './AccountService';

@Injectable({ providedIn: 'root' })
export class StripeService {
  baseUrl = environment.apiUrl;

  private http = inject(HttpClient);
  private cartService = inject(CartService);
  private accountService = inject(AccountService);

  private stripePromise: Promise<Stripe | null> = loadStripe(environment.stripePublicKey);
  private elements?: StripeElements;
  private addressElement?: StripeAddressElement;
  private paymentElement?: StripePaymentElement;

  async getStripeInstance() {
    return this.stripePromise;
  }

  async initializeElements() {
    if (this.elements) return this.elements;

    const stripe = await this.getStripeInstance();
    const cart = this.cartService.cart();

    if (!stripe || !cart?.clientSecret) {
      throw new Error('Stripe not initialized');
    }

    this.elements = stripe.elements({
      clientSecret: cart.clientSecret,
      appearance: { labels: 'floating' },
    });

    return this.elements;
  }

  async createPaymentElement() {
    if (!this.paymentElement) {
      const elements = await this.initializeElements();
      this.paymentElement = elements.create('payment');
    }
    return this.paymentElement;
  }

  async createAddressElement() {
    if (!this.addressElement) {
      const elements = await this.initializeElements();
      const user = this.accountService.currentUser();

      let defaultValues: StripeAddressElementOptions['defaultValues'] = {};

      if (user) {
        defaultValues.name = `${user.firstName} ${user.lastName}`;
      }

      if (user?.address) {
        defaultValues.address = {
          line1: user.address.line1,
          line2: user.address.line2,
          city: user.address.city,
          state: user.address.state,
          country: user.address.country,
          postal_code: user.address.postalCard,
        };
      }

      this.addressElement = elements.create('address', {
        mode: 'shipping',
        defaultValues,
      });
    }

    return this.addressElement;
  }

  async createConfirmationToken() {
    const stripe = await this.getStripeInstance();
    const elements = await this.initializeElements();

    const submitResult = await elements.submit();
    if (submitResult.error) throw new Error(submitResult.error.message);

    if (!stripe) throw new Error('Stripe not available');

    return stripe.createConfirmationToken({ elements });
  }

  async confirmPayment(confirmationToken: ConfirmationToken) {
    const stripe = await this.getStripeInstance();
    const clientSecret = this.cartService.cart()?.clientSecret;

    if (!stripe || !clientSecret) {
      throw new Error('Unable to confirm payment');
    }

    return stripe.confirmPayment({
      clientSecret,
      confirmParams: {
        confirmation_token: confirmationToken.id,
      },
      redirect: 'if_required',
    });
  }

  createOrUpdatePaymentIntent() {
    const cart = this.cartService.cart();
    if (!cart) throw new Error('Problem with cart');

    return this.http
      .post<Cart>(`${this.baseUrl}payments/${cart.id}`, {})
      .pipe(
        map(cart => {
          this.cartService.setCart(cart);
          return cart;
        })
      );
  }

  disposeElements() {
    this.elements = undefined;
    this.addressElement = undefined;
    this.paymentElement = undefined;
  }
}
