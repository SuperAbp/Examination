import { PermissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JWTGuard } from '@delon/auth';
import { QuestionManagementQuestionComponent } from './question/question.component';
import { QuestionManagementRepositoryComponent } from './repository/repository.component';

const routes: Routes = [
  {
    path: 'question',
    component: QuestionManagementQuestionComponent,
    canActivate: [JWTGuard, PermissionGuard],
    data: {
      requiredPolicy: 'Exam.Question'
    }
  },
  {
    path: 'repository',
    component: QuestionManagementRepositoryComponent,
    canActivate: [JWTGuard, PermissionGuard],
    data: {
      requiredPolicy: 'Exam.QuestionRepository'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class QuestionManagementRoutingModule {}
