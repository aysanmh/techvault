import { ChangeDetectorRef, Component, inject, NgZone, OnInit } from '@angular/core';
import { OrderService } from '../../../core/services/OrderService';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Order } from '../../../shared/models/order';
import { MatCard, MatCardModule } from '@angular/material/card';
import { MatButton } from '@angular/material/button';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { AddressPipe } from '../../../shared/pipes/address-pipe';
import { PaymentCardPipe } from '../../../shared/pipes/payment-card-pipe';
import { AccountService } from '../../../core/services/AccountService';
import { AdminService } from '../../../core/services/AdminService';

@Component({
  selector: 'app-order-detailed',
  imports: [
    MatCardModule,
    DatePipe,
    CurrencyPipe,
    AddressPipe,
    PaymentCardPipe,
    RouterLink,
    CommonModule,
  ],
  templateUrl: './order-detailed.html',
  styleUrl: './order-detailed.css',
})
export class OrderDetailed implements OnInit {
  private orderService = inject(OrderService);
  private activatedRoute = inject(ActivatedRoute);
  private zone = inject(NgZone);
  private changeDetector = inject(ChangeDetectorRef);
  private accountService = inject(AccountService);
  private adminService = inject(AdminService);
  private router = inject(Router);
  order?: Order;
  buttonText = this.accountService.isAdmin() ? 'Return to admin' : 'Return to orders';

  ngOnInit(): void {
    this.loadOrder();
  }
  onReturnClick(){
    this.accountService.isAdmin()
    ? this.router.navigateByUrl('/admin')
    : this.router.navigateByUrl('/orders')

  }

  loadOrder() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) return;

    const loadOrderData = this.accountService.isAdmin()
      ? this.adminService.getOrder(+id)
      : this.orderService.getOrderDetailed(+id);

    loadOrderData.subscribe({
      next: (order) => {
        this.zone.run(() => {
          this.order = order;
          this.changeDetector.detectChanges();
        });
      },
    });
  }
}
