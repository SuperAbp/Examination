import { authGuard, permissionGuard } from '@abp/ng.core';
import { Routes } from '@angular/router';

export const APP_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    canActivate: [authGuard],
    loadComponent: () => import('./home/home.component').then(c => c.HomeComponent),
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(c => c.createRoutes()),
  },
  {
    path: '',
    loadChildren: () => import('./exams/routes').then(m => m.routes),
    canActivate: [authGuard],
  },
  {
    path: '',
    loadChildren: () => import('./question-banks/routes').then(m => m.routes),
    canActivate: [authGuard],
  },
  {
    path: '',
    loadChildren: () => import('./my/routes').then(m => m.routes),
    canActivate: [authGuard],
  },
  {
    path: 'announcements',
    loadChildren: () => import('./announcements/routes').then(m => m.routes),
    canActivate: [authGuard],
  },
  {
    path: '',
    loadChildren: () => import('./notifications/routes').then(m => m.routes),
    canActivate: [authGuard],
  },
];
