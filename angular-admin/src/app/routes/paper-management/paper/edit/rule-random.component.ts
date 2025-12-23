import { CoreModule, LocalizationService } from '@abp/ng.core';
import { Component, inject, Input, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { OptionService, QuestionBankService } from '@proxy/admin/controllers';
import { QuestionBankCountDto, QuestionBankListDto } from '@proxy/admin/question-management/question-banks';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputNumberLegacyModule } from 'ng-zorro-antd/input-number-legacy';
import { NzModalModule, NzModalRef } from 'ng-zorro-antd/modal';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { map, tap } from 'rxjs';

export interface RuleRandomParams {
  selectedRules: Array<{ questionBankId: string; questionType: number; count: number }>;
}

@Component({
    selector: 'app-rule-random',
    templateUrl: './rule-random.component.html',
    imports: [CoreModule, NzButtonModule, NzSpinModule, NzModalModule, NzFormModule, NzSelectModule, NzInputNumberLegacyModule]
})
export class RuleRandomComponent implements OnInit {
  @Input() selectedRules: Array<{ questionBankId: string; questionType: number; count: number }> = [];

  private localizationService = inject(LocalizationService);
  private questionBankService = inject(QuestionBankService);
  private fb = inject(FormBuilder);
  private optionService = inject(OptionService);
  private modal = inject(NzModalRef);

  questionTypes: Array<{ label: string; value: number }> = [];
  questionBanks: QuestionBankListDto[] = [];
  questionBankCount: QuestionBankCountDto;
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
          this.questionBanks = res.items;
          this.form = this.fb.group({
            questionBankId: [null, [Validators.required]],
            questionType: [null, [Validators.required]],
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
    this.questionBankService
      .getQuestionCount(questionBankId)
      .pipe(
        tap(res => {
          this.questionBankCount = res;
        })
      )
      .subscribe();
  }

  getQuestionCount(value: number) {
    let count = 0;
    switch (value) {
      case 0:
        count = this.questionBankCount.singleCount;
        break;
      case 1:
        count = this.questionBankCount.judgeCount;
        break;
      case 2:
        count = this.questionBankCount.multiCount;
        break;
      case 3:
        count = this.questionBankCount.blankCount;
        break;
      default:
        count = 0;
    }

    // Subtract already selected count for the same type
    const bankId = this.form.get('questionBankId').value;
    const selectedCount = this.selectedRules
      .filter(r => r.questionBankId === bankId && r.questionType === value)
      .reduce((sum, r) => sum + r.count, 0);

    this.totalQuestionCount = Math.max(0, count - selectedCount);
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
