import { Routes } from '@angular/router';
import { authGuard } from '@abp/ng.core';
import { AnnouncementsComponent } from './announcements.component';
import { AnnouncementDetailComponent } from './detail/detail.component';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: AnnouncementsComponent,
    canActivate: [authGuard],
  },
  {
    path: ':id',
    component: AnnouncementDetailComponent,
    canActivate: [authGuard],
  },
];
