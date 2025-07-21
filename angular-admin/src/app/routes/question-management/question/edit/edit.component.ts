import { CoreModule, LocalizationService } from '@abp/ng.core';
import { Component, OnInit, ViewChild, inject, ChangeDetectorRef } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FooterToolbarModule } from '@delon/abc/footer-toolbar';
import { PageHeaderModule } from '@delon/abc/page-header';
import { KnowledgePointService, OptionService, QuestionBankService, QuestionService } from '@proxy/admin/controllers';
import { QuestionBankListDto } from '@proxy/admin/question-management/question-banks';
import { GetQuestionForEditorOutput } from '@proxy/admin/question-management/questions';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzTreeSelectModule } from 'ng-zorro-antd/tree-select';
import { forkJoin, Observable } from 'rxjs';
import { expand, finalize, map, tap } from 'rxjs/operators';

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
    BlankComponent,
    NzTreeSelectModule
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
  private questionBankService = inject(QuestionBankService);
  private optionService = inject(OptionService);
  private knowledgePointService = inject(KnowledgePointService);
  private cdr = inject(ChangeDetectorRef);

  loading = false;
  isConfirmLoading = false;
  questionTypes: Array<{ label: string; value: number }> = [];
  questionBanks: QuestionBankListDto[];

  form: FormGroup = null;
  nodes: any[] = [];

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

      this.knowledgePointService
        .getAll({})
        .pipe(
          tap(response => {
            this.nodes = this.mapTreeData(response.items);
          })
        )
        .subscribe();
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
        this.question = { answers: [] } as GetQuestionForEditorOutput;
        this.buildForm();
        this.loading = false;
      }
    });
  }
  mapTreeData(data: any[]): any[] {
    return data.map(item => ({
      title: item.name,
      key: item.id,
      isLeaf: item.children.length === 0,
      children: item.children ? this.mapTreeData(item.children) : []
    }));
  }

  buildForm() {
    this.questionBankService
      .getList({ skipCount: 0, maxResultCount: 100 })
      .pipe(
        tap(res => {
          this.optionService
            .getQuestionTypes()
            .pipe(
              map(res => {
                Object.keys(res).forEach(key => {
                  this.questionTypes.push({
                    label: this.localizationService.instant(`Exam::QuestionType:${key}`),
                    value: +key
                  });
                });
              })
            )
            .subscribe();
          this.questionBanks = res.items;

          this.form = this.fb.group({
            content: [this.question.content || '', [Validators.required]],
            analysis: [this.question.analysis || ''],
            questionType: [this.question.questionType >= 0 ? this.question.questionType : null, [Validators.required]],
            questionBankId: [this.question.questionBankId || '', [Validators.required]],
            knowledgePointIds: [this.question.knowledgePointIds || []],
            options: this.fb.array([], [Validators.required])
          });
          // 修复 ExpressionChangedAfterItHasBeenCheckedError
          this.cdr.detectChanges();
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
  back(e: MouseEvent) {
    e.preventDefault();
    this.goback();
  }
  goback() {
    this.router.navigateByUrl('/question-management/question');
  }
}
