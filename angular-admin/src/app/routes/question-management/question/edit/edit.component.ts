import { LocalizationService } from '@abp/ng.core';
import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EnumService, QuestionAnswerService, QuestionRepoService, QuestionService } from '@proxy/super-abp/exam/admin/controllers';
import { QuestionRepoListDto } from '@proxy/super-abp/exam/admin/question-management/question-repos';
import { GetQuestionForEditorOutput } from '@proxy/super-abp/exam/admin/question-management/questions';
import { QuestionType } from '@proxy/super-abp/exam/question-management/questions';
import { NzMessageService } from 'ng-zorro-antd/message';
import { forkJoin, from, Observable } from 'rxjs';
import { finalize, tap } from 'rxjs/operators';
import { QuestionManagementAnswerComponent } from '../../answer/answer.component';

@Component({
  selector: 'app-question-management-question-edit',
  templateUrl: './edit.component.html'
})
export class QuestionManagementQuestionEditComponent implements OnInit {
  questionId: string;
  @ViewChild('QuestionAnswer')
  questionAnswerComponent: QuestionManagementAnswerComponent;

  question: GetQuestionForEditorOutput;

  loading = false;
  isConfirmLoading = false;
  questionTypes: Array<{ label: string; value: number }> = [];
  questionRepositories: QuestionRepoListDto[];

  form: FormGroup = null;

  get options() {
    return this.form.get('options') as FormArray;
  }
  get questionType() {
    return this.form.get('questionType');
  }

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private messageService: NzMessageService,
    private localizationService: LocalizationService,
    private questionService: QuestionService,
    private questionRepoService: QuestionRepoService,
    private answerService: QuestionAnswerService,
    private enumService: EnumService
  ) {}

  ngOnInit(): void {
    this.loading = true;
    this.route.paramMap.subscribe(params => {
      let id = params.get('id');
      this.questionId = id;
      if (this.questionId) {
        this.questionService
          .getEditor(this.questionId)
          .pipe(
            tap(response => {
              this.question = response;
              this.buildForm();
              this.loading = false;
            })
          )
          .subscribe();
      } else {
        this.question = {} as GetQuestionForEditorOutput;
        this.buildForm();
        this.loading = false;
      }
    });
  }

  buildForm() {
    this.questionRepoService
      .getList({ skipCount: 0, maxResultCount: 100 })
      .pipe(
        tap(res => {
          Object.keys(QuestionType)
            .filter(k => !isNaN(Number(k)))
            .map(key => {
              this.questionTypes.push({ label: this.localizationService.instant('Exam::QuestionType:' + key), value: +key });
            });
          this.questionRepositories = res.items;

          this.form = this.fb.group({
            content: [this.question.content || '', [Validators.required]],
            analysis: [this.question.analysis || ''],
            questionType: [this.question.questionType >= 0 ? this.question.questionType : null, [Validators.required]],
            questionRepositoryId: [this.question.questionRepositoryId || '', [Validators.required]],
            options: this.fb.array([], [Validators.required])
          });
        })
      )
      .subscribe();
  }

  save() {
    if (!this.form.valid || this.isConfirmLoading) {
      for (const key of Object.keys(this.form.controls)) {
        this.form.controls[key].markAsDirty();
        this.form.controls[key].updateValueAndValidity();
      }
      return;
    }
    this.isConfirmLoading = true;

    if (this.questionId) {
      let services = this.getAnswerSaveService();
      services.push(
        this.questionService.update(this.questionId, {
          ...this.question,
          ...this.form.value
        })
      );
      forkJoin(services)
        .pipe(
          tap(() => {
            this.goback();
          }),
          finalize(() => (this.isConfirmLoading = false))
        )
        .subscribe();
    } else {
      this.questionService
        .create({
          ...this.form.value
        })
        .pipe(
          tap(res => {
            this.questionId = res.id;
            forkJoin(this.getAnswerSaveService())
              .pipe(
                tap(() => {
                  this.goback();
                }),
                finalize(() => (this.isConfirmLoading = false))
              )
              .subscribe();
          })
        )
        .subscribe();
    }
  }
  getAnswerSaveService() {
    var services: Array<Observable<any>> = [];
    this.options.controls.forEach(answer => {
      var value = answer.value;
      if (value.id) {
        services.push(this.answerService.update(value.id, value));
      } else {
        value.questionId = this.questionId;
        delete value.id;
        services.push(this.answerService.create(value));
      }
    });
    return services;
  }
  back(e: MouseEvent) {
    e.preventDefault();
    this.goback();
  }
  goback() {
    this.router.navigateByUrl('/question-management/question');
  }
}
