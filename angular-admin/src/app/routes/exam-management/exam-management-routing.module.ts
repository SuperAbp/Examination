import { PermissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JWTGuard } from '@delon/auth';
import { ExamManagementExamingEditComponent } from './examing/edit/edit.component';
import { ExamManagementExamingComponent } from './examing/examing.component';
import { ExamManagementRepositoryComponent } from './repository/repository.component';

const routes: Routes = [
  { path: 'examing', component: ExamManagementExamingComponent },
  {
    path: 'examing/:id/edit',
    component: ExamManagementExamingEditComponent,
    canActivate: [JWTGuard, PermissionGuard],
    data: {
      requiredPolicy: 'Exam.Examing.Update'
    }
  },
  {
    path: 'examing/create',
    component: ExamManagementExamingEditComponent,
    canActivate: [JWTGuard, PermissionGuard],
    data: {
      requiredPolicy: 'Exam.Examing.Create'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExamManagementRoutingModule {}
