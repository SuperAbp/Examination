import { Routes } from '@angular/router';
import { authGuard, permissionGuard } from '@abp/ng.core';
import { ExamsComponent } from './exams.component';

export const routes: Routes = [
  {
    path: 'exams',
    component: ExamsComponent,
    data: {
      requiredPolicy: 'Exam.Exams',
    },
  },
  {
    path: 'exams/welcome/:id',
    loadComponent: () => import('./welcome/welcome.component').then(m => m.ExamsWelcomeComponent),
    data: {
      requiredPolicy: 'Exam.Exams',
    },
  },
  {
    path: 'exams/start/:id',
    loadComponent: () => import('./start/start.component').then(m => m.ExamsStartComponent),
    canActivate: [authGuard],
    data: {
      requiredPolicy: 'Exam.Exams',
    },
  },
  {
    path: 'exams/submitted/:id',
    loadComponent: () =>
      import('./submitted/submitted.component').then(m => m.ExamsSubmittedComponent),
    data: {
      requiredPolicy: 'Exam.Exams',
    },
  },
];
