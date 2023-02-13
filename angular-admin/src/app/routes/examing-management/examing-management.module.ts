import { NgModule, Type } from '@angular/core';
import { SharedModule } from '@shared';
import { ExamingManagementRoutingModule } from './examing-management-routing.module';
import { ExamingManagementExamingComponent } from './examing/examing.component';
import { ExamingManagementExamingEditComponent } from './examing/edit/edit.component';

const COMPONENTS: Type<void>[] = [ExamingManagementExamingComponent, ExamingManagementExamingEditComponent];

@NgModule({
  imports: [SharedModule, ExamingManagementRoutingModule],
  declarations: COMPONENTS
})
export class ExamingManagementModule {}
