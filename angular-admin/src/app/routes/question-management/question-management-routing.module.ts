import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { QuestionManagementQuestionComponent } from './question/question.component';

const routes: Routes = [

  { path: 'question', component: QuestionManagementQuestionComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class QuestionManagementRoutingModule { }
