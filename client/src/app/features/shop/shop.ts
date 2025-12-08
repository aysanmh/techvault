import { Component, inject, OnInit } from '@angular/core';
import { Device } from '../../shared/models/device';
import { Pagination } from '../../shared/models/pagination';
import { ShopService } from '../../core/services/ShopService';
import {MatCard} from '@angular/material/card'
import { DeviceItem } from "./device-item/device-item";

@Component({
  selector: 'app-shop',
  imports: [
    MatCard,
    DeviceItem
],
  templateUrl: './shop.html',
  styleUrl: './shop.scss',
})
export class Shop  implements OnInit {
  private shopService = inject(ShopService);
  
  devices: Device[] = [];
  pagination?: Pagination<Device>;

  ngOnInit(): void {
    this.shopService.getDevices().subscribe({
      next: response => this.devices = response.data,
      
      error: error => console.error(error),
    });
  }

}
