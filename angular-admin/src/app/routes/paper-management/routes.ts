import { permissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authJWTCanActivate } from '@delon/auth';
import { PaperManagementPaperEditComponent } from './paper/edit/edit.component';
import { PaperManagementPaperComponent } from './paper/paper.component';

export const routes: Routes = [
  { path: 'paper', component: PaperManagementPaperComponent },
  {
    path: 'paper/:id/edit',
    component: PaperManagementPaperEditComponent,
    canActivate: [authJWTCanActivate, permissionGuard],
    data: {
      requiredPolicy: 'Exam.Paper.Update'
    }
  },
  {
    path: 'paper/create',
    component: PaperManagementPaperEditComponent,
    canActivate: [authJWTCanActivate, permissionGuard],
    data: {
      requiredPolicy: 'Exam.Paper.Create'
    }
  }
];
