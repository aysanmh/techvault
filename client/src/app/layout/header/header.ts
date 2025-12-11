import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { BusyService } from '../../core/services/BusyService';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    CommonModule,
    MatProgressBarModule,
    MatIconModule,
    RouterModule
  ],
  templateUrl: './header.html',
  styleUrls: ['./header.scss']
})
export class Header {


  busyService = inject(BusyService)

}
