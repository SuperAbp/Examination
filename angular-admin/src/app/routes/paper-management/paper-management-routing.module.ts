import { PermissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authJWTCanActivate } from '@delon/auth';
import { PaperManagementPaperEditComponent } from './paper/edit/edit.component';
import { PaperManagementPaperComponent } from './paper/paper.component';

const routes: Routes = [
  { path: 'paper', component: PaperManagementPaperComponent },
  {
    path: 'paper/:id/edit',
    component: PaperManagementPaperEditComponent,
    canActivate: [authJWTCanActivate, PermissionGuard],
    data: {
      requiredPolicy: 'Exam.Paper.Update'
    }
  },
  {
    path: 'paper/create',
    component: PaperManagementPaperEditComponent,
    canActivate: [authJWTCanActivate, PermissionGuard],
    data: {
      requiredPolicy: 'Exam.Paper.Create'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PaperManagementRoutingModule {}
