import { Routes } from '@angular/router';
import { authGuard, permissionGuard, RouterOutletComponent } from '@abp/ng.core';
import { ExamsComponent } from './exams/exams.component';

export const routes: Routes = [
  {
    path: 'exams',
    component: ExamsComponent,
    data: {
      requiredPolicy: 'Exam.Exams',
    },
  },
];
