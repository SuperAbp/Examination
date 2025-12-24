import { Routes } from '@angular/router';
import { authGuard } from '@abp/ng.core';
import { MistakesComponent } from './mistakes/mistakes.component';
import { MistakesTrainComponent } from './mistakes/train/train.component';
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
        path: 'mistakes',
        component: MistakesComponent,
        canActivate: [authGuard],
      },
      {
        path: 'mistakes/train',
        component: MistakesTrainComponent,
        canActivate: [authGuard],
      },
    ],
  },
];
