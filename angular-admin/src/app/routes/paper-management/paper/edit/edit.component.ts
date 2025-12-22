import { CoreModule } from '@abp/ng.core';
import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QuestionDetailDto } from '@proxy/admin/question-management/questions';
import { SharedModule } from '@shared';

import { PaperManagementPaperFixEditComponent } from './paper-fix.component';
import { PaperManagementPaperRandomEditComponent } from './paper-random.component';

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
    selector: 'app-exam-management-paper-edit',
    templateUrl: './edit.component.html',
    styles: [
        `
      [nz-radio] {
        display: block;
        height: 32px;
        line-height: 32px;
      }
      .ant-form-item-label {
        width: 95px;
      }
      .ant-input {
        width: 120px;
      }
      .box {
        border: 1px solid #ddd;
        padding: 10px;
        margin: 15px 0;
        border-radius: 4px;
      }
    `
    ],
    imports: [SharedModule, CoreModule, PaperManagementPaperFixEditComponent, PaperManagementPaperRandomEditComponent]
})
export class PaperManagementPaperEditComponent implements OnInit {
  private route = inject(ActivatedRoute);

  model: number;

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.model = +params['model'];
    });
  }
}
