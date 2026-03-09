import { CoreModule } from '@abp/ng.core';
import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FooterToolbarModule } from '@delon/abc/footer-toolbar';
import { PageHeaderModule } from '@delon/abc/page-header';
import { ModalHelper } from '@delon/theme';
import { dateTimePickerUtil, log } from '@delon/util';
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
import { NzAnchorModule } from 'ng-zorro-antd/anchor';
import { QuestionRandomComponent } from './question-random.component';
import { QuestionSelectComponent } from './question-select.component';
import { NzGridModule } from 'ng-zorro-antd/grid';

@Component({
  selector: 'app-paper-fix',
  templateUrl: './paper-fix.component.html',
  styleUrls: ['./paper-fix.component.less'],
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
    NzAnchorModule,
    NzGridModule,
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
  private cdr = inject(ChangeDetectorRef);
  paperId: string;
  paper: GetPaperForEditorOutput;

  loading = false;
  isConfirmLoading = false;
  showPaperTime: boolean;
  form: FormGroup = null;
  questionIds: string[] = [];
  questionDetails = new Map<string, QuestionDetailDto>();

  get totalScore() {
    return this.sections.controls.reduce((total, section) => {
      return total + section.get('paperQuestions').value.length * section.get('scoreEach').value;
    }, 0);
  }
  get sections() {
    return this.form?.get('sections') as FormArray;
  }
  getQuestionsArray1(section: any): FormArray {
    return section.get('paperQuestions') as FormArray;
  }
  getQuestionsArray(sectionIndex: number): FormArray {
    return this.sections.at(sectionIndex).get('paperQuestions') as FormArray;
  }

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
              this.cdr.detectChanges();
            })
          )
          .subscribe();
      } else {
        this.paper = {} as GetPaperForEditorOutput;
        this.buildForm();
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  buildForm() {
    this.form = this.fb.group({
      name: [this.paper.name || '', [Validators.required]],
      description: [this.paper.description || ''],
      paperType: [0],
      sections: this.fb.array((this.paper.sections || []).map(s => this.createSection(s)))
    });
  }
  createSection(section?: PaperCreateOrUpdateDtoBase_PaperSectionDto) {
    return this.fb.group({
      id: [section?.id || ''],
      title: [section?.title || '', [Validators.required]],
      scoreEach: [section?.scoreEach, [Validators.required]],
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
          this.cdr.detectChanges();
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
            this.cdr.detectChanges();
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
            this.cdr.detectChanges();
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

  back(e: MouseEvent) {
    e.preventDefault();
    this.goback();
  }
  goback() {
    this.router.navigateByUrl('/paper-management/paper');
  }
}
