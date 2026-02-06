import { ConfigStateService, CoreModule, LocalizationService, PermissionService } from '@abp/ng.core';
import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { PageHeaderModule } from '@delon/abc/page-header';
import { STChange, STColumn, STComponent, STModule, STPage } from '@delon/abc/st';
import { DelonFormModule, SFSchema } from '@delon/form';
import { ModalHelper } from '@delon/theme';
import { AnnouncementService } from '@proxy/admin/controllers';
import { GetAnnouncementsInput } from '@proxy/admin/announcements/models';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { tap } from 'rxjs/operators';

import { SysAnnouncementEditComponent } from './edit/edit.component';

@Component({
    selector: 'app-sys-announcement',
    templateUrl: './announcement.component.html',
    imports: [
        CoreModule,
        PageHeaderModule,
        DelonFormModule,
        STModule,
        NzCardModule,
        NzButtonModule,
        NzPopconfirmModule
    ]
})
export class SysAnnouncementComponent implements OnInit {
    private modal = inject(ModalHelper);
    private localizationService = inject(LocalizationService);
    private messageService = inject(NzMessageService);
    private permissionService = inject(PermissionService);
    private announcementService = inject(AnnouncementService);

    announcements: any[];
    total: number;
    loading = false;
    params: GetAnnouncementsInput;
    page: STPage = {
        show: true,
        showSize: true,
        front: false,
        pageSizes: [10, 20, 30, 40, 50]
    };
    searchSchema: SFSchema = {
        properties: {
            title: {
                type: 'string',
                title: '',
                ui: {
                    placeholder: this.localizationService.instant('Exam::Placeholder', this.localizationService.instant('Exam::Title'))
                }
            }
        }
    };
    @ViewChild('st', { static: false }) st: STComponent;
    columns: STColumn[] = [
        { title: this.localizationService.instant('Exam::CategoryName'), index: 'categoryName' },
        { title: this.localizationService.instant('Exam::Title'), index: 'title' },
        { title: this.localizationService.instant('Exam::Content'), index: 'content' },
        {
            title: this.localizationService.instant('Exam::IsPublished'),
            index: 'isPublished',
            type: 'yn'
        },
        { title: this.localizationService.instant('Exam::PublishTime'), index: 'publishTime', type: 'date' },
        { title: this.localizationService.instant('Exam::ExpirationTime'), index: 'expirationTime', type: 'date' },
        { title: this.localizationService.instant('Exam::Sort'), index: 'sort' },
        
        {
            title: this.localizationService.instant('Exam::Actions'),
            buttons: [
                {
                    icon: 'edit',
                    type: 'modal',
                    iif: (record: any) => {
                        return !record.isPublished && this.permissionService.getGrantedPolicy('Exam.Announcements.Update');
                    },
                    modal: {
                        component: SysAnnouncementEditComponent,
                        params: (record: any) => ({
                            announcementId: record.id
                        })
                    },
                    click: 'reload'
                },
                {
                    icon: 'check',
                    text: this.localizationService.instant('Exam::Publish'),
                    iif: (record: any) => {
                        return !record.isPublished && this.permissionService.getGrantedPolicy('Exam.Announcements.Publish');
                    },
                    pop: {
                        title: this.localizationService.instant('Exam::AreYouSure'),
                        okType: 'primary'
                    },
                    click: (record: any) => {
                        this.announcementService.publish(record.id).subscribe(() => {
                            this.messageService.success(this.localizationService.instant('Exam::PublishedSuccessfully'));
                            this.st.reload();
                        });
                    }
                },
                {
                    icon: 'close',
                    text: this.localizationService.instant('Exam::Unpublish'),
                    iif: (record: any) => {
                        return record.isPublished && this.permissionService.getGrantedPolicy('Exam.Announcements.Unpublish');
                    },
                    pop: {
                        title: this.localizationService.instant('Exam::AreYouSure'),
                        okType: 'primary'
                    },
                    click: (record: any) => {
                        this.announcementService.unpublish(record.id).subscribe(() => {
                            this.messageService.success(this.localizationService.instant('Exam::UnpublishedSuccessfully'));
                            this.st.reload();
                        });
                    }
                },
                {
                    icon: 'delete',
                    type: 'del',
                    pop: {
                        title: this.localizationService.instant('Exam::AreYouSure'),
                        okType: 'danger',
                        icon: 'star'
                    },
                    iif: () => {
                        return this.permissionService.getGrantedPolicy('Exam.Announcements.Delete');
                    },
                    click: (record: any, _modal, component) => {
                        this.announcementService.delete(record.id).subscribe(() => {
                            this.messageService.success(this.localizationService.instant('Exam::DeletedSuccessfully'));
                            // tslint:disable-next-line: no-non-null-assertion
                            component!.removeRow(record);
                        });
                    }
                }
            ]
        }
    ];

    ngOnInit() {
        this.params = this.resetParameters();
        this.getList();
    }

    getList() {
        this.loading = true;
        this.announcementService
            .getList(this.params)
            .pipe(tap(() => (this.loading = false)))
            .subscribe(response => ((this.announcements = response.items), (this.total = response.totalCount)));
    }

    resetParameters(): GetAnnouncementsInput {
        return {
            skipCount: 0,
            maxResultCount: 10,
            sorting: 'Sort Asc'
        };
    }

    change(e: STChange) {
        if (e.type === 'pi' || e.type === 'ps') {
            this.params.skipCount = (e.pi - 1) * e.ps;
            this.params.maxResultCount = e.ps;
            this.getList();
        } else if (e.type === 'sort') {
            this.params.sorting = `${e.sort?.column?.index as string} ${e.sort.value === 'ascend' ? 'asc' : 'desc'}`;
            this.getList();
        }
    }

    reset() {
        this.params = this.resetParameters();
        this.st.load(1);
    }

    search(e) {
        if (e.title) {
            this.params.title = e.title;
        } else {
            delete this.params.title;
        }
        this.st.load(1);
    }

    add() {
        this.modal.createStatic(SysAnnouncementEditComponent, { announcementId: '' }).subscribe(() => this.st.reload());
    }
}
