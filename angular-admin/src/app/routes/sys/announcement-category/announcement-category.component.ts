import { ConfigStateService, CoreModule, LocalizationService, PermissionService } from '@abp/ng.core';
import { Component, inject, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { PageHeaderModule } from '@delon/abc/page-header';
import { STChange, STColumn, STComponent, STModule, STPage } from '@delon/abc/st';
import { DelonFormModule, SFSchema } from '@delon/form';
import { ModalHelper } from '@delon/theme';
import { AnnouncementCategoryService } from '@proxy/admin/controllers';
import { AnnouncementCategoryListDto } from '@proxy/admin/announcements/models';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { tap } from 'rxjs/operators';
import { SysAnnouncementCategoryEditComponent } from './edit/edit.component';

@Component({
  selector: 'app-sys-announcement-category',
  templateUrl: './announcement-category.component.html',
  imports: [CoreModule, PageHeaderModule, DelonFormModule, STModule, NzCardModule, NzButtonModule, NzPopconfirmModule]
})
export class SysAnnouncementCategoryComponent implements OnInit {
  private modal = inject(ModalHelper);
  private localizationService = inject(LocalizationService);
  private messageService = inject(NzMessageService);
  private permissionService = inject(PermissionService);
  private categoryService = inject(AnnouncementCategoryService);
  private cdr = inject(ChangeDetectorRef);

  categories: AnnouncementCategoryListDto[];
  total: number;
  loading = false;
  page: STPage = {
    show: false
  };
  searchSchema: SFSchema = {
    properties: {
      name: {
        type: 'string',
        title: '',
        ui: {
          placeholder: this.localizationService.instant('Exam::Placeholder', this.localizationService.instant('Exam::Name'))
        }
      }
    }
  };
  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[] = [
    { title: this.localizationService.instant('Exam::Name'), index: 'name' },
    { title: this.localizationService.instant('Exam::Sort'), index: 'sort' },
    { title: this.localizationService.instant('Exam::Remark'), index: 'remark' },
    {
      title: this.localizationService.instant('Exam::Actions'),
      buttons: [
        {
          icon: 'edit',
          type: 'modal',
          iif: () => {
            return this.permissionService.getGrantedPolicy('Exam.AnnouncementCategories.Update');
          },
          modal: {
            component: SysAnnouncementCategoryEditComponent,
            params: (record: any) => ({
              categoryId: record.id
            })
          },
          click: 'reload'
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
            return this.permissionService.getGrantedPolicy('Exam.AnnouncementCategories.Delete');
          },
          click: (record: any, _modal, component) => {
            this.categoryService.delete(record.id).subscribe(() => {
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
    this.getList();
  }

  getList() {
    this.loading = true;
    this.cdr.markForCheck();
    this.categoryService
      .getList()
      .pipe(
        tap(() => {
          this.loading = false;
          this.cdr.markForCheck();
        })
      )
      .subscribe(response => {
        this.categories = response.items;
        this.total = response.items.length;
        this.cdr.detectChanges();
      });
  }

  reset() {
    this.st.load(1);
  }

  search(e) {
    if (e.name) {
      this.categories = this.categories.filter(c => c.name.includes(e.name));
    } else {
      delete e.name;
    }
    this.st.load(1);
  }

  add() {
    this.modal.createStatic(SysAnnouncementCategoryEditComponent, { categoryId: '' }).subscribe(() => this.getList());
  }
}
