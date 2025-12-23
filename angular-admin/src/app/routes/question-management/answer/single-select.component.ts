import { CoreModule } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { QuestionService } from '@proxy/admin/controllers';
import { QuestionOptionDto } from '@proxy/admin/question-management/questions';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberLegacyModule } from 'ng-zorro-antd/input-number-legacy';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTooltipModule } from 'ng-zorro-antd/tooltip';

import { QuestionManagementAnswerComponent } from './answer.component';
interface QuestionOptionTemp extends QuestionOptionDto {
  id?: string;
}

@Component({
  selector: 'single-select',
  templateUrl: './single-select.component.html',
  imports: [
    CoreModule,
    NzButtonModule,
    NzTableModule,
    NzFormModule,
    NzRadioModule,
    NzInputModule,
    NzInputNumberLegacyModule,
    NzPopconfirmModule,
    NzTooltipModule,
    NzIconModule
  ]
})
export class SingleSelectComponent extends QuestionManagementAnswerComponent {
  constructor(
    protected override fb: FormBuilder,
    protected override questionService: QuestionService
  ) {
    super(fb, questionService);
  }

  changeRadio(index: number, item) {
    this.options.controls.forEach((c, i) => {
      if (i != index && c['controls']['right'].value) {
        c['controls']['right'].setValue(false);
      }
    });
    if (!item.right) {
      item.right = true;
    }
  }
}
