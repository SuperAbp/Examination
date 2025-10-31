import { CoreModule } from '@abp/ng.core';
import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FooterToolbarModule } from '@delon/abc/footer-toolbar';
import { PageHeaderModule } from '@delon/abc/page-header';
import { ModalHelper } from '@delon/theme';
import { dateTimePickerUtil } from '@delon/util';
import { PaperService, QuestionService } from '@proxy/admin/controllers';
import {
  GetPaperForEditorOutput,
  PaperCreateOrUpdateDtoBase_PaperSectionDto,
  PaperCreateOrUpdateDtoBase_PaperSectionDto_PaperQuestionDto
} from '@proxy/admin/paper-management/papers';
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
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { finalize, tap } from 'rxjs/operators';

import { QuestionRandomComponent } from './question-random.component';
import { QuestionSelectComponent } from './question-select.component';

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
  questionDetails = new Map<string, QuestionDetailDto>();

  get score() {
    return this.form.get('score');
  }

  get sections() {
    return this.form?.get('sections') as FormArray;
  }
  getQuestionsArray(sectionIndex: number): FormArray {
    return this.sections.at(sectionIndex).get('paperQuestions') as FormArray;
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
              this.questionIds = this.paper.sections.flatMap(s => s.paperQuestions.map(q => q.questionId));
              this.getQuesions(this.questionIds);
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
      paperType: [0],
      score: [this.paper.score || 0, [Validators.required, Validators.min(1)]],
      sections: this.fb.array((this.paper.sections || []).map(s => this.createSection(s)))
    });
    console.log(this.form.value);
  }
  createSection(section?: PaperCreateOrUpdateDtoBase_PaperSectionDto) {
    return this.fb.group({
      id: [section?.id || ''],
      title: [section?.title || '', [Validators.required]],
      scoreEach: [section?.scoreEach, [Validators.required]],
      totalScore: [section?.totalScore, [Validators.required]],
      totalCount: [section?.totalCount, [Validators.required]],
      order: [section?.order || 0],
      remark: [section?.remark || ''],
      paperQuestions: this.fb.array((section?.paperQuestions || []).map(q => this.createQuestion(q)))
    });
  }
  createQuestion(question?: PaperCreateOrUpdateDtoBase_PaperSectionDto_PaperQuestionDto) {
    return this.fb.group({
      questionId: [question?.questionId || '', [Validators.required]],
      score: [question?.score, [Validators.required, Validators.min(1)]],
      order: [question?.order || 0]
    });
  }

  addSection() {
    this.sections.push(
      this.createSection({
        title: `第${simplifiedOrdinary(this.sections.length + 1)}大题`,
        scoreEach: 0
      } as PaperCreateOrUpdateDtoBase_PaperSectionDto)
    );
  }
  getQuesions(ids: string[]) {
    this.questionService
      .getListWithDetail({ includeIds: ids } as GetQuestionWithDetailInput)
      .pipe(
        tap(res => {
          res.forEach(q => this.questionDetails.set(q.id, q));
        })
      )
      .subscribe();
  }
  selectQuestion(sectionIndex: number) {
    this.modal.createStatic(QuestionSelectComponent, { questionIds: this.questionIds }, { size: 'xl' }).subscribe(selectedQuestionIds => {
      this.questionService
        .getListWithDetail({ includeIds: selectedQuestionIds } as GetQuestionWithDetailInput)
        .pipe(
          tap(res => {
            res.forEach(q => this.questionDetails.set(q.id, q));
            const sectionsArray = this.sections;
            if (sectionIndex > -1) {
              const sectionGroup = sectionsArray.at(sectionIndex);
              const questionsArray = sectionGroup.get('paperQuestions') as any;
              const scoreEach = sectionGroup.get('scoreEach')?.value || 0;
              res.forEach(q => {
                questionsArray.push(this.createQuestion({ questionId: q.id, score: scoreEach } as any));
              });
            }
            this.questionIds = [...this.questionIds, ...selectedQuestionIds];
            this.recomputeTotal();
          })
        )
        .subscribe();
    });
  }
  randomAdditionQuestion(sectionIndex: number) {
    const section = this.sections.value[sectionIndex];
    this.modal.createStatic(QuestionRandomComponent, { selectedQuestions: section.paperQuestions }, { size: 'xl' }).subscribe(params => {
      this.questionService
        .getListWithDetail({
          questionBankId: params.questionBankId,
          questionType: params.questionType,
          count: params.count,
          excludeIds: this.questionIds
        } as GetQuestionWithDetailInput)
        .pipe(
          tap(res => {
            res.forEach(q => this.questionDetails.set(q.id, q));
            const sectionsArray = this.sections;
            if (sectionIndex > -1) {
              const sectionGroup = sectionsArray.at(sectionIndex);
              const questionsArray = sectionGroup.get('paperQuestions') as any;
              const scoreEach = sectionGroup.get('scoreEach')?.value || 0;
              res.forEach(q => {
                questionsArray.push(this.createQuestion({ questionId: q.id, score: scoreEach } as any));
              });
            }
            this.questionIds = [...this.questionIds, ...res.map(x => x.id)];
            this.recomputeTotal();
          })
        )
        .subscribe();
    });
  }
  trashBigQuestion(index) {
    const sectionsArray = this.sections;
    sectionsArray.removeAt(index);
    this.recomputeTotal();
  }
  trashQuestion(sectionIndex: number, itemIndex: number) {
    const sectionsArray = this.sections;
    if (sectionIndex > -1) {
      const sectionGroup = sectionsArray.at(sectionIndex);
      const questionsArray = sectionGroup.get('paperQuestions') as any;
      questionsArray.removeAt(itemIndex);
      this.recomputeTotal();
    }
  }
  up(sectionIndex: number, questionIndex: number) {
    if (questionIndex === 0) {
      return;
    }
    const sectionsArray = this.sections;
    const sectionGroup = sectionsArray.at(sectionIndex);
    const questionsArray = sectionGroup.get('paperQuestions') as any;
    const item = questionsArray.at(questionIndex).value;
    const prevItem = questionsArray.at(questionIndex - 1).value;
    questionsArray.at(questionIndex).setValue(prevItem);
    questionsArray.at(questionIndex - 1).setValue(item);
  }
  down(sectionIndex: number, questionIndex: number) {
    const questionsArray = this.sections.at(sectionIndex).get('paperQuestions') as FormArray;
    if (questionIndex === questionsArray.length - 1) {
      return;
    }
    const sectionsArray = this.sections;
    const section = sectionsArray.at(sectionIndex);
    const questionsArrayForm = section.get('paperQuestions') as any;
    const item = questionsArrayForm.at(questionIndex).value;
    const nextItem = questionsArrayForm.at(questionIndex + 1).value;
    questionsArrayForm.at(questionIndex).setValue(nextItem);
    questionsArrayForm.at(questionIndex + 1).setValue(item);
  }

  onQuestionScoreChange(sectionIndex: number) {
    const sectionsArray = this.sections;
    const section = sectionsArray.at(sectionIndex);
    const questions = section.get('paperQuestions') as FormArray;
    const scoreEach = section.get('scoreEach')?.value;
    questions.controls.forEach(q => {
      q.get('score').setValue(scoreEach);
    });

    this.recomputeTotal();
  }

  // Recompute total score from sections and update the form control
  recomputeTotal() {
    if (!this.form) return;
    let totalScore = 0;

    this.sections.controls.forEach((sectionControl, index) => {
      const questionsArray = sectionControl.get('paperQuestions') as FormArray;
      const sectionTotalScore = questionsArray.controls.reduce((sum, q) => sum + (q.get('score')?.value || 0), 0);
      const sectionTotalCount = questionsArray.length;

      sectionControl.get('totalScore')?.setValue(sectionTotalScore);
      sectionControl.get('totalCount')?.setValue(sectionTotalCount);

      totalScore += sectionTotalScore;
    });

    this.form.get('score').setValue(totalScore);
  }
  // Assign order values to all sections and questions before submission
  private assignOrderValues(formValue: any) {
    formValue.sections.forEach((section, sectionIndex) => {
      section.order = sectionIndex;
      section.paperQuestions.forEach((question, questionIndex) => {
        question.order = questionIndex;
      });
    });
    return formValue;
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

    const formValue = this.assignOrderValues({ ...this.form.value });

    if (this.paperId) {
      this.paperService
        .update(this.paperId, {
          ...this.paper,
          ...formValue
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
        .create(formValue)
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
