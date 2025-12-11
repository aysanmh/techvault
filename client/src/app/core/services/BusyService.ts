import { Injectable, signal, computed } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class BusyService {

  private _busyRequestsCount = signal(0);

  busyRequestsCount = computed(() => this._busyRequestsCount());
  loading = computed(() => this._busyRequestsCount() > 0);

  busy() {
    this._busyRequestsCount.set(this._busyRequestsCount() + 1);
  }

  idle() {
    this._busyRequestsCount.update(count => Math.max(count - 1, 0));
  }

}
