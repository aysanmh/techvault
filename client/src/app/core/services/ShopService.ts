import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Device } from '../../shared/models/device';
import { ShopParams } from '../../shared/models/shopParams';

@Injectable({
  providedIn: 'root',
})
export class ShopService {

  
  baseUrl = 'https://localhost:5001/api/'

  private http =inject(HttpClient);

  brands: string[] =[];
  groups: string[] = [];

   getDevices(shopParams: ShopParams){

    let params = new HttpParams();

    if(shopParams.brands.length > 0){

      params = params.append('brands', shopParams.brands.join(','));

    }
    if(shopParams.groups.length > 0){

      params = params.append('groups', shopParams.groups.join(','));

    }

    if(shopParams.sort){
      params = params.append('sort',shopParams.sort);
    }

    if(shopParams.search){
      params = params.append('search', shopParams.search)
    }

    params = params.append('pageSize', shopParams.pageSize);

    params = params.append('pageIndex', shopParams.pageNumber);

    return this.http.get<Pagination<Device>>(this.baseUrl + 'devices', {params})

   }

   getBrands(){

      if(this.brands.length > 0) return;

      return this.http.get<string[]>(this.baseUrl + 'devices/brands').subscribe({
        next: response => this.brands = response
      })
   }

   getGroups(){

      if(this.groups.length > 0) return;
      
      return this.http.get<string[]>(this.baseUrl + 'devices/groups').subscribe({
        next: response => this.groups = response
      })
   }
  
}
