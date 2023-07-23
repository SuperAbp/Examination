import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { QuestionAnswerService } from '@proxy/super-abp/exam/admin/controllers';
import { GetQuestionAnswersInput, QuestionAnswerCreateDto } from '@proxy/super-abp/exam/admin/question-management/question-answers';
import { finalize, tap } from 'rxjs';
import { QuestionManagementAnswerComponent } from './answer.component';

interface QuestionAnswerTemp extends QuestionAnswerCreateDto {
  id?: string;
}

@Component({
  selector: 'single-select',
  templateUrl: './single-select.component.html'
})
export class SingleSelectComponent extends QuestionManagementAnswerComponent implements OnInit {
  constructor(protected override fb: FormBuilder, protected override answerService: QuestionAnswerService) {
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
