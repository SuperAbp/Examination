import { CoreModule } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { QuestionService } from '@proxy/admin/controllers';
import { QuestionOptionDto } from '@proxy/admin/question-management/questions';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberLegacyModule } from 'ng-zorro-antd/input-number-legacy';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTooltipModule } from 'ng-zorro-antd/tooltip';

import { QuestionManagementAnswerComponent } from './answer.component';

interface QuestionOptionTemp extends QuestionOptionDto {
  id?: string;
}

@Component({
  selector: 'blank',
  templateUrl: './blank.component.html',
  imports: [
    CoreModule,
    NzButtonModule,
    NzTableModule,
    NzFormModule,
    NzInputModule,
    NzInputNumberLegacyModule,
    NzPopconfirmModule,
    NzTooltipModule,
    NzIconModule
  ]
})
export class BlankComponent extends QuestionManagementAnswerComponent {
  constructor(
    protected override fb: FormBuilder,
    protected override questionService: QuestionService
  ) {
    super(fb, questionService);
  }

  override createAttribute(item: QuestionOptionTemp) {
    return this.fb.group({
      id: [item.id || null],
      right: [true],
      content: [item.content || null, [Validators.required]],
      analysis: [item.analysis || null],
      sort: [item.sort || 0]
    });
  }
}
