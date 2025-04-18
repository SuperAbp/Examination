import { CoreModule, LocalizationService } from '@abp/ng.core';
import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FooterToolbarModule } from '@delon/abc/footer-toolbar';
import { PageHeaderModule } from '@delon/abc/page-header';
import { OptionService, QuestionAnswerService, QuestionRepoService, QuestionService } from '@proxy/admin/controllers';
import { QuestionRepoListDto } from '@proxy/admin/question-management/question-repos';
import { GetQuestionForEditorOutput } from '@proxy/admin/question-management/questions';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { forkJoin, Observable } from 'rxjs';
import { finalize, map, tap } from 'rxjs/operators';

import { QuestionManagementAnswerComponent } from '../../answer/answer.component';
import { BlankComponent } from '../../answer/blank.component';
import { JudgeComponent } from '../../answer/judge.component';
import { MultiSelectComponent } from '../../answer/multi-select.component';
import { SingleSelectComponent } from '../../answer/single-select.component';

@Component({
  selector: 'app-question-management-question-edit',
  templateUrl: './edit.component.html',
  standalone: true,
  imports: [
    CoreModule,
    PageHeaderModule,
    FooterToolbarModule,
    NzSpinModule,
    NzCardModule,
    NzGridModule,
    NzFormModule,
    NzSelectModule,
    NzInputModule,
    NzButtonModule,
    JudgeComponent,
    SingleSelectComponent,
    MultiSelectComponent,
    BlankComponent
  ]
})
export class QuestionManagementQuestionEditComponent implements OnInit {
  questionId: string;
  @ViewChild('QuestionAnswer')
  questionAnswerComponent: QuestionManagementAnswerComponent;

  question: GetQuestionForEditorOutput;

  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private localizationService = inject(LocalizationService);
  private questionService = inject(QuestionService);
  private questionRepoService = inject(QuestionRepoService);
  private answerService = inject(QuestionAnswerService);
  private optionService = inject(OptionService);

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
          this.optionService
            .getQuestionTypes()
            .pipe(
              map(res => {
                Object.keys(res).forEach(key => {
                  this.questionTypes.push({
                    label: this.localizationService.instant(`Exam::QuestionType:${res[key].value}`),
                    value: res[key].value
                  });
                });
              })
            )
            .subscribe();
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
      this.questionService
        .update(this.questionId, {
          ...this.question,
          ...this.form.value
        })
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
          tap(() => {
            this.goback();
          }),
          finalize(() => (this.isConfirmLoading = false))
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
