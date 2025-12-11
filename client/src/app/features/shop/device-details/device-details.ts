import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../../core/services/ShopService';
import { ActivatedRoute } from '@angular/router';
import { Device } from '../../../shared/models/device';
import { CurrencyPipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatFormField, MatLabel } from '@angular/material/select';
import {MatInput} from '@angular/material/input';
import { MatDivider } from '@angular/material/divider';

@Component({
  selector: 'app-device-details',
  imports: [
    CurrencyPipe,
    MatButton,
    MatIcon,
    MatFormField,
    MatInput,
    MatLabel,
    
  ],
  templateUrl: './device-details.html',
  styleUrl: './device-details.scss',
})
export class DeviceDetails implements OnInit {
  
  private shopService = inject(ShopService);
  
  private activatedRoute = inject(ActivatedRoute);

  device?: Device;

  ngOnInit(): void {
    this.loadDevice();
  }

  loadDevice(){
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if(!id) return;

    this.shopService.getDevice(+id).subscribe({
      next: device => this.device = device,
      error: error => console.log(error)
    })
  }

}
