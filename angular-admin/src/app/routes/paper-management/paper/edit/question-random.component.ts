import { CoreModule, LocalizationService } from '@abp/ng.core';
import { Component, inject, Input, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { STChange, STColumn, STComponent, STModule, STPage } from '@delon/abc/st';
import { DelonFormModule, SFSchema, SFSchemaEnumType, SFSelectWidgetSchema, SFStringWidgetSchema } from '@delon/form';
import { OptionService, QuestionBankService, QuestionService } from '@proxy/admin/controllers';
import { GetQuestionCountInput, QuestionBankListDto } from '@proxy/admin/question-management/question-banks';
import { GetQuestionsInput, QuestionDetailDto, QuestionListDto } from '@proxy/admin/question-management/questions';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzModalModule, NzModalRef } from 'ng-zorro-antd/modal';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { count, map, tap } from 'rxjs';

@Component({
  selector: 'app-question-random',
  templateUrl: './question-random.component.html',
  imports: [CoreModule, NzButtonModule, NzSpinModule, NzModalModule, NzFormModule, NzSelectModule, NzInputNumberModule]
})
export class QuestionRandomComponent implements OnInit {
  @Input()
  selectedQuestions: QuestionDetailDto[];

  private localizationService = inject(LocalizationService);
  private questionBankService = inject(QuestionBankService);
  private questionService = inject(QuestionService);
  private fb = inject(FormBuilder);
  private optionService = inject(OptionService);
  private modal = inject(NzModalRef);

  questions: QuestionListDto[];
  questionTypes: Array<{ label: string; value: number }> = [];
  questionBanks: QuestionBankListDto[];
  totalQuestionCount: number = 0;
  total: number;
  loading = true;
  isConfirmLoading = false;
  form: FormGroup = null;
  params: GetQuestionsInput;

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
            count: [0, [Validators.min(0), (control: AbstractControl) => Validators.max(this.totalQuestionCount)(control)]]
          });
          this.loading = false;
        })
      )
      .subscribe();
  }
  getQuestionBankWithQuestionCount(questionBankId: string) {
    const questionType = this.form.get('questionType')?.value;
    if (questionBankId && questionType !== null && questionType !== undefined) {
      this.updateAvailableQuestionCount(questionBankId, questionType);
    }
  }

  private updateAvailableQuestionCount(questionBankId: string, questionType: number) {
    const input: GetQuestionCountInput = {
      questionBankId,
      questionType
    };

    this.questionService
      .getCount(input)
      .pipe(
        tap(count => {
          this.totalQuestionCount = count;
        })
      )
      .subscribe();
  }

  getQuestionCount(value: number) {
    const questionBankId = this.form.get('questionBankId')?.value;
    if (questionBankId) {
      this.updateAvailableQuestionCount(questionBankId, value);
    }
  }
  save() {
    this.modal.close(this.form.value);
  }
}
