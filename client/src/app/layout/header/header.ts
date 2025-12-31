import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatIconModule } from '@angular/material/icon';
import { Router, RouterModule } from '@angular/router';
import { BusyService } from '../../core/services/BusyService';
import { CartService } from '../../core/services/CartService';
import { MatBadgeModule } from '@angular/material/badge';
import { AccountService } from '../../core/services/AccountService';
import { MatAnchor } from '@angular/material/button';
import { MatMenu, MatMenuItem, MatMenuTrigger } from '@angular/material/menu';
import { MatDivider } from '@angular/material/divider';
import { IsAdmin } from "../../shared/directives/is-admin";

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    CommonModule,
    MatProgressBarModule,
    MatIconModule,
    RouterModule,
    MatBadgeModule,
    MatAnchor,
    MatMenuTrigger,
    MatMenu,
    MatDivider,
    MatMenuItem,
    IsAdmin
],
  templateUrl: './header.html',
  styleUrls: ['./header.scss'],
})
export class Header {
  busyService = inject(BusyService);
  cartService = inject(CartService);
  accountService = inject(AccountService);
  private router = inject(Router);

  logout() {
    this.accountService.logout().subscribe({
      next: () => {
        this.accountService.currentUser.set(null);
        this.router.navigateByUrl('/');
      },
    });
  }
}
