import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { QuestionManagementAnswerComponent } from './answer.component';
import { CoreModule } from '@abp/ng.core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { QuestionAnswerCreateDto } from '@proxy/admin/question-management/question-answers';
import { QuestionAnswerService } from '@proxy/admin/controllers';

interface QuestionAnswerTemp extends QuestionAnswerCreateDto {
  id?: string;
}

@Component({
  selector: 'blank',
  templateUrl: './blank.component.html',
  standalone: true,
  imports: [
    CoreModule,
    NzButtonModule,
    NzTableModule,
    NzFormModule,
    NzInputModule,
    NzInputNumberModule,
    NzPopconfirmModule,
    NzToolTipModule,
    NzIconModule
  ]
})
export class BlankComponent extends QuestionManagementAnswerComponent implements OnInit {
  constructor(
    protected override fb: FormBuilder,
    protected override answerService: QuestionAnswerService
  ) {
    super(fb, answerService);
  }

  ngOnInit(): void {
    this.params = this.resetParameters();
    if (this.questionId) {
      this.getList();
    } else {
      this.batchAdd(2);
    }
  }
}
