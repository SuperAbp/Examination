import { NgModule, Type } from '@angular/core';
import { SharedModule } from '@shared';
import { QuestionManagementRoutingModule } from './question-management-routing.module';
import { QuestionManagementQuestionComponent } from './question/question.component';
import { QuestionManagementQuestionEditComponent } from './question/edit/edit.component';

const COMPONENTS: Type<void>[] = [QuestionManagementQuestionComponent, QuestionManagementQuestionEditComponent];

@NgModule({
  imports: [SharedModule, QuestionManagementRoutingModule],
  declarations: COMPONENTS
})
export class QuestionManagementModule {}
