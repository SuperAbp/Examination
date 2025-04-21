import { CoreModule, LocalizationService } from '@abp/ng.core';
import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FooterToolbarModule } from '@delon/abc/footer-toolbar';
import { PageHeaderModule } from '@delon/abc/page-header';
import { QuestionCategoryService } from '@proxy/admin/controllers';
import { GetQuestionCategoryForEditorOutput } from '@proxy/admin/question-management/question-categories';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { finalize, tap } from 'rxjs/operators';

@Component({
  selector: 'app-question-management-category-edit',
  templateUrl: './edit.component.html',
  standalone: true,
  imports: [
    CoreModule,
    PageHeaderModule,
    FooterToolbarModule,
    NzSpinModule,
    NzCardModule,
    NzGridModule,
    NzFormModule,
    NzSelectModule,
    NzInputModule,
    NzButtonModule
  ]
})
export class QuestionManagementCategoryEditComponent implements OnInit {
  @Input()
  categoryId: string;
  category: GetQuestionCategoryForEditorOutput;

  loading = false;
  isConfirmLoading = false;

  form: FormGroup = null;

  constructor(
    private fb: FormBuilder,
    private modal: NzModalRef,
    private messageService: NzMessageService,
    private localizationService: LocalizationService,
    private questionCategoryService: QuestionCategoryService
  ) {}

  ngOnInit(): void {
    this.loading = true;
    if (this.categoryId) {
      this.questionCategoryService
        .getEditor(this.categoryId)
        .pipe(
          tap(response => {
            this.category = response;
            this.buildForm();
            this.loading = false;
          })
        )
        .subscribe();
    } else {
      this.category = {} as GetQuestionCategoryForEditorOutput;
      this.buildForm();
      this.loading = false;
    }
  }

  buildForm() {
    this.form = this.fb.group({
      name: [this.category.name || '', [Validators.required]],
      parentId: [this.category.parentId || null]
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

    if (this.categoryId) {
      this.questionCategoryService
        .update(this.categoryId, {
          ...this.category,
          ...this.form.value
        })
        .pipe(
          tap(response => {
            this.modal.close(true);
          }),
          finalize(() => (this.isConfirmLoading = false))
        )
        .subscribe();
    } else {
      this.questionCategoryService
        .create({
          ...this.form.value
        })
        .pipe(
          tap(response => {
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
