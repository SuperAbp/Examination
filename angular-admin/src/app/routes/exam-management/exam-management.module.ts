import { NgModule, Type } from '@angular/core';
import { SharedModule } from '@shared';
import { ExamManagementRoutingModule } from './exam-management-routing.module';
import { ExamManagementExamingComponent } from './examing/examing.component';
import { ExamManagementExamingEditComponent } from './examing/edit/edit.component';
import { ExamManagementRepositoryComponent } from './repository/repository.component';

const COMPONENTS: Type<void>[] = [ExamManagementExamingComponent, ExamManagementExamingEditComponent, ExamManagementRepositoryComponent];

@NgModule({
  imports: [SharedModule, ExamManagementRoutingModule],
  declarations: COMPONENTS
})
export class ExamManagementModule {}
