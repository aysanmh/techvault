import { Component, inject, OnInit } from '@angular/core';
import { Device } from '../../shared/models/device';
import { Pagination } from '../../shared/models/pagination';
import { ShopService } from '../../core/services/ShopService';
import {MatCard} from '@angular/material/card'
import { DeviceItem } from "./device-item/device-item";
import {MatDialog} from '@angular/material/dialog'
import { FiltersDialog } from './filters-dialog/filters-dialog';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import {MatMenu, MatMenuTrigger} from '@angular/material/menu';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { ShopParams } from '../../shared/models/shopParams';
import {MatPaginator, PageEvent} from '@angular/material/paginator'
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';




@Component({
  selector: 'app-shop',
    standalone: true, 
  imports: [
    CommonModule,
    DeviceItem,
    MatButton,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger,
    MatPaginator,
    FormsModule,
    MatIconButton
],
  templateUrl: './shop.html',
  styleUrl: './shop.scss',
})
export class Shop  implements OnInit {

  private shopService = inject(ShopService);

  private dialogService = inject(MatDialog);
  
  devices?: Pagination<Device>;

  sortOptions = [
    {name: 'Alphabetical' , value:'name'},
    {name: 'Price: Low-High', value: 'priceAsc'},
    {name: 'Price: High-Low', value: 'priceDesc'},
  ]
  pagination?: Pagination<Device>;

  shopParams = new ShopParams();

  pageSizeOptions = [5,10,15,20] 

  ngOnInit(): void {
     
    this.initializeShop()
  }

  initializeShop(){

    this.shopService.getBrands();
    this.shopService.getGroups();
    this.getDevices();
    
  }

  getDevices(){

    this.shopService.getDevices(this.shopParams).subscribe({
      next: response => this.devices = response,
      
      error: error => console.error(error),
    })

  }


  onSearchChange(){

    this.shopParams.pageNumber = 1;
    this.getDevices();

  }
    handlePageEvent(event:PageEvent){
    this.shopParams.pageNumber = event.pageIndex + 1;
    this.shopParams.pageSize = event.pageSize;
    this.getDevices();
  }

  onSortChange(event: MatSelectionListChange){

    const selectedOption = event.options[0];
    if(selectedOption){
      this.shopParams.sort = selectedOption.value;
      this.shopParams.pageNumber = 1;
      this.getDevices();
    }
  }


  openFiltersDialog(){

    const dialogRef = this.dialogService.open(FiltersDialog, {
      minWidth: '500px',
      data:{

        selectedBrands: this.shopParams.brands,
        selectedGroups: this.shopParams.groups

      }
    });
    dialogRef.afterClosed().subscribe({
      next: result => {
        if(result){
         
          this.shopParams.brands = result.selectedBrands;
          this.shopParams.groups = result.selectedGroups;
          this.shopParams.pageNumber = 1;
          this.getDevices();
        }
      }
    })
  }
  


}
