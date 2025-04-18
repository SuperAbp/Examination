import { CoreModule, LocalizationService, PermissionService } from '@abp/ng.core';
import { Component, OnInit, ViewChild } from '@angular/core';
import { PageHeaderModule } from '@delon/abc/page-header';
import { STChange, STColumn, STComponent, STModule, STPage } from '@delon/abc/st';
import { DelonFormModule, SFSchema, SFStringWidgetSchema } from '@delon/form';
import { ModalHelper } from '@delon/theme';
import { QuestionCategoryService } from '@proxy/admin/controllers';
import { GetQuestionCategoriesInput, QuestionCategoryListDto } from '@proxy/admin/question-management/question-categories';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzMessageService } from 'ng-zorro-antd/message';
import { tap } from 'rxjs/operators';

import { QuestionManagementCategoryEditComponent } from './edit/edit.component';

@Component({
  selector: 'app-question-management-category',
  templateUrl: './category.component.html',
  standalone: true,
  imports: [CoreModule, PageHeaderModule, DelonFormModule, STModule, NzCardModule, NzButtonModule]
})
export class QuestionManagementCategoryComponent implements OnInit {
  categorys: QuestionCategoryListDto[];
  total: number;
  loading = false;
  params: GetQuestionCategoriesInput;
  page: STPage = {
    show: true,
    showSize: true,
    front: false,
    pageSizes: [10, 20, 30, 40, 50]
  };
  searchSchema: SFSchema = {
    properties: {
      content: {
        type: 'string',
        title: '',
        ui: {
          placeholder: this.localizationService.instant('Exam::Placeholder', this.localizationService.instant('Exam::Title'))
        } as SFStringWidgetSchema
      }
    }
  };
  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[] = [
    { title: this.localizationService.instant('Exam::Name'), index: 'name' },
    {
      title: this.localizationService.instant('Exam::Actions'),
      buttons: [
        {
          icon: 'edit',
          type: 'modal',
          tooltip: this.localizationService.instant('Exam::Edit'),
          iif: () => {
            return this.permissionService.getGrantedPolicy('Exam.QuestionCategory.Update');
          },
          modal: {
            component: QuestionManagementCategoryEditComponent,
            params: (record: any) => ({
              categoryId: record.id
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
            return this.permissionService.getGrantedPolicy('Exam.QuestionCategory.Delete');
          },
          click: (record, _modal, component) => {
            this.questionCategoryService.delete(record.id).subscribe(response => {
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
    private questionCategoryService: QuestionCategoryService
  ) {}

  ngOnInit() {
    this.params = this.resetParameters();
    this.getList();
  }
  getList() {
    this.loading = true;
    this.questionCategoryService
      .getList(this.params)
      .pipe(tap(() => (this.loading = false)))
      .subscribe(response => ((this.categorys = response.items), (this.total = response.totalCount)));
  }
  resetParameters(): GetQuestionCategoriesInput {
    return {
      skipCount: 0,
      maxResultCount: 10
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
    this.modal.createStatic(QuestionManagementCategoryEditComponent, { categoryId: '' }).subscribe(() => this.st.reload());
  }
}
