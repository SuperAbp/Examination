import { CoreModule, LocalizationService } from '@abp/ng.core';
import { Component, OnInit, Input, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AnnouncementCategoryService } from '@proxy/admin/controllers';
import { AnnouncementCategoryDetailDto } from '@proxy/admin/announcements/models';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalModule, NzModalRef } from 'ng-zorro-antd/modal';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { finalize, tap } from 'rxjs/operators';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';

@Component({
  selector: 'app-sys-announcement-category-edit',
  templateUrl: './edit.component.html',
  imports: [CoreModule, NzSpinModule, NzModalModule, NzFormModule, NzInputModule, NzInputNumberModule, NzButtonModule]
})
export class SysAnnouncementCategoryEditComponent implements OnInit {
  @Input()
  categoryId: string;

  category: AnnouncementCategoryDetailDto;
  loading = false;
  isConfirmLoading = false;

  form: FormGroup = null;

  constructor(
    private fb: FormBuilder,
    private modal: NzModalRef,
    private messageService: NzMessageService,
    private localizationService: LocalizationService,
    private categoryService: AnnouncementCategoryService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loading = true;
    this.cdr.markForCheck();

    if (this.categoryId) {
      this.categoryService
        .get(this.categoryId)
        .pipe(
          tap(response => {
            this.category = response;
            this.buildForm();
            this.loading = false;
            this.cdr.markForCheck();
          })
        )
        .subscribe(() => this.cdr.detectChanges());
    } else {
      this.category = {
        name: '',
        sort: 0,
        remark: ''
      } as AnnouncementCategoryDetailDto;
      this.buildForm();
      this.loading = false;
      this.cdr.detectChanges();
    }
  }

  buildForm() {
    this.form = this.fb.group({
      name: [this.category?.name || '', [Validators.required]],
      sort: [this.category?.sort ?? 0, [Validators.required, Validators.min(0)]],
      remark: [this.category?.remark || null]
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

    const data = this.form.value;

    if (this.categoryId) {
      this.categoryService
        .update(this.categoryId, data)
        .pipe(
          tap(() => {
            this.messageService.success(this.localizationService.instant('Exam::SaveSuccessfully'));
            this.modal.close(true);
          })
        )
        .subscribe();
    } else {
      this.categoryService
        .create(data)
        .pipe(
          tap(() => {
            this.messageService.success(this.localizationService.instant('Exam::SaveSuccessfully'));
            this.modal.close(true);
          })
        )
        .subscribe();
    }
  }

  close() {
    this.modal.destroy();
  }
}
