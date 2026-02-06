import { CoreModule, LocalizationService } from '@abp/ng.core';
import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AnnouncementService, AnnouncementCategoryService } from '@proxy/admin/controllers';
import { AnnouncementDetailDto, AnnouncementCategoryListDto } from '@proxy/admin/announcements/models';
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
    imports: [
        CoreModule,
        NzSpinModule,
        NzModalModule,
        NzFormModule,
        NzInputModule,
        NzInputNumberModule,
        NzButtonModule,
        NzDatePickerModule,
        NzSelectModule
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

    constructor(
        private fb: FormBuilder,
        private modal: NzModalRef,
        private messageService: NzMessageService,
        private localizationService: LocalizationService,
        private announcementService: AnnouncementService,
        private categoryService: AnnouncementCategoryService
    ) {}

    ngOnInit(): void {
        this.loading = true;
        this.categoryService.getList().subscribe(response => {
            this.categories = response.items;
        });

        if (this.announcementId) {
            this.announcementService
                .get(this.announcementId)
                .pipe(
                    tap(response => {
                        this.announcement = response;
                        this.buildForm();
                        this.loading = false;
                    })
                )
                .subscribe();
        } else {
            this.announcement = {
                title: '',
                content: '',
                publishTime: null,
                expirationTime: null,
                sort: 0,
                categoryId: null,
                isPublished: false,
                displayOrder: 0
            } as AnnouncementDetailDto;
            this.buildForm();
            this.loading = false;
        }
    }

    buildForm() {
        this.form = this.fb.group({
            title: [this.announcement?.title || '', [Validators.required]],
            content: [this.announcement?.content || '', [Validators.required]],
            publishTime: [this.announcement?.publishTime ? new Date(this.announcement.publishTime) : null],
            expirationTime: [this.announcement?.expirationTime ? new Date(this.announcement.expirationTime) : null],
            sort: [this.announcement?.sort ?? 0, [Validators.required, Validators.min(0)]],
            categoryId: [this.announcement?.categoryId || null]
        });

        this.form.get('publishTime').valueChanges.subscribe(() => {
            const expirationTimeControl = this.form.get('expirationTime');
            if (expirationTimeControl) {
                expirationTimeControl.updateValueAndValidity();
            }
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

        const publishTime = this.form.get('publishTime').value;
        const expirationTime = this.form.get('expirationTime').value;

        if (publishTime && expirationTime) {
            const publishDate = new Date(publishTime);
            const expireDate = new Date(expirationTime);
            if (expireDate.getTime() <= publishDate.getTime()) {
                this.messageService.warning(this.localizationService.instant('Exam::ExpirationTimeMustBeAfterPublishTime'));
                return;
            }
        }

        this.isConfirmLoading = true;

        const formValue = this.form.value;
        const data = {
            ...formValue,
            expirationTime: formValue.expirationTime ? dateTimePickerUtil.format(formValue.expirationTime, 'yyyy-MM-dd HH:mm') + ':00' : null,
            publishTime: formValue.publishTime ? dateTimePickerUtil.format(formValue.publishTime, 'yyyy-MM-dd HH:mm') + ':00' : null
        };

        if (this.announcementId) {
            this.announcementService
                .update(this.announcementId, data)
                .pipe(
                    tap(() => {
                        this.messageService.success(this.localizationService.instant('Exam::SaveSuccessfully'));
                        this.modal.close(true);
                    }),
                    finalize(() => (this.isConfirmLoading = false))
                )
                .subscribe();
        } else {
            this.announcementService
                .create(data)
                .pipe(
                    tap(() => {
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
