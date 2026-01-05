import { Routes } from '@angular/router';
import { authGuard } from '@abp/ng.core';
import { ExamsComponent } from './exams.component';

export const routes: Routes = [
  {
    path: 'exams',
    component: ExamsComponent,
  },
  {
    path: 'exams/welcome/:id',
    loadComponent: () => import('./welcome/welcome.component').then(m => m.ExamsWelcomeComponent),
  },
  {
    path: 'exams/start/:id',
    loadComponent: () => import('./start/start.component').then(m => m.ExamsStartComponent),
  },
  {
    path: 'exams/submitted/:id',
    loadComponent: () =>
      import('./submitted/submitted.component').then(m => m.ExamsSubmittedComponent),
  },
];
