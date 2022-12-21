import { LocalizationService } from '@abp/ng.core';
import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd/message';
import { finalize, tap } from 'rxjs/operators';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { QuestionRepoService } from '@proxy/super-abp/exam/admin/controllers';
import { GetQuestionRepoForEditorOutput } from '@proxy/super-abp/exam/admin/question-management/question-repos';

@Component({
  selector: 'app-question-management-repository-edit',
  templateUrl: './edit.component.html'
})
export class QuestionManagementRepositoryEditComponent implements OnInit {
  @Input()
  repositoryId: string;
  repository: GetQuestionRepoForEditorOutput;

  loading = false;
  isConfirmLoading = false;

  form: FormGroup = null;

  constructor(
    private fb: FormBuilder,
    private modal: NzModalRef,
    private messageService: NzMessageService,
    private localizationService: LocalizationService,
    private repositoryService: QuestionRepoService
  ) {}

  ngOnInit(): void {
    this.loading = true;
    if (this.repositoryId) {
      this.repositoryService
        .getEditor(this.repositoryId)
        .pipe(
          tap(response => {
            this.repository = response;
            this.buildForm();
            this.loading = false;
          })
        )
        .subscribe();
    } else {
      this.repository = {} as GetQuestionRepoForEditorOutput;
      this.buildForm();
      this.loading = false;
    }
  }

  buildForm() {
    this.form = this.fb.group({
      title: [this.repository.title || '', [Validators.required]],
      remark: [this.repository.remark || '']
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
      this.repositoryService
        .update(this.repositoryId, {
          ...this.repository,
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
      this.repositoryService
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
