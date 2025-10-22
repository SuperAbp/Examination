import { CoreModule } from '@abp/ng.core';
import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FooterToolbarModule } from '@delon/abc/footer-toolbar';
import { PageHeaderModule } from '@delon/abc/page-header';
import { ModalHelper } from '@delon/theme';
import { dateTimePickerUtil } from '@delon/util';
import { PaperService, QuestionService } from '@proxy/admin/controllers';
import { GetPaperForEditorOutput } from '@proxy/admin/paper-management/papers';
import { GetQuestionWithDetailInput, QuestionDetailDto } from '@proxy/admin/question-management/questions';
import { SharedModule, simplifiedOrdinary } from '@shared';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { NzFlexModule } from 'ng-zorro-antd/flex';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzSpaceComponent, NzSpaceModule } from 'ng-zorro-antd/space';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { finalize, tap } from 'rxjs/operators';

import { PaperManagementPaperQuestionRuleComponent } from '../../paper-question-rule/paper-question-rule.component';
import { QuestionRandomComponent } from './question-random.component';
import { QuestionSelectComponent } from './question-select.component';

export class questionTest {
  constructor(name: string, score: number, items: QuestionDetailDto[]) {
    this.name = name;
    this.score = score;
    this.items = items;
  }
  name: string;
  score: number;
  items: QuestionDetailDto[];
}
@Component({
  selector: 'app-paper-fix',
  templateUrl: './paper-fix.component.html',
  styles: [
    `
      [nz-radio] {
        display: block;
        height: 32px;
        line-height: 32px;
      }
      .ant-form-item-label {
        width: 95px;
      }
      .ant-input {
        width: 120px;
      }
      .box {
        border: 1px solid #ddd;
        padding: 10px;
        margin: 15px 0;
        border-radius: 4px;
      }
    `
  ],
  standalone: true,
  imports: [
    SharedModule,
    CoreModule,
    PageHeaderModule,
    FooterToolbarModule,
    NzSpinModule,
    NzCardModule,
    NzFormModule,
    NzIconModule,
    NzInputModule,
    NzListModule,
    NzInputNumberModule,
    NzButtonModule,
    NzFlexModule,
    NzSpaceModule,
    NzModalModule,
    NzRadioModule,
    NzDescriptionsModule
  ]
})
export class PaperManagementPaperFixEditComponent implements OnInit {
  private modal = inject(ModalHelper);
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private paperService = inject(PaperService);
  private questionService = inject(QuestionService);
  paperId: string;
  paper: GetPaperForEditorOutput;

  loading = false;
  isConfirmLoading = false;
  showPaperTime: boolean;
  form: FormGroup = null;
  questionIds: string[] = [];

  get score() {
    return this.form.get('score');
  }

  // expose questions for template iteration (form is single source of truth)
  get questions(): questionTest[] {
    return this.getQuestions();
  }
  range(start: number, end: number): number[] {
    const result: number[] = [];
    for (let i = start; i < end; i++) {
      result.push(i);
    }
    return result;
  }
  disabledDate = (current: Date): boolean => dateTimePickerUtil.getDiffDays(current, new Date()) < 0;

  ngOnInit(): void {
    this.loading = true;
    this.route.paramMap.subscribe(params => {
      let id = params.get('id');
      this.paperId = id;
      if (this.paperId) {
        this.paperService
          .getEditor(this.paperId)
          .pipe(
            tap(response => {
              this.paper = response;
              this.buildForm();
              this.loading = false;
            })
          )
          .subscribe();
      } else {
        this.paper = {} as GetPaperForEditorOutput;
        this.buildForm();
        this.loading = false;
      }
    });
  }

  buildForm() {
    this.form = this.fb.group({
      name: [this.paper.name || '', [Validators.required]],
      description: [this.paper.description || ''],
      score: [this.paper.score || 0],
      // store questions array on the form so it will be submitted
      questions: []
    });
    this.addBigQuestion();
  }

