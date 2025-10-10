import { CoreModule, LocalizationService } from '@abp/ng.core';
import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { STChange, STColumn, STComponent, STModule, STPage } from '@delon/abc/st';
import { DelonFormModule, SFSchema, SFSchemaEnumType, SFSelectWidgetSchema, SFStringWidgetSchema } from '@delon/form';
import { OptionService, QuestionBankService, QuestionService } from '@proxy/admin/controllers';
import { QuestionBankCountDto, QuestionBankListDto } from '@proxy/admin/question-management/question-banks';
import { GetQuestionsInput, QuestionListDto } from '@proxy/admin/question-management/questions';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { count, map, tap } from 'rxjs';

@Component({
  selector: 'app-question-random',
  templateUrl: './question-random.component.html',
  standalone: true,
  imports: [CoreModule, NzButtonModule, NzSpinModule, NzModalModule, NzFormModule, NzSelectModule, NzInputNumberModule]
})
export class QuestionRandomComponent implements OnInit {
  private localizationService = inject(LocalizationService);
  private questionBankService = inject(QuestionBankService);
  private questionService = inject(QuestionService);
  private fb = inject(FormBuilder);
  private optionService = inject(OptionService);

  questions: QuestionListDto[];
  questionTypes: Array<{ label: string; value: number }> = [];
  questionBanks: QuestionBankListDto[];
  questionBankCount: QuestionBankCountDto;
  selectedQuestions = [];
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
  getQuestionBankCount(value: string) {
    this.questionBankService
      .getQuestionCount(value)
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
    this.totalQuestionCount = count;
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
  }
}
