import { Component, Input } from '@angular/core';
import { Device } from '../../../shared/models/device';
import { MatCard, MatCardContent, MatCardActions } from '@angular/material/card';
import { CurrencyPipe } from '@angular/common';
import { MatAnchor } from "@angular/material/button";
import { MatIcon } from "@angular/material/icon";

@Component({
  selector: 'app-device-item',
  imports: [
    MatCard,
    MatCardContent,
    CurrencyPipe,
    MatCardActions,
    MatAnchor,
    MatIcon
],
  templateUrl: './device-item.html',
  styleUrl: './device-item.scss',
})
export class DeviceItem {

  @Input() device?: Device;

}
