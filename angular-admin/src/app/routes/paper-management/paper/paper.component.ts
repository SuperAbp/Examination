import { CoreModule, LocalizationService, PermissionService } from '@abp/ng.core';
import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { STChange, STColumn, STComponent, STData, STModule, STPage } from '@delon/abc/st';
import { DelonFormModule, SFSchema, SFStringWidgetSchema } from '@delon/form';
import { NzMessageService } from 'ng-zorro-antd/message';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { PageHeaderModule } from '@delon/abc/page-header';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { PaperService } from '@proxy/admin/controllers';
import { GetPapersInput, PaperListDto } from '@proxy/admin/paper-management/papers';

@Component({
  selector: 'app-exam-management-paper',
  templateUrl: './paper.component.html',
  standalone: true,
  imports: [CoreModule, PageHeaderModule, DelonFormModule, STModule, NzCardModule, NzButtonModule]
})
export class PaperManagementPaperComponent implements OnInit {
  private router = inject(Router);
  private localizationService = inject(LocalizationService);
  private messageService = inject(NzMessageService);
  private permissionService = inject(PermissionService);
  private paperService = inject(PaperService);
  papers: PaperListDto[];
  total: number;
  loading = false;
  params: GetPapersInput;
  page: STPage = {
    show: true,
    showSize: true,
    front: false,
    pageSizes: [10, 20, 30, 40, 50]
  };
  searchSchema: SFSchema = {
    properties: {
      name: {
        type: 'string',
        title: '',
        ui: {
          width: 250,
          placeholder: this.localizationService.instant('Exam::Placeholder', this.localizationService.instant('Exam::Name'))
        } as SFStringWidgetSchema
      }
    }
  };
  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[] = [
    { title: this.localizationService.instant('Exam::Name'), index: 'name' },
    { title: this.localizationService.instant('Exam::Score'), index: 'score' },
    {
      title: this.localizationService.instant('Exam::Actions'),
      buttons: [
        {
          icon: 'edit',
          type: 'link',
          tooltip: this.localizationService.instant('Exam::Edit'),
          iif: () => {
            return this.permissionService.getGrantedPolicy('Exam.Paper.Update');
          },
          click: (record: STData, modal?: any, instance?: STComponent) => {
            this.router.navigateByUrl(`/paper-management/paper/${record['id']}/edit`);
          }
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
            return this.permissionService.getGrantedPolicy('Exam.Paper.Delete');
          },
          click: (record, _modal, component) => {
            this.paperService.delete(record.id).subscribe(response => {
              this.messageService.success(this.localizationService.instant('Exam::DeletedSuccessfully', record.name));
              // tslint:disable-next-line: no-non-null-assertion
              component!.removeRow(record);
            });
          }
        }
      ]
    }
  ];

  constructor() {}

  ngOnInit() {
    this.params = this.resetParameters();
    this.getList();
  }
  getList() {
    this.loading = true;
    this.paperService
      .getList(this.params)
      .pipe(tap(() => (this.loading = false)))
      .subscribe(response => ((this.papers = response.items), (this.total = response.totalCount)));
  }
  resetParameters(): GetPapersInput {
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
    if (e.name) {
      this.params.name = e.name;
    } else {
      delete this.params.name;
    }
    this.st.load(1);
  }
  add() {
    this.router.navigateByUrl(`/paper-management/paper/create`);
  }
}
