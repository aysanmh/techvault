import { Routes } from '@angular/router';
import { Home } from './features/home/home';
import { Shop } from './features/shop/shop';
import { DeviceDetails } from './features/shop/device-details/device-details';

export const routes: Routes = [
    {path: '' , component:Home},
    {path:'shop', component:Shop},
    {path:'shop/:id', component:DeviceDetails},
    {path: '**', redirectTo: '', pathMatch:'full'}
];
