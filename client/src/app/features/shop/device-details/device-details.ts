import { Component, inject, OnInit, signal } from '@angular/core';
import { ShopService } from '../../../core/services/ShopService';
import { ActivatedRoute } from '@angular/router';
import { Device } from '../../../shared/models/device';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { CartService } from '../../../core/services/CartService';

@Component({
  selector: 'app-device-details',
  standalone: true,
  imports: [CurrencyPipe, MatButton, MatIcon, CommonModule],
  templateUrl: './device-details.html',
  styleUrls: ['./device-details.scss'],
})
export class DeviceDetails implements OnInit {
  private shopService = inject(ShopService);
  private activatedRoute = inject(ActivatedRoute);
  private cartService = inject(CartService);

  device = signal<Device | null>(null);
  loading = signal(true);

  ngOnInit(): void {
    this.loadDevice();
  }

  loadDevice() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) return;

    this.shopService.getDevice(+id).subscribe({
      next: (device) => {
        this.device.set(device);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      },
    });
  }

  addToCart() {
    const device = this.device();
    if (!device) return;

    this.cartService.addItemToCart(device, 1);
  }
}
