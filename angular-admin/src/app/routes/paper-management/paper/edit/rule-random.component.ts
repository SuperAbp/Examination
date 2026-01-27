import { CoreModule, LocalizationService } from '@abp/ng.core';
import { Component, inject, Input, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { KnowledgePointService, OptionService, QuestionBankService, QuestionService } from '@proxy/admin/controllers';
import { GetQuestionCountInput, QuestionBankListDto } from '@proxy/admin/question-management/question-banks';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzModalModule, NzModalRef } from 'ng-zorro-antd/modal';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { map, tap } from 'rxjs';

export interface RuleRandomParams {
  selectedRules: Array<{ questionBankId: string; questionType: number; count: number; knowledgePointId?: string }>;
}

@Component({
  selector: 'app-rule-random',
  templateUrl: './rule-random.component.html',
  imports: [CoreModule, NzButtonModule, NzSpinModule, NzModalModule, NzFormModule, NzSelectModule, NzInputNumberModule]
})
export class RuleRandomComponent implements OnInit {
  @Input() selectedRules: Array<{ questionBankId: string; questionType: number; count: number; knowledgePointId?: string }> = [];

  private localizationService = inject(LocalizationService);
  private questionBankService = inject(QuestionBankService);
  private questionService = inject(QuestionService);
  private knowledgePointService = inject(KnowledgePointService);
  private fb = inject(FormBuilder);
  private optionService = inject(OptionService);
  private modal = inject(NzModalRef);

  questionTypes: Array<{ label: string; value: number }> = [];
  questionBanks: QuestionBankListDto[] = [];
  knowledgePoints: any[] = [];
  totalQuestionCount: number = 0;
  loading = true;
  isConfirmLoading = false;
  form: FormGroup = null;

  get questionBankId() {
    return this.form.get('questionBankId');
  }

  get count() {
    return this.form.get('count');
  }

  ngOnInit() {
    this.buildForm();
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

          // Load Knowledge Points
          this.knowledgePointService.getAll({}).subscribe(res => {
            this.knowledgePoints = res.items;
          });

          this.questionBanks = res.items;
          this.form = this.fb.group({
            questionBankId: [null, [Validators.required]],
            questionType: [null, [Validators.required]],
            knowledgePointId: [null],
            count: [
              0,
              [Validators.required, Validators.min(1), (control: AbstractControl) => Validators.max(this.totalQuestionCount)(control)]
            ]
          });
          this.loading = false;
        })
      )
      .subscribe();
  }

  getQuestionBankWithQuestionCount(questionBankId: string) {
    const questionType = this.form.get('questionType')?.value;
    const knowledgePointId = this.form.get('knowledgePointId')?.value;
    
    // Only call if both question bank and question type are selected
    if (questionBankId && questionType !== null && questionType !== undefined) {
      this.updateAvailableQuestionCount(questionBankId, questionType, knowledgePointId);
    }
  }

  onKnowledgePointChanged(knowledgePointId: string | null) {
    // When knowledge point changes, refresh the question count
    const questionBankId = this.form.get('questionBankId')?.value;
    const questionType = this.form.get('questionType')?.value;
    
    if (questionBankId && questionType !== null && questionType !== undefined) {
      this.updateAvailableQuestionCount(questionBankId, questionType, knowledgePointId);
    }
  }

  private updateAvailableQuestionCount(questionBankId: string, questionType: number, knowledgePointId?: string) {
    const input: GetQuestionCountInput = {
      questionBankId,
      questionType,
      knowledgePointId
    };

    this.questionService
      .getCount(input)
      .pipe(
        tap(count => {
          const selectedCount = this.getSelectedQuestionCount(questionBankId, questionType, knowledgePointId);
          this.totalQuestionCount = Math.max(0, count - selectedCount);
          this.updateCountValidators();
        })
      )
      .subscribe();
  }

  private getSelectedQuestionCount(questionBankId: string, questionType: number, knowledgePointId?: string): number {
    return this.selectedRules
      .filter(r => 
        r.questionBankId === questionBankId && 
        r.questionType === questionType &&
        (!knowledgePointId || r.knowledgePointId === knowledgePointId)
      )
      .reduce((sum, r) => sum + r.count, 0);
  }

  private updateCountValidators(): void {
    const countControl = this.form.get('count');
    countControl.setValidators([Validators.required, Validators.min(1), Validators.max(this.totalQuestionCount)]);
    countControl.updateValueAndValidity();
  }

  save() {
    if (!this.form.valid) {
      for (const key of Object.keys(this.form.controls)) {
        this.form.controls[key].markAsDirty();
        this.form.controls[key].updateValueAndValidity();
      }
      return;
    }
    this.modal.close(this.form.value);
  }
  cancel() {
    this.modal.close();
  }
}
