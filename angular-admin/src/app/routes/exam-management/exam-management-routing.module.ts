import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExamManagementExamComponent } from './exam/exam.component';

const routes: Routes = [{ path: 'exam', component: ExamManagementExamComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExamManagementRoutingModule {}
