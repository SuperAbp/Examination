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

import { QuestionRandomComponent } from './question-random.component';
import { QuestionSelectComponent } from './question-select.component';
import { PaperManagementPaperQuestionRuleComponent } from '../../paper-question-rule/paper-question-rule.component';

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
  selector: 'app-paper-random',
  templateUrl: './paper-random.component.html',
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
    NzDescriptionsModule,
    PaperManagementPaperQuestionRuleComponent
  ]
})
export class PaperManagementPaperRandomEditComponent implements OnInit {
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
  showQuestionModal = false;
  form: FormGroup = null;
  questions: questionTest[] = [];
  questionIds: string[] = [];

  @ViewChild('PaperQuestionRule')
  paperRepositoryComponent: PaperManagementPaperQuestionRuleComponent;

  get score() {
    return this.form.get('score');
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
      paperQuestionRules: this.fb.array([], [Validators.required])
    });
    this.addBigQuestion();
  }

  addBigQuestion() {
    this.questions.push(new questionTest(`第${simplifiedOrdinary(this.questions.length + 1)}大题`, 0.0, []));
  }
  selectQuestion(item: questionTest) {
    this.modal.createStatic(QuestionSelectComponent, { questionIds: this.questionIds }, { size: 'xl' }).subscribe(selectedQuestionIds => {
      this.questionService
        .getListWithDetail({ includeIds: selectedQuestionIds } as GetQuestionWithDetailInput)
        .pipe(
          tap(res => {
            item.items = [...item.items, ...res];
            this.questionIds = [...this.questionIds, ...selectedQuestionIds];
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
            item.items = [...item.items, ...res];
            this.questionIds = [...this.questionIds, ...res.map(x => x.id)];
          })
        )
        .subscribe();
    });
  }
  trashBigQuestion(index) {
    this.questions.splice(index, 1);
  }
  trashQuestion(questions: QuestionDetailDto[], index: number) {
    questions.splice(index, 1);
  }
  up(items, index) {
    if (index === 0) {
      return;
    }
    items[index] = items.splice(index - 1, 1, items[index])[0];
  }
  down(items, index) {
    if (index === items.length - 1) {
      return;
    }
    items[index] = items.splice(index + 1, 1, items[index])[0];
  }
  selectedQuestions() {}

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
