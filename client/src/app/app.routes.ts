import { Routes } from '@angular/router';
import { Home } from './features/home/home';
import { Shop } from './features/shop/shop';
import { DeviceDetails } from './features/shop/device-details/device-details';
import { TestError } from './features/test-error/test-error';
import { NotFound } from './shared/components/not-found/not-found';
import { ServerError } from './shared/components/server-error/server-error';

export const routes: Routes = [
    {path: '' , component:Home},
    {path:'shop', component:Shop},
    {path:'shop/:id', component:DeviceDetails},
    {path: 'test-error', component: TestError},
    {path: 'not-found', component:NotFound},
    {path: 'server-error', component:ServerError},
    {path: '**', redirectTo: 'not-found', pathMatch:'full'}
];
