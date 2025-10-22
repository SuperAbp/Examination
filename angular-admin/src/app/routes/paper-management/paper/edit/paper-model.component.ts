import { CoreModule } from '@abp/ng.core';
import { Component, inject, OnInit } from '@angular/core';
import { QuestionDetailDto } from '@proxy/admin/question-management/questions';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzModalModule, NzModalRef } from 'ng-zorro-antd/modal';
import { NzSpaceModule } from 'ng-zorro-antd/space';

export class questionTest {
  constructor(name: string, score: number, items: QuestionDetailDto[]) {
    this.name = name;
    this.score = score;
    this.items = items;
  }
  name: string;
  score: number;
  items: QuestionDetailDto[];
}
@Component({
  selector: 'app-exam-management-paper-model',
  templateUrl: './paper-model.component.html',
  standalone: true,
  imports: [CoreModule, NzButtonModule, NzSpaceModule, NzGridModule, NzModalModule]
})
export class PaperManagementPaperModelComponent {
  private modal = inject(NzModalRef);

  goTo(model) {
    this.modal.close(model);
  }
}
