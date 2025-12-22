import { CoreModule, LocalizationService } from '@abp/ng.core';
import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { QuestionBankService } from '@proxy/admin/controllers';
import { GetQuestionBankForEditorOutput } from '@proxy/admin/question-management/question-banks';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalModule, NzModalRef } from 'ng-zorro-antd/modal';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { finalize, tap } from 'rxjs/operators';

@Component({
  selector: 'app-question-management-question-bank-edit',
  templateUrl: './edit.component.html',
  standalone: true,
  imports: [CoreModule, NzSpinModule, NzModalModule, NzFormModule, NzInputModule, NzButtonModule]
})
export class QuestionManagementQuestionBankEditComponent implements OnInit {
  @Input()
  repositoryId: string;
  questionBank: GetQuestionBankForEditorOutput;

  loading = false;
  isConfirmLoading = false;

  form: FormGroup = null;

  constructor(
    private fb: FormBuilder,
    private modal: NzModalRef,
    private messageService: NzMessageService,
    private localizationService: LocalizationService,
    private questionBankService: QuestionBankService
  ) {}

  ngOnInit(): void {
    this.loading = true;
    if (this.repositoryId) {
      this.questionBankService
        .getEditor(this.repositoryId)
        .pipe(
          tap(response => {
            this.questionBank = response;
            this.buildForm();
            this.loading = false;
          })
        )
        .subscribe();
    } else {
      this.questionBank = {} as GetQuestionBankForEditorOutput;
      this.buildForm();
      this.loading = false;
    }
  }

  buildForm() {
    this.form = this.fb.group({
      title: [this.questionBank.title || '', [Validators.required]],
      remark: [this.questionBank.remark || '']
    });
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

    if (this.repositoryId) {
      this.questionBankService
        .update(this.repositoryId, {
          ...this.questionBank,
          ...this.form.value
        })
        .pipe(
          tap(response => {
            this.messageService.success(this.localizationService.instant('Exam::SaveSuccessfully'));
            this.modal.close(true);
          }),
          finalize(() => (this.isConfirmLoading = false))
        )
        .subscribe();
    } else {
      this.questionBankService
        .create({
          ...this.form.value
        })
        .pipe(
          tap(response => {
            this.messageService.success(this.localizationService.instant('Exam::SaveSuccessfully'));
            this.modal.close(true);
          }),
          finalize(() => (this.isConfirmLoading = false))
        )
        .subscribe();
    }
  }

  close() {
    this.modal.destroy();
  }
}
