import { Routes } from '@angular/router';
import { DeviceListComponent } from './components/device-list/device-list.component';
import { DeviceDetailComponent } from './components/device-detail/device-detail.component';
import { DeviceFormComponent } from './components/device-form/device-form.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { authGuard } from './guards/auth.guard';
import { UnauthorizedComponent } from './components/auth/unauthorized/unauthorized.component';
import { roleGuard } from './guards/role.guard';

export const routes: Routes = [
    { path: '', redirectTo: 'devices', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'unauthorized', component: UnauthorizedComponent },
    { path: 'devices', component: DeviceListComponent },
    { path: 'devices/new', component: DeviceFormComponent, canActivate: [authGuard, roleGuard] },
    { path: 'devices/edit/:id', component: DeviceFormComponent, canActivate: [authGuard, roleGuard] },
    { path: 'devices/:id', component: DeviceDetailComponent },
    { path: '**', redirectTo: 'devices' }
];
