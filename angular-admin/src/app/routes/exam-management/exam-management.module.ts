import { NgModule, Type } from '@angular/core';
import { SharedModule } from '@shared';
import { ExamManagementRoutingModule } from './exam-management-routing.module';
import { ExamManagementExamComponent } from './exam/exam.component';
import { ExamManagementExamEditComponent } from './exam/edit/edit.component';

const COMPONENTS: Type<void>[] = [ExamManagementExamComponent, ExamManagementExamEditComponent];

@NgModule({
  imports: [SharedModule, ExamManagementRoutingModule],
  declarations: COMPONENTS
})
export class ExamManagementModule {}
