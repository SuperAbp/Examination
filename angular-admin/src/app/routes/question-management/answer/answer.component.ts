import { Component, Input, OnChanges, OnInit, SimpleChanges, ViewChild, inject } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { QuestionAnswerService } from '@proxy/admin/controllers';
import { GetQuestionAnswersInput, QuestionAnswerCreateDto, QuestionAnswerListDto } from '@proxy/admin/question-management/question-answers';
import { QuestionType } from '@proxy/question-management/questions';
import { forkJoin, Observable } from 'rxjs';
import { finalize, tap } from 'rxjs/operators';

interface QuestionAnswerTemp extends QuestionAnswerCreateDto {
  id?: string;
}
@Component({
  selector: 'app-question-management-answer',
  template: '',
  styles: [
    `
      button {
        margin-bottom: 10px;
      }
      nz-input-number {
        width: 100%;
      }
      .required:before {
        display: inline-block;
        margin-right: 4px;
        color: #ff4d4f;
        font-size: 14px;
        font-family: SimSun, sans-serif;
        line-height: 1;
        content: '*';
      }
    `
  ],
  standalone: true
})
export class QuestionManagementAnswerComponent {
  @Input()
  questionId: string;
  @Input()
  questionType: QuestionType;
  @Input()
  questionForm: FormGroup;

  answers: QuestionAnswerListDto[];
  removeIds: string[] = [];
  loading = false;
  params: GetQuestionAnswersInput;

  constructor(
    protected fb: FormBuilder,
    protected answerService: QuestionAnswerService
  ) {}

  get options() {
    return this.questionForm.get('options') as FormArray;
  }
  getList() {
    this.answerService
      .getList(this.params)
      .pipe(
        tap(res => {
          res.items.forEach(item => {
            this.add({
              id: item.id,
              sort: item.sort,
              right: item.right,
              content: item.content,
              analysis: item.analysis,
              questionId: this.questionId
            });
          });
        }),
        finalize(() => (this.loading = false))
      )
      .subscribe();
  }
  batchAdd(length) {
    this.options.clear();
    for (let index = 0; index < length; index++) {
      this.add({ right: false, sort: 0 });
    }
  }

  add(item: QuestionAnswerTemp = {} as QuestionAnswerTemp) {
    let fg = this.createAttribute(item);
    this.options.push(fg);
  }
  createAttribute(item: QuestionAnswerTemp) {
    return this.fb.group({
      id: [item.id || null],
      questionId: [item.questionId || this.questionId],
      right: [item.right || false],
      content: [item.content || null, [Validators.required]],
      analysis: [item.analysis || null],
      sort: [item.sort || 0]
    });
  }
  delete(index: number, item: AbstractControl) {
    this.answerService.delete(item.value.id).subscribe(() => {
      this.options.removeAt(index);
    });
  }

  resetParameters(): GetQuestionAnswersInput {
    return {
      skipCount: 0,
      maxResultCount: 10,
      questionId: this.questionId
    };
  }

  save(questionId) {
    var services: Array<Observable<any>> = [];
    this.options.controls.forEach(answer => {
      var value = answer.value;
      if (value.id) {
        services.push(this.answerService.update(value.id, value));
      } else {
        value.questionId = questionId;
        delete value.id;
        services.push(this.answerService.create(value));
      }
    });
    return forkJoin(services);
  }
}
