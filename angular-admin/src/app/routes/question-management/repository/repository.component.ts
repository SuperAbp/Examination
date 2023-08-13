import { ConfigStateService, LocalizationService, PermissionService } from '@abp/ng.core';
import { Component, OnInit, ViewChild } from '@angular/core';
import { STChange, STColumn, STComponent, STPage } from '@delon/abc/st';
import { SFSchema } from '@delon/form';
import { ModalHelper } from '@delon/theme';
import { QuestionManagementRepositoryEditComponent } from './edit/edit.component';
import { NzMessageService } from 'ng-zorro-antd/message';
import { tap } from 'rxjs/operators';
import { GetQuestionReposInput, QuestionRepoListDto } from '@proxy/super-abp/exam/admin/question-management/question-repos';
import { QuestionRepoService } from '@proxy/super-abp/exam/admin/controllers';

@Component({
  selector: 'app-question-management-repository',
  templateUrl: './repository.component.html'
})
export class QuestionManagementRepositoryComponent implements OnInit {
  repositorys: QuestionRepoListDto[];
  total: number;
  loading = false;
  params: GetQuestionReposInput;
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
            return this.permissionService.getGrantedPolicy('Exam.QuestionRepository.Update');
          },
          modal: {
            component: QuestionManagementRepositoryEditComponent,
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
            return this.permissionService.getGrantedPolicy('Exam.QuestionRepository.Delete');
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

  constructor(
    private modal: ModalHelper,
    private localizationService: LocalizationService,
    private messageService: NzMessageService,
    private permissionService: PermissionService,
    private repositoryService: QuestionRepoService
  ) {}

  ngOnInit() {
    this.params = this.resetParameters();
    this.getList();
  }
  getList() {
    this.loading = true;
    this.repositoryService
      .getList(this.params)
      .pipe(tap(() => (this.loading = false)))
      .subscribe(response => ((this.repositorys = response.items), (this.total = response.totalCount)));
  }
  resetParameters(): GetQuestionReposInput {
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
    this.modal.createStatic(QuestionManagementRepositoryEditComponent, { repositoryId: '' }).subscribe(() => this.st.reload());
  }
}
