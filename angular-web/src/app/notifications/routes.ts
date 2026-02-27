import { Routes } from '@angular/router';
import { authGuard } from '@abp/ng.core';
import { NotificationsComponent } from './notifications.component';

export const routes: Routes = [
  {
    path: 'notifications',
    component: NotificationsComponent,
    canActivate: [authGuard],
  },
];
