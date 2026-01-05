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
      },
      {
        path: 'exams/detail/:id',
        component: ExamDetailComponent,
      },
      {
        path: 'favorites',
        component: MyFavoritesComponent,
      },
      {
        path: 'favorites/train',
        component: MyFavoritesTrainComponent,
      },
      {
        path: 'mistakes',
        component: MistakesComponent,
      },
      {
        path: 'mistakes/train',
        component: MistakesTrainComponent,
      },
    ],
  },
];
