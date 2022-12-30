import { ConfigStateService, COOKIE_LANGUAGE_KEY, LocalizationService, PermissionService } from '@abp/ng.core';
import { AfterViewInit, Component, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { STChange, STColumn, STComponent, STData, STPage } from '@delon/abc/st';
import { SFSchema } from '@delon/form';
import { ModalHelper } from '@delon/theme';
import { QuestionAnswerService } from '@proxy/super-abp/exam/admin/controllers';
import {
  GetQuestionAnswersInput,
  QuestionAnswerCreateDto,
  QuestionAnswerListDto
} from '@proxy/super-abp/exam/admin/question-management/question-answers';
import { QuestionType } from '@proxy/super-abp/exam/question-management/questions';
import { NzMessageService } from 'ng-zorro-antd/message';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-question-management-answer',
  templateUrl: './answer.component.html'
})
export class QuestionManagementAnswerComponent implements OnInit, OnChanges {
  @Input()
  questionId: string;
  @Input()
  questionType: QuestionType;
  @Input()
  questionForm: FormGroup;

  answers: QuestionAnswerListDto[];
  loading = false;
  params: GetQuestionAnswersInput;
  page: STPage = {
    front: false,
    show: false
  };
  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[];

  constructor(
    private modal: ModalHelper,
    private fb: FormBuilder,
    private localizationService: LocalizationService,
    private messageService: NzMessageService,
    private permissionService: PermissionService,
    private answerService: QuestionAnswerService
  ) {}

  get options() {
    return this.questionForm.get('options') as FormArray;
  }
  ngOnChanges(changes: SimpleChanges): void {
    this.loaded();
  }

  ngOnInit() {
    this.params = this.resetParameters();
    this.loaded();
  }

  loaded() {
    if (this.questionId) {
      this.getList();
    } else {
      this.options.clear();
      let len = 2;
      if (this.questionType === QuestionType.SingleSelect || this.questionType == QuestionType.MultiSelect) {
        len = 4;
      }
      for (let index = 0; index < len; index++) {
        this.add();
      }
    }
  }
  getList() {
    this.loading = true;
    this.answerService
      .getList(this.params)
      .pipe(tap(() => (this.loading = false)))
      .subscribe(response => (this.answers = response.items));
  }

  add(item: QuestionAnswerCreateDto = {} as QuestionAnswerCreateDto) {
    let fg = this.createAttribute(item);
    this.options.push(fg);
  }
  createAttribute(item: QuestionAnswerCreateDto) {
    return this.fb.group({
      questionId: [item.questionId || this.questionId],
      right: [item.right || false],
      content: [item.content || null, [Validators.required]],
      analysis: [item.analysis || null]
    });
  }
  delete(index: number) {
    this.options.removeAt(index);
  }

  resetParameters(): GetQuestionAnswersInput {
    return {
      skipCount: 0,
      maxResultCount: 10,
      sorting: 'Id Desc',
      questionId: this.questionId
    };
  }

  checkbox(index, value: boolean): void {
    this.st.setRow(index, { right: value });
  }

  radio(index): void {
    // for (let i = 0; i < this.answers.length; i++) {
    //   const answer = this.answers[i];
    //   if (i === index) {
    //     answer.right = true;
    //   } else {
    //     answer.right = false;
    //   }
    // }
    // this.answers = [...this.answers];
    this.st.setRow(index, { right: true });
  }
}
