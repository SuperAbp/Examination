import { PermissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JWTGuard } from '@delon/auth';
import { QuestionManagementQuestionEditComponent } from './question/edit/edit.component';
import { QuestionManagementQuestionComponent } from './question/question.component';
import { QuestionManagementRepositoryComponent } from './repository/repository.component';

const routes: Routes = [
  {
    path: 'repository',
    component: QuestionManagementRepositoryComponent,
    canActivate: [JWTGuard, PermissionGuard],
    data: {
      requiredPolicy: 'Exam.QuestionRepository'
    }
  },
  {
    path: 'question',
    component: QuestionManagementQuestionComponent,
    canActivate: [JWTGuard, PermissionGuard],
    data: {
      requiredPolicy: 'Exam.Question'
    }
  },
  {
    path: 'question/:id',
    component: QuestionManagementQuestionEditComponent,
    canActivate: [JWTGuard, PermissionGuard],
    data: {
      requiredPolicy: 'Exam.Question.Update'
    }
  },
  {
    path: 'question/create',
    component: QuestionManagementQuestionEditComponent,
    canActivate: [JWTGuard, PermissionGuard],
    data: {
      requiredPolicy: 'Exam.Question.Create'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class QuestionManagementRoutingModule {}
