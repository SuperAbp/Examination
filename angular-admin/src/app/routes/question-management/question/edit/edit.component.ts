import { LocalizationService } from '@abp/ng.core';
import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EnumService, QuestionRepoService, QuestionService } from '@proxy/super-abp/exam/admin/controllers';
import { QuestionRepoListDto } from '@proxy/super-abp/exam/admin/question-management/question-repos';
import { GetQuestionForEditorOutput } from '@proxy/super-abp/exam/admin/question-management/questions';
import { NzMessageService } from 'ng-zorro-antd/message';
import { forkJoin, from } from 'rxjs';
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

  get questionType() {
    return this.form.get('questionType');
  }

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private localizationService: LocalizationService,
    private questionService: QuestionService,
    private questionRepoService: QuestionRepoService,
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
    const questionRepositoryRequest$ = from(this.questionRepoService.getList({ skipCount: 0, maxResultCount: 100 }));
    const questionTypeRequest$ = from(this.enumService.getQuestionType());
    forkJoin([questionTypeRequest$, questionRepositoryRequest$])
      .pipe(
        tap(res => {
          Object.keys(res[0]).map(key => {
            this.questionTypes.push({ label: res[0][key], value: +key });
          });
          this.questionRepositories = res[1].items;

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
      this.questionService
        .update(this.questionId, {
          ...this.question,
          ...this.form.value
        })
        .pipe(
          tap(() => {
            this.questionAnswerComponent
              .save(this.questionId)
              .pipe(
                tap(() => {
                  this.goback();
                }),
                finalize(() => (this.isConfirmLoading = false))
              )
              .subscribe();
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
            this.questionAnswerComponent
              .save(res.id)
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
  back(e: MouseEvent) {
    e.preventDefault();
    this.goback();
  }
  goback() {
    this.router.navigateByUrl('/question-management/question');
  }
}
