import { permissionGuard } from '@abp/ng.core';
import { Routes } from '@angular/router';
import { authJWTCanActivate } from '@delon/auth';

import { ExamManagementExamComponent } from './exam/exam.component';
import { ExamManagementExamViewComponent } from './exam/view/view.component';
import { ExamManagementUserExamUserComponent } from './user-exam-user/user-exam-user.component';
import { ExamManagementUserExamComponent } from './user-exam/user-exam.component';
import { ExamManagementUserExamViewComponent } from './user-exam/view/view.component';

export const routes: Routes = [
  {
    path: 'exam',
    component: ExamManagementExamComponent,
    canActivate: [authJWTCanActivate, permissionGuard],
    data: {
      requiredPolicy: 'Exam.Exams'
    }
  },
  { path: 'view', component: ExamManagementExamViewComponent },
  { path: 'user-exam', component: ExamManagementUserExamComponent },
  { path: 'user-exam/:id', component: ExamManagementUserExamViewComponent },
  { path: 'user-exam-user', component: ExamManagementUserExamUserComponent }
];
