import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExamingManagementExamingComponent } from './examing/examing.component';

const routes: Routes = [

  { path: 'examing', component: ExamingManagementExamingComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExamingManagementRoutingModule { }
