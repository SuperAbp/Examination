import { NgModule, Type } from '@angular/core';
import { SharedModule } from '@shared';
import { QuestionManagementRoutingModule } from './question-management-routing.module';
import { QuestionManagementQuestionComponent } from './question/question.component';
import { QuestionManagementQuestionEditComponent } from './question/edit/edit.component';
import { QuestionManagementRepositoryComponent } from './repository/repository.component';
import { QuestionManagementRepositoryEditComponent } from './repository/edit/edit.component';
import { QuestionManagementAnswerComponent } from './answer/answer.component';

const COMPONENTS: Type<void>[] = [
  QuestionManagementQuestionComponent,
  QuestionManagementQuestionEditComponent,
  QuestionManagementRepositoryComponent,
  QuestionManagementRepositoryEditComponent
,
  QuestionManagementAnswerComponent];

@NgModule({
  imports: [SharedModule, QuestionManagementRoutingModule],
  declarations: COMPONENTS
})
export class QuestionManagementModule {}
