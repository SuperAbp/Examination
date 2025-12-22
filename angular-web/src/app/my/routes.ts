import { Routes } from '@angular/router';
import { authGuard } from '@abp/ng.core';
import { MistakeReviewsComponent } from './mistake-reviews/mistake-reviews.component';
import { MistakeReviewsTrainComponent } from './mistake-reviews/train/train.component';
import { MyFavoritesComponent } from './my-favorites/my-favorites.component';
import { MyFavoritesTrainComponent } from './my-favorites/train/train.component';
import { MyExamsComponent } from './my-exams/my-exams.component';
import { ExamDetailComponent } from './my-exams/detail/detail.component';

export const routes: Routes = [
  {
    path: 'my',
    children: [
      {
        path: 'exams',
        component: MyExamsComponent,
        canActivate: [authGuard],
      },
      {
        path: 'exams/detail/:id',
        component: ExamDetailComponent,
        canActivate: [authGuard],
      },
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
];
