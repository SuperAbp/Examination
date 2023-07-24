import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { QuestionAnswerService } from '@proxy/super-abp/exam/admin/controllers';
import { QuestionAnswerCreateDto } from '@proxy/super-abp/exam/admin/question-management/question-answers';
import { QuestionManagementAnswerComponent } from './answer.component';

interface QuestionAnswerTemp extends QuestionAnswerCreateDto {
  id?: string;
}

@Component({
  selector: 'blank',
  templateUrl: './blank.component.html'
})
export class BlankComponent extends QuestionManagementAnswerComponent implements OnInit {
  constructor(protected override fb: FormBuilder, protected override answerService: QuestionAnswerService) {
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