  addBigQuestion() {
    const qs = this.getQuestions();
    qs.push(new questionTest(`第${simplifiedOrdinary(qs.length + 1)}大题`, 0.0, []));
    this.setQuestions(qs);
  }
  selectQuestion(item: questionTest) {
    this.modal.createStatic(QuestionSelectComponent, { questionIds: this.questionIds }, { size: 'xl' }).subscribe(selectedQuestionIds => {
      this.questionService
        .getListWithDetail({ includeIds: selectedQuestionIds } as GetQuestionWithDetailInput)
        .pipe(
          tap(res => {
            // find and update the question inside form.questions
            const qs = this.getQuestions();
            const idx = qs.indexOf(item);
            if (idx > -1) {
              qs[idx] = { ...qs[idx], items: [...(qs[idx].items || []), ...res] };
              this.setQuestions(qs);
            }
            this.questionIds = [...this.questionIds, ...selectedQuestionIds];
            this.recomputeTotal();
          })
        )
        .subscribe();
    });
  }
  randomAdditionQuestion(item: questionTest) {
    this.modal.createStatic(QuestionRandomComponent, { selectedQuestions: item.items }, { size: 'xl' }).subscribe(params => {
      this.questionService
        .getListWithDetail({
          questionBankId: params.questionBankId,
          questionType: params.questionType,
          count: params.count,
          excludeIds: this.questionIds
        } as GetQuestionWithDetailInput)
        .pipe(
          tap(res => {
            const qs = this.getQuestions();
            const idx = qs.indexOf(item);
            if (idx > -1) {
              qs[idx] = { ...qs[idx], items: [...(qs[idx].items || []), ...res] };
              this.setQuestions(qs);
            }
            this.questionIds = [...this.questionIds, ...res.map(x => x.id)];
            this.recomputeTotal();
          })
        )
        .subscribe();
    });
  }
  trashBigQuestion(index) {
    const qs = this.getQuestions();
    qs.splice(index, 1);
    this.setQuestions(qs);
    this.recomputeTotal();
  }
  trashQuestion(question: questionTest, itemIndex: number) {
    const qs = this.getQuestions();
    const questionIndex = qs.indexOf(question);
    if (questionIndex > -1) {
      // 直接修改对应大题的 items 数组
      qs[questionIndex] = {
        ...qs[questionIndex],
        items: qs[questionIndex].items.filter((_, index) => index !== itemIndex)
      };
      this.setQuestions(qs);
      this.recomputeTotal();
    }
  }
  up(items, index) {
    if (index === 0) {
      return;
    }
    const qs = this.getQuestions();
    const idx = qs.findIndex(q => q.items === items || q.items === items);
    if (idx === -1) return;
    const q = qs[idx];
    const newItems = [...(q.items || [])];
    newItems[index] = newItems.splice(index - 1, 1, newItems[index])[0];
    qs[idx] = { ...q, items: newItems };
    this.setQuestions(qs);
  }
  down(items, index) {
    if (index === items.length - 1) {
      return;
    }
    const qs = this.getQuestions();
    const idx = qs.findIndex(q => q.items === items || q.items === items);
    if (idx === -1) return;
    const q = qs[idx];
    const newItems = [...(q.items || [])];
    newItems[index] = newItems.splice(index + 1, 1, newItems[index])[0];
    qs[idx] = { ...q, items: newItems };
    this.setQuestions(qs);
  }

  // Called when a question type's score input changes
  onQuestionScoreChange() {
    this.recomputeTotal();
    // persist any inline changes back to form control
    this.setQuestions(this.getQuestions());
  }

  // Recompute total score from questions and update the form control
  recomputeTotal() {
    try {
      if (!this.form) return;
      let total = 0;
      const qs = this.getQuestions();
      for (const q of qs) {
        const scorePerQuestion = Number(q.score) || 0;
        const count = q.items ? q.items.length : 0;
        total += scorePerQuestion * count;
      }
      // update form control without emitting event loop issues
      if (this.form && this.form.get('score')) {
        this.form.get('score').setValue(total);
      }
      // questions are stored in form; nothing else to sync here
    } catch (e) {
      // swallow errors to avoid breaking UI; could log if logger available
      // console.warn('recomputeTotal error', e);
    }
  }
  // helpers to read/write questions from/to form control
  private getQuestions(): questionTest[] {
    if (!this.form) return [];
    return (this.form.get('questions')?.value as questionTest[]) || [];
  }

  private setQuestions(qs: questionTest[]) {
    if (!this.form) return;
    this.form.get('questions').setValue(qs);
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

    if (this.paperId) {
      this.paperService
        .update(this.paperId, {
          ...this.paper,
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
      this.paperService
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

  changeTotalScore(e) {
    this.score.setValue(e);
  }

  back(e: MouseEvent) {
    e.preventDefault();
    this.goback();
  }
  goback() {
    this.router.navigateByUrl('/paper-management/paper');
  }
}
