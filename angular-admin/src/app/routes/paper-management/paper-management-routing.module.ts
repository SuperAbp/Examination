import { PermissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JWTGuard } from '@delon/auth';
import { PaperManagementPaperEditComponent } from './paper/edit/edit.component';
import { PaperManagementPaperComponent } from './paper/paper.component';

const routes: Routes = [
  { path: 'paper', component: PaperManagementPaperComponent },
  {
    path: 'paper/:id/edit',
    component: PaperManagementPaperEditComponent,
    canActivate: [JWTGuard, PermissionGuard],
    data: {
      requiredPolicy: 'Exam.Papers.Update'
    }
  },
  {
    path: 'paper/create',
    component: PaperManagementPaperEditComponent,
    canActivate: [JWTGuard, PermissionGuard],
    data: {
      requiredPolicy: 'Exam.Papers.Create'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PaperManagementRoutingModule {}
