import { permissionGuard } from '@abp/ng.core';
import { Routes } from '@angular/router';
import { authJWTCanActivate } from '@delon/auth';

import { QuestionManagementAnswerComponent } from './answer/answer.component';
import { QuestionManagementQuestionBankComponent } from './question-bank/question-bank.component';
import { QuestionManagementQuestionEditComponent } from './question/edit/edit.component';
import { QuestionManagementQuestionImportComponent } from './question/import/import.component';
import { QuestionManagementQuestionComponent } from './question/question.component';
export const routes: Routes = [
  {
    path: 'question-bank',
    component: QuestionManagementQuestionBankComponent,
    canActivate: [authJWTCanActivate, permissionGuard],
    data: {
      requiredPolicy: 'Exam.QuestionBank'
    }
  },
  {
    path: 'question',
    component: QuestionManagementQuestionComponent,
    canActivate: [authJWTCanActivate, permissionGuard],
    data: {
      requiredPolicy: 'Exam.Question'
    }
  },
  {
    path: 'question/:id/edit',
    component: QuestionManagementQuestionEditComponent,
    canActivate: [authJWTCanActivate, permissionGuard],
    data: {
      requiredPolicy: 'Exam.Question.Update'
    }
  },
  {
    path: 'question/create',
    component: QuestionManagementQuestionEditComponent,
    canActivate: [authJWTCanActivate, permissionGuard],
    data: {
      requiredPolicy: 'Exam.Question.Create'
    }
  },
  {
    path: 'question/import',
    component: QuestionManagementQuestionImportComponent,
    canActivate: [authJWTCanActivate, permissionGuard],
    data: {
      requiredPolicy: 'Exam.Question.Import'
    }
  },
  { path: 'answer', component: QuestionManagementAnswerComponent }
];
