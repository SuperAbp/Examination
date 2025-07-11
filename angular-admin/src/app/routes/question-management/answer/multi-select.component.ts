import { CoreModule } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { QuestionService } from '@proxy/admin/controllers';
import { QuestionAnswerDto } from '@proxy/admin/question-management/questions';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';

import { QuestionManagementAnswerComponent } from './answer.component';
interface QuestionAnswerTemp extends QuestionAnswerDto {
  id?: string;
}

@Component({
  selector: 'multi-select',
  templateUrl: './multi-select.component.html',
  standalone: true,
  imports: [
    CoreModule,
    NzButtonModule,
    NzTableModule,
    NzFormModule,
    NzCheckboxModule,
    NzInputModule,
    NzInputNumberModule,
    NzPopconfirmModule,
    NzToolTipModule,
    NzIconModule
  ]
})
export class MultiSelectComponent extends QuestionManagementAnswerComponent {
  constructor(
    protected override fb: FormBuilder,
    protected override questionService: QuestionService
  ) {
    super(fb, questionService);
  }
}
