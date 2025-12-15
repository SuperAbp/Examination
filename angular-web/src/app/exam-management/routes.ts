import { Routes } from '@angular/router';
import { authGuard, permissionGuard, RouterOutletComponent } from '@abp/ng.core';
import { ExamsComponent } from './exams/exams.component';
import { QuestionBanksComponent } from './question-banks/question-banks.component';
import { QuestionBanksDetailComponent } from './question-banks/detail/detail.component';
import { QuestionBanksTrainComponent } from './question-banks/train/train.component';
import { MyFavoriteComponent } from './my/my-favorite/my-favorite.component';
import { MyFavoriteTrainComponent } from './my/my-favorite/train/train.component';
import { MistakeReviewsComponent } from './my/mistake-reviews/mistake-reviews.component';

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
  {
    path: 'my',
    children: [
      {
        path: 'favorites',
        component: MyFavoriteComponent,
        canActivate: [authGuard],
      },
      {
        path: 'favorites/train',
        component: MyFavoriteTrainComponent,
        canActivate: [authGuard],
      },
      {
        path: 'mistakes-reviews',
        component: MistakeReviewsComponent,
        canActivate: [authGuard],
      },
    ],
  },
  {
    path: 'exams',
    component: ExamsComponent,
    data: {
      requiredPolicy: 'Exam.Exams',
    },
  },
];
