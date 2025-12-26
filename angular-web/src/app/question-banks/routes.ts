import { Routes } from '@angular/router';
import { authGuard } from '@abp/ng.core';
import { QuestionBanksComponent } from './question-banks.component';
import { QuestionBanksDetailComponent } from './detail/detail.component';
import { QuestionBanksTrainComponent } from './train/train.component';

export const routes: Routes = [
  {
    path: 'question-banks',
    component: QuestionBanksComponent,
    canActivate: [authGuard],
  },
  {
    path: 'question-banks/:id',
    component: QuestionBanksDetailComponent,
    canActivate: [authGuard],
  },
  {
    path: 'question-banks/:id/train',
    component: QuestionBanksTrainComponent,
    canActivate: [authGuard],
  },
];
