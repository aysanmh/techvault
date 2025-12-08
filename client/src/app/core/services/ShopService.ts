import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Device } from '../../shared/models/device';

@Injectable({
  providedIn: 'root',
})
export class ShopService {

  
  baseUrl = 'https://localhost:5001/api/'

  private http =inject(HttpClient);
  types
   getDevices(){

    return this.http.get<Pagination<Device>>(this.baseUrl + 'devices?pageSize=10')

   }
  
}
