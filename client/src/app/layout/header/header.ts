import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { BusyService } from '../../core/services/BusyService';
import { CartService } from '../../core/services/CartService';
import { MatBadgeModule } from '@angular/material/badge';


@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    CommonModule,
    MatProgressBarModule,
    MatIconModule,
    RouterModule,
    MatBadgeModule
  ],
  templateUrl: './header.html',
  styleUrls: ['./header.scss']
})
export class Header {


  busyService = inject(BusyService);
  cartService = inject(CartService);

}
