import { Routes } from '@angular/router';
import { DeviceListComponent } from './components/device-list/device-list.component';
import { DeviceDetailComponent } from './components/device-detail/device-detail.component';
import { DeviceFormComponent } from './components/device-form/device-form.component';

export const routes: Routes = [
    { path: '', redirectTo: 'devices', pathMatch: 'full' },
    { path: 'devices', component: DeviceListComponent },
    { path: 'devices/new', component: DeviceFormComponent },
    { path: 'devices/edit/:id', component: DeviceFormComponent },
    { path: 'devices/:id', component: DeviceDetailComponent },
    { path: '**', redirectTo: 'devices' }
];
