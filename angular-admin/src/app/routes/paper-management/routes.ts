import { permissionGuard } from '@abp/ng.core';
import { Routes } from '@angular/router';
import { authJWTCanActivate } from '@delon/auth';

import { PaperManagementPaperEditComponent } from './paper/edit/edit.component';
import { PaperManagementPaperComponent } from './paper/paper.component';

export const routes: Routes = [
  { path: 'paper', component: PaperManagementPaperComponent },
  {
    path: 'paper/:id/edit/:model',
    component: PaperManagementPaperEditComponent,
    canActivate: [authJWTCanActivate, permissionGuard],
    data: {
      requiredPolicy: 'Exam.Paper.Update'
    }
  },
  {
    path: 'paper/create/:model',
    component: PaperManagementPaperEditComponent,
    canActivate: [authJWTCanActivate, permissionGuard],
    data: {
      requiredPolicy: 'Exam.Paper.Create'
    }
  }
];
