import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { QuestionAnswerService } from '@proxy/super-abp/exam/admin/controllers';
import { QuestionAnswerCreateDto } from '@proxy/super-abp/exam/admin/question-management/question-answers';
import { QuestionManagementAnswerComponent } from './answer.component';

interface QuestionAnswerTemp extends QuestionAnswerCreateDto {
  id?: string;
}

@Component({
  selector: 'judge',
  templateUrl: './judge.component.html'
})
export class JudgeComponent extends QuestionManagementAnswerComponent implements OnInit {
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
