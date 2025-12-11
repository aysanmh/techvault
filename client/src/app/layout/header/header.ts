import { Component, inject } from '@angular/core';
import {MatBadge} from "@angular/material/badge";
import {MatButton} from "@angular/material/button";
import {MatIcon} from "@angular/material/icon";
import {MatProgressBar} from "@angular/material/progress-bar";

import { RouterLink, RouterLinkActive } from '@angular/router';
import { BusyService } from '../../core/services/BusyService';

@Component({
  selector: 'app-header',
  imports: [
    MatIcon,
    MatButton,
    MatBadge,
    RouterLink,
    RouterLinkActive,
    MatProgressBar
  ],
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header {

  busyService = inject(BusyService)

}
