import { Routes } from '@angular/router';
import { authGuard, permissionGuard } from '@abp/ng.core';
import { QuestionBanksComponent } from './question-banks.component';
import { QuestionBanksDetailComponent } from './detail/detail.component';
import { QuestionBanksTrainComponent } from './train/train.component';

export const routes: Routes = [
  {
    path: 'question-banks',
    component: QuestionBanksComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      requiredPolicy: 'Exam.QuestionBanks',
    },
  },
  {
    path: 'question-banks/:id',
    component: QuestionBanksDetailComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      requiredPolicy: 'Exam.QuestionBanks',
    },
  },
  {
    path: 'question-banks/:id/train',
    component: QuestionBanksTrainComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      requiredPolicy: 'Exam.QuestionBanks',
    },
  },
];
