import { ConfigStateService, CoreModule, LocalizationService, PermissionService } from '@abp/ng.core';
import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { PageHeaderModule } from '@delon/abc/page-header';
import { STChange, STColumn, STComponent, STModule, STPage } from '@delon/abc/st';
import { DelonFormModule, SFSchema } from '@delon/form';
import { ModalHelper } from '@delon/theme';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzMessageService } from 'ng-zorro-antd/message';
import { tap } from 'rxjs/operators';

import { ExamManagementUserExamEditComponent } from './edit/edit.component';

@Component({
  selector: 'app-exam-management-user-exam',
  templateUrl: './user-exam.component.html',
  standalone: true,
  imports: [CoreModule, PageHeaderModule, DelonFormModule, STModule, NzCardModule, NzButtonModule]
})
export class ExamManagementUserExamComponent implements OnInit {
  private modal = inject(ModalHelper);
  private localizationService = inject(LocalizationService);
  private messageService = inject(NzMessageService);
  private permissionService = inject(PermissionService);
  private userExamService = inject(UserExamService);

  userExams: UserExamListDto[];
  total: number;
  loading = false;
  params: GetUserExamsInput;
  page: STPage = {
    show: true,
    showSize: true,
    front: false,
    pageSizes: [10, 20, 30, 40, 50]
  };
  searchSchema: SFSchema = {
    properties: {
      // TODO:搜索参数
    }
  };
  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[] = [
    { title: this.localizationService.instant('*::Name'), index: 'name' },
    {
      title: this.localizationService.instant('*::Actions'),
      buttons: [
        {
          icon: 'edit',
          type: 'modal',
          tooltip: this.localizationService.instant('*::Edit'),
          iif: () => {
            return this.permissionService.getGrantedPolicy('*.*.Update');
          },
          modal: {
            component: ExamManagementUserExamEditComponent,
            params: (record: any) => ({
              userExamId: record.id
            })
          },
          click: 'reload'
        },
        {
          icon: 'delete',
          type: 'del',
          tooltip: this.localizationService.instant('*::Delete'),
          pop: {
            title: this.localizationService.instant('*::AreYouSure'),
            okType: 'danger',
            icon: 'star'
          },
          iif: () => {
            return this.permissionService.getGrantedPolicy('*.*.Delete');
          },
          click: (record, _modal, component) => {
            this.userExamService.delete(record.id).subscribe(response => {
              this.messageService.success(this.localizationService.instant('*::DeletedSuccessfully', record.name));
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
    this.userExamService
      .getList(this.params)
      .pipe(tap(() => (this.loading = false)))
      .subscribe(response => ((this.userExams = response.items), (this.total = response.totalCount)));
  }
  resetParameters(): GetUserExamsInput {
    return {
      skipCount: 0,
      maxResultCount: 10,
      sorting: 'Id Desc'
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
    //if (e.name) {
    //  this.params.name = e.name;
    //} else {
    //  delete this.params.name;
    //}
    this.st.load(1);
  }
  add() {
    this.modal.createStatic(ExamManagementUserExamEditComponent, { userExamId: '' }).subscribe(() => this.st.reload());
  }
}
