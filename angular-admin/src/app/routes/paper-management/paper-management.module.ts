import { NgModule, Type } from '@angular/core';
import { SharedModule } from '@shared';
import { PaperManagementRepositoryComponent } from './repository/repository.component';
import { PaperManagementPaperComponent } from './paper/paper.component';
import { PaperManagementPaperEditComponent } from './paper/edit/edit.component';
import { PaperManagementRoutingModule } from './paper-management-routing.module';

const COMPONENTS: Type<void>[] = [PaperManagementPaperComponent, PaperManagementPaperEditComponent, PaperManagementRepositoryComponent];

@NgModule({
  imports: [SharedModule, PaperManagementRoutingModule],
  declarations: COMPONENTS
})
export class PaperManagementModule {}
