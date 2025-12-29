import { ChangeDetectorRef, Component, inject, NgZone, OnInit } from '@angular/core';
import { OrderService } from '../../../core/services/OrderService';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Order } from '../../../shared/models/order';
import { MatCard, MatCardModule } from '@angular/material/card';
import { MatButton } from '@angular/material/button';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { AddressPipe } from "../../../shared/pipes/address-pipe";
import { PaymentCardPipe } from '../../../shared/pipes/payment-card-pipe';

@Component({
  selector: 'app-order-detailed',
  imports: [
    MatCardModule,
    DatePipe,
    CurrencyPipe,
    AddressPipe,
    PaymentCardPipe,
    RouterLink,
    CommonModule
],
  templateUrl: './order-detailed.html',
  styleUrl: './order-detailed.css',
})
export class OrderDetailed implements OnInit {

  private orderService = inject(OrderService);
  private activatedRoute = inject(ActivatedRoute);
  private zone = inject(NgZone);
  private changeDetector = inject(ChangeDetectorRef);
  order?: Order;


  ngOnInit(): void {
    this.loadOrder(); 
  }


  loadOrder() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) return;

    this.orderService.getOrderDetailed(+id).subscribe({
      next: order => {
        this.zone.run(() => {   
          this.order = order;
          this.changeDetector.detectChanges();  
        });
      }
    });
  }
}
