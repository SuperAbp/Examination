import { LocalizationService } from '@abp/ng.core';
import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd/message';
import { finalize, tap } from 'rxjs/operators';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { QuestionService } from '@proxy/super-abp/exam/admin/controllers';
import { GetQuestionForEditorOutput } from '@proxy/super-abp/exam/admin/question-management/questions';

@Component({
  selector: 'app-question-management-question-edit',
  templateUrl: './edit.component.html'
})
export class QuestionManagementQuestionEditComponent implements OnInit {
  @Input()
  questionId: string;
  question: GetQuestionForEditorOutput;

  loading = false;
  isConfirmLoading = false;

  form: FormGroup = null;

  constructor(
    private fb: FormBuilder,
    private modal: NzModalRef,
    private messageService: NzMessageService,
    private localizationService: LocalizationService,
    private questionService: QuestionService
  ) {}

  ngOnInit(): void {
    this.loading = true;
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
      this.question = {} as GetQuestionForEditorOutput;
      this.buildForm();
      this.loading = false;
    }
  }

  buildForm() {
    // TODO:完善列定义
    this.form = this.fb.group({
      content: [this.question.content || '', [Validators.required]]
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

    if (this.questionId) {
      this.questionService
        .update(this.questionId, {
          ...this.question,
          ...this.form.value
        })
        .pipe(
          tap(response => {
            this.messageService.success(this.localizationService.instant('*::SaveSucceed'));
            this.modal.close(true);
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
          tap(response => {
            this.messageService.success(this.localizationService.instant('*::SaveSucceed'));
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
