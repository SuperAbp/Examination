import { ConfigStateService, CoreModule, LocalizationService, PermissionService } from '@abp/ng.core';
import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { PageHeaderModule } from '@delon/abc/page-header';
import { STChange, STColumn, STComponent, STModule, STPage } from '@delon/abc/st';
import { DelonFormModule, SFSchema } from '@delon/form';
import { ModalHelper } from '@delon/theme';
import { QuestionBankService } from '@proxy/admin/controllers';
import { GetQuestionBanksInput, QuestionBankListDto } from '@proxy/admin/question-management/question-banks';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzMessageService } from 'ng-zorro-antd/message';
import { tap } from 'rxjs/operators';

import { QuestionManagementQuestionBankEditComponent } from './edit/edit.component';

@Component({
  selector: 'app-question-management-question-bank',
  templateUrl: './question-bank.component.html',
  standalone: true,
  imports: [CoreModule, PageHeaderModule, DelonFormModule, STModule, NzButtonModule, NzCardModule]
})
export class QuestionManagementQuestionBankComponent implements OnInit {
  private modal = inject(ModalHelper);
  private localizationService = inject(LocalizationService);
  private messageService = inject(NzMessageService);
  private permissionService = inject(PermissionService);
  private repositoryService = inject(QuestionBankService);

  questionBanks: QuestionBankListDto[];
  total: number;
  loading = false;
  params: GetQuestionBanksInput;
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
    { title: this.localizationService.instant('Exam::Title'), index: 'title' },
    { title: this.localizationService.instant('Exam::SingleCount'), index: 'singleCount' },
    { title: this.localizationService.instant('Exam::MultiCount'), index: 'multiCount' },
    { title: this.localizationService.instant('Exam::JudgeCount'), index: 'judgeCount' },
    { title: this.localizationService.instant('Exam::BlankCount'), index: 'blankCount' },
    {
      title: this.localizationService.instant('Exam::Actions'),
      buttons: [
        {
          icon: 'edit',
          type: 'modal',
          tooltip: this.localizationService.instant('Exam::Edit'),
          iif: () => {
            return this.permissionService.getGrantedPolicy('Exam.QuestionBank.Update');
          },
          modal: {
            component: QuestionManagementQuestionBankEditComponent,
            params: (record: any) => ({
              repositoryId: record.id
            })
          },
          click: 'reload'
        },
        {
          icon: 'delete',
          type: 'del',
          tooltip: this.localizationService.instant('Exam::Delete'),
          pop: {
            title: this.localizationService.instant('Exam::AreYouSure'),
            okType: 'danger',
            icon: 'star'
          },
          iif: () => {
            return this.permissionService.getGrantedPolicy('Exam.QuestionBank.Delete');
          },
          click: (record, _modal, component) => {
            this.repositoryService.delete(record.id).subscribe(response => {
              this.messageService.success(this.localizationService.instant('Exam::DeletedSuccessfully', record.name));
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
    this.repositoryService
      .getList(this.params)
      .pipe(tap(() => (this.loading = false)))
      .subscribe(response => ((this.questionBanks = response.items), (this.total = response.totalCount)));
  }
  resetParameters(): GetQuestionBanksInput {
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
    if (e.title) {
      this.params.title = e.title;
    } else {
      delete this.params.title;
    }
    this.st.load(1);
  }
  add() {
    this.modal.createStatic(QuestionManagementQuestionBankEditComponent, { repositoryId: '' }).subscribe(() => this.st.reload());
  }
}
