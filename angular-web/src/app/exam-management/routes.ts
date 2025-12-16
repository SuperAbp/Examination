import { Routes } from '@angular/router';
import { authGuard, permissionGuard, RouterOutletComponent } from '@abp/ng.core';
import { ExamsComponent } from './exams/exams.component';
import { QuestionBanksComponent } from './question-banks/question-banks.component';
import { QuestionBanksDetailComponent } from './question-banks/detail/detail.component';
import { QuestionBanksTrainComponent } from './question-banks/train/train.component';
import { MistakeReviewsComponent } from './my/mistake-reviews/mistake-reviews.component';
import { MistakeReviewsTrainComponent } from './my/mistake-reviews/train/train.component';
import { MyFavoritesComponent } from './my/my-favorites/my-favorites.component';
import { MyFavoritesTrainComponent } from './my/my-favorites/train/train.component';

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
        component: MyFavoritesComponent,
        canActivate: [authGuard],
      },
      {
        path: 'favorites/train',
        component: MyFavoritesTrainComponent,
        canActivate: [authGuard],
      },
      {
        path: 'mistakes-reviews',
        component: MistakeReviewsComponent,
        canActivate: [authGuard],
      },
      {
        path: 'mistakes-reviews/train',
        component: MistakeReviewsTrainComponent,
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
  {
    path: 'exams',
    component: ExamsComponent,
    data: {
      requiredPolicy: 'Exam.Exams',
    },
  },
  {
    path: 'exams/welcome/:id',
    loadComponent: () =>
      import('./exams/welcome/welcome.component').then(m => m.ExamsWelcomeComponent),
    data: {
      requiredPolicy: 'Exam.Exams',
    },
  },
];
