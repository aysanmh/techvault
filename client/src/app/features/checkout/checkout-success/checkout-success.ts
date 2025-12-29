import { Component, inject, OnDestroy } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { SignalrService } from '../../../core/services/SignalrService';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AsyncPipe, CurrencyPipe, DatePipe, NgIf } from '@angular/common';
import { AddressPipe } from '../../../shared/pipes/address-pipe';
import { PaymentCardPipe } from '../../../shared/pipes/payment-card-pipe';
import { MatIcon } from "@angular/material/icon";
import { OrderService } from '../../../core/services/OrderService';

@Component({
  selector: 'app-checkout-success',
  imports: [
    MatButton,
    RouterLink,
    MatProgressSpinnerModule,
    DatePipe,
    AddressPipe,
    CurrencyPipe,
    PaymentCardPipe,
    NgIf,
    
],
  templateUrl: './checkout-success.html',
  styleUrls: ['./checkout-success.css'],
})
export class CheckoutSuccess implements OnDestroy {
  signalrService = inject(SignalrService);
  
  private orderService = inject(OrderService);

  ngOnDestroy(): void {
    this.orderService.orderComplete = false;

    this.signalrService.orderSignal.set(null);
  }


}
