import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { QuestionManagementAnswerComponent } from './answer.component';
import { CoreModule } from '@abp/ng.core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { QuestionAnswerService } from '@proxy/admin/controllers';
import { QuestionAnswerCreateDto } from '@proxy/admin/question-management/question-answers';
interface QuestionAnswerTemp extends QuestionAnswerCreateDto {
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
export class MultiSelectComponent extends QuestionManagementAnswerComponent implements OnInit {
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
      this.batchAdd(4);
    }
  }
}
