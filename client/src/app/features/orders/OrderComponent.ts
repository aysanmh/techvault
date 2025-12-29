import { ChangeDetectorRef, Component, inject, NgZone, OnInit } from '@angular/core';
import { OrderService } from '../../core/services/OrderService';
import { Order } from '../../shared/models/order';
import { RouterLink } from '@angular/router';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';

@Component({
  selector: 'app-order',
  imports: [RouterLink, DatePipe, CurrencyPipe, CommonModule],
  templateUrl: './order.html',
  styleUrls: ['./order.css'],
})
export class OrderComponent implements OnInit {
  private orderService = inject(OrderService);
  private zone = inject(NgZone);
  private changeDetector = inject(ChangeDetectorRef);
  orders: Order[] = [];

  ngOnInit(): void {
    this.loadOrders();
  }
  loadOrders() {
    this.orderService.getOrdersForUser().subscribe({
      next: (orders) => {
        this.zone.run(() => {
          this.orders = orders;
          this.changeDetector.detectChanges();
        });
      },
      error: (err) => {
        console.error('Failed to load orders', err);
      },
    });
  }
}
