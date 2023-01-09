import { Component, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { STColumn, STComponent, STPage } from '@delon/abc/st';
import { ModalHelper } from '@delon/theme';
import { QuestionAnswerService } from '@proxy/super-abp/exam/admin/controllers';
import {
  GetQuestionAnswersInput,
  QuestionAnswerCreateDto,
  QuestionAnswerListDto
} from '@proxy/super-abp/exam/admin/question-management/question-answers';
import { QuestionType } from '@proxy/super-abp/exam/question-management/questions';
import { forkJoin, Observable } from 'rxjs';
import { finalize, tap } from 'rxjs/operators';

interface QuestionAnswerTemp extends QuestionAnswerCreateDto {
  id?: string;
}
@Component({
  selector: 'app-question-management-answer',
  templateUrl: './answer.component.html',
  styles: [
    `
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
  ]
})
export class QuestionManagementAnswerComponent implements OnInit, OnChanges {
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
  page: STPage = {
    front: false,
    show: false
  };
  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[];
  answerEditorItems: QuestionAnswerCreateDto[];

  constructor(private modal: ModalHelper, private fb: FormBuilder, private answerService: QuestionAnswerService) {}

  get options() {
    return this.questionForm.get('options') as FormArray;
  }
  ngOnChanges(changes: SimpleChanges): void {
    this.loaded();
  }

  ngOnInit() {
    this.loaded();
  }

  loaded() {
    if (this.loading) {
      return;
    }
    this.loading = true;
    this.params = this.resetParameters();
    if (this.questionId) {
      this.getList();
    } else {
      this.options.clear();
      let len = 2;
      if (this.questionType === QuestionType.SingleSelect || this.questionType == QuestionType.MultiSelect) {
        len = 4;
      }
      for (let index = 0; index < len; index++) {
        this.add({ right: false, sort: 0 });
      }
      this.loading = false;
    }
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
    if (item.value.id) {
      this.removeIds.push(item.value.id);
    }
    this.options.removeAt(index);
  }

  resetParameters(): GetQuestionAnswersInput {
    return {
      skipCount: 0,
      maxResultCount: 10,
      sorting: 'CreationTime ASC',
      questionId: this.questionId
    };
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
    if (this.removeIds.length > 0) {
      this.removeIds.forEach(id => {
        services.push(this.answerService.delete(id));
      });
    }
    return forkJoin(services);
  }
}
