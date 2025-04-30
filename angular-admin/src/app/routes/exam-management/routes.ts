import { Routes } from '@angular/router';

import { ExamManagementExamComponent } from './exam/exam.component';
import { ExamManagementExamViewComponent } from './exam/view/view.component';
import { ExamManagementUserExamComponent } from './user-exam/user-exam.component';

export const routes: Routes = [
  { path: 'exam', component: ExamManagementExamComponent },
  { path: 'view', component: ExamManagementExamViewComponent }
];
