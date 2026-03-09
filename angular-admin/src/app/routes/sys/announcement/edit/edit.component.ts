import { CoreModule, LocalizationService } from '@abp/ng.core';
import { Component, OnInit, Input, inject, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { I18NService } from '@core';
import { AnnouncementService, AnnouncementCategoryService } from '@proxy/admin/controllers';
import { AnnouncementDetailDto, AnnouncementCategoryListDto } from '@proxy/admin/announcements/models';
import { EditorComponent, TINYMCE_SCRIPT_SRC } from '@tinymce/tinymce-angular';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalModule, NzModalRef } from 'ng-zorro-antd/modal';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { finalize, tap } from 'rxjs/operators';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { dateTimePickerUtil } from '@delon/util';

@Component({
  selector: 'app-sys-announcement-edit',
  templateUrl: './edit.component.html',
  providers: [{ provide: TINYMCE_SCRIPT_SRC, useValue: 'tinymce/tinymce.min.js' }],
  imports: [
    CoreModule,
    NzSpinModule,
    NzModalModule,
    NzFormModule,
    NzInputModule,
    NzInputNumberModule,
    NzButtonModule,
    NzDatePickerModule,
    NzSelectModule,
    EditorComponent
  ]
})
export class SysAnnouncementEditComponent implements OnInit {
  @Input()
  announcementId: string;

  announcement: AnnouncementDetailDto;
  categories: AnnouncementCategoryListDto[] = [];
  loading = false;
  isConfirmLoading = false;

  form: FormGroup = null;
  init: EditorComponent['init'] = {
    base_url: '/tinymce',
    suffix: '.min',
    plugins: 'preview fullscreen link table lists image media code',
    toolbar:
      'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | align numlist bullist | forecolor backcolor removeformat | link image media table | fullscreen preview code',
    toolbar_mode: 'sliding',
    height: 300,
    menubar: false,
    branding: false
  };

  private fb = inject(FormBuilder);
  private modal = inject(NzModalRef);
  private messageService = inject(NzMessageService);
  private localizationService = inject(LocalizationService);
  private announcementService = inject(AnnouncementService);
  private categoryService = inject(AnnouncementCategoryService);
  private i18n = inject(I18NService);
  private cdr = inject(ChangeDetectorRef);

  constructor() {
    if (this.i18n.defaultLang === 'zh-CN') {
      this.init['language'] = 'zh_CN';
      this.init['language_url'] = '/assets/tinymce/langs/zh_CN.js';
    }
  }

  get canPublish(): boolean {
    return !this.form?.get('scheduledPublishTime')?.value;
  }

  get canSave(): boolean {
    return !this.announcement?.isPublished;
  }

  ngOnInit(): void {
    this.loading = true;
    this.cdr.markForCheck();
    this.categoryService.getList().subscribe(response => {
      this.categories = response.items;
      this.cdr.detectChanges();
    });

    if (this.announcementId) {
      this.announcementService
        .get(this.announcementId)
        .pipe(
          tap(response => {
            this.announcement = response;
            this.buildForm();
            this.loading = false;
            this.cdr.markForCheck();
          })
        )
        .subscribe(() => this.cdr.detectChanges());
    } else {
      this.announcement = {
        title: '',
        content: '',
        scheduledPublishTime: null,
        scheduledExpirationTime: null,
        sort: 0,
        categoryId: null,
        isPublished: false,
        displayOrder: 0
      } as AnnouncementDetailDto;
      this.buildForm();
      this.loading = false;
      this.cdr.detectChanges();
    }
  }

  buildForm() {
    this.form = this.fb.group({
      title: [this.announcement?.title || '', [Validators.required]],
      content: [this.announcement?.content || '', [Validators.required]],
      scheduledPublishTime: [this.announcement?.scheduledPublishTime ? new Date(this.announcement.scheduledPublishTime) : null],
      scheduledExpirationTime: [this.announcement?.scheduledExpirationTime ? new Date(this.announcement.scheduledExpirationTime) : null],
      sort: [this.announcement?.sort ?? 0, [Validators.required, Validators.min(0)]],
      categoryId: [this.announcement?.categoryId || null]
    });

    this.updateContentDisabledState();

    this.form.get('scheduledPublishTime').valueChanges.subscribe(() => {
      const scheduledExpirationTimeControl = this.form.get('scheduledExpirationTime');
      if (scheduledExpirationTimeControl) {
        scheduledExpirationTimeControl.updateValueAndValidity();
      }
    });
  }

  /**
   * 根据 canSave 状态更新 content 表单控件的启用/禁用状态
   */
  private updateContentDisabledState(): void {
    const contentControl = this.form.get('content');
    if (contentControl) {
      if (this.canSave) {
        contentControl.enable();
      } else {
        contentControl.disable();
      }
    }
  }

  save(publish: boolean = false) {
    if (!this.form.valid || this.isConfirmLoading) {
      for (const key of Object.keys(this.form.controls)) {
        this.form.controls[key].markAsDirty();
        this.form.controls[key].updateValueAndValidity();
      }
      return;
    }

    const scheduledPublishTime = this.form.get('scheduledPublishTime').value;
    const scheduledExpirationTime = this.form.get('scheduledExpirationTime').value;
    if (scheduledPublishTime) {
      publish = false;
    }

    if (scheduledPublishTime && scheduledExpirationTime) {
      const publishDate = new Date(scheduledPublishTime);
      const expireDate = new Date(scheduledExpirationTime);
      if (expireDate.getTime() <= publishDate.getTime()) {
        this.messageService.warning(this.localizationService.instant('Exam::ScheduledExpirationTimeMustBeAfterScheduledPublishTime'));
        return;
      }
    }

    this.isConfirmLoading = true;

    const formValue = this.form.value;
    const data = {
      ...formValue,
      publish: publish,
      scheduledExpirationTime: formValue.scheduledExpirationTime
        ? dateTimePickerUtil.format(formValue.scheduledExpirationTime, 'yyyy-MM-dd HH:mm') + ':00'
        : null,
      scheduledPublishTime: formValue.scheduledPublishTime
        ? dateTimePickerUtil.format(formValue.scheduledPublishTime, 'yyyy-MM-dd HH:mm') + ':00'
        : null
    };

    if (this.announcementId) {
      this.announcementService
        .update(this.announcementId, data)
        .pipe(
          tap(() => {
            this.messageService.success(this.localizationService.instant('Exam::SaveSuccessfully'));
            this.modal.close(true);
          })
        )
        .subscribe();
    } else {
      this.announcementService
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

  /**
   * 禁用今天的日期之前的日期（发布时间只能选今天或未来）
   */
  disabledDateBeforeToday = (current: Date): boolean => {
    if (!current) {
      return false;
    }
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const currentDate = new Date(current);
    currentDate.setHours(0, 0, 0, 0);
    return currentDate.getTime() < today.getTime();
  };

  /**
   * 禁用早于发布时间的日期（过期时间不能早于发布时间）
   */
  disabledDateBeforePublishTime = (current: Date): boolean => {
    if (!current) {
      return false;
    }
    const publishTime = this.form?.get('publishTime')?.value;
    if (!publishTime) {
      return this.disabledDateBeforeToday(current);
    }
    const publishDate = new Date(publishTime);
    publishDate.setHours(0, 0, 0, 0);
    const currentDate = new Date(current);
    currentDate.setHours(0, 0, 0, 0);
    return currentDate.getTime() < publishDate.getTime();
  };
}
