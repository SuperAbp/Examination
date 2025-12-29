import { CoreModule, LocalizationService, PermissionService } from '@abp/ng.core';
import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { Router } from '@angular/router';
import { PageHeaderModule } from '@delon/abc/page-header';
import { STChange, STColumn, STComponent, STModule, STPage } from '@delon/abc/st';
import { DelonFormModule, SFSchema, SFSchemaEnumType, SFSelectWidgetSchema, SFStringWidgetSchema } from '@delon/form';
import { ModalHelper } from '@delon/theme';
import { ExaminationService, OptionService } from '@proxy/admin/controllers';
import { ExamListDto, GetExamsInput } from '@proxy/admin/exam-management/exams';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzMessageService } from 'ng-zorro-antd/message';
import { map, tap } from 'rxjs/operators';
import { ExaminationStatus } from '@shared';

import { ExamManagementExamEditComponent } from './edit/edit.component';

@Component({
  selector: 'app-exam-management-exam',
  templateUrl: './exam.component.html',
  imports: [CoreModule, PageHeaderModule, DelonFormModule, STModule, NzCardModule, NzButtonModule]
})
export class ExamManagementExamComponent implements OnInit {
  private modal = inject(ModalHelper);
  private router = inject(Router);
  private localizationService = inject(LocalizationService);
  private messageService = inject(NzMessageService);
  private permissionService = inject(PermissionService);
  private examService = inject(ExaminationService);
  private optionService = inject(OptionService);
  exams: ExamListDto[];
  total: number;
  loading = false;
  params: GetExamsInput;
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
      },
      status: {
        type: 'string',
        title: '',
        ui: {
          widget: 'select',
          width: 150,
          placeholder: this.localizationService.instant('Exam::ChoosePlaceholder', this.localizationService.instant('Exam::Status')),
          allowClear: true,
          asyncData: () =>
            this.optionService.getExaminationStatus().pipe(
              map(res => {
                const temp: SFSchemaEnumType[] = [];
                Object.keys(res).map(item => {
                  temp.push({ label: this.localizationService.instant('::ExaminationStatus:' + Number(item)), value: Number(item) });
                });
                return temp;
              })
            )
        } as SFSelectWidgetSchema
      }
    }
  };
  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[] = [
    { title: this.localizationService.instant('Exam::Name'), index: 'name' },
    { title: this.localizationService.instant('Exam::Score'), index: 'score', render: 'score' },
    {
      title: {
        text: this.localizationService.instant('Exam::TotalTime'),
        optional: `（${this.localizationService.instant('Exam::Unit')}：${this.localizationService.instant('Exam::Minute')}）`
      },
      index: 'totalTime'
    },
    { title: this.localizationService.instant('Exam::Status'), render: 'status' },
    { title: this.localizationService.instant('Exam::AnswerMode'), render: 'answerMode' },
    { title: this.localizationService.instant('Exam::ReviewMode'), render: 'reviewMode' },
    { title: this.localizationService.instant('Exam::RandomOrderOfOption'), type: 'yn', index: 'randomOrderOfOption' },
    { title: this.localizationService.instant('Exam::ExamTime'), render: 'examTime' },
    {
      title: this.localizationService.instant('Exam::Actions'),
      width: '220px',
      buttons: [
        {
          icon: 'edit',
          type: 'modal',
          iif: record => {
            return this.permissionService.getGrantedPolicy('Exam.Exams.Update') && record.status === ExaminationStatus.Draft;
          },
          modal: {
            component: ExamManagementExamEditComponent,
            params: (record: any) => ({
              examId: record.id
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
          iif: record => {
            return this.permissionService.getGrantedPolicy('Exam.Exams.Delete') && record.status === ExaminationStatus.Draft;
          },
          click: (record, _modal, component) => {
            this.examService.delete(record.id).subscribe(response => {
              this.messageService.success(this.localizationService.instant('Exam::DeletedSuccessfully', record.name));
              // tslint:disable-next-line: no-non-null-assertion
              component!.removeRow(record);
            });
          }
        },
        {
          iif: record => {
            return record.status !== ExaminationStatus.Draft && record.status !== ExaminationStatus.Cancelled;
          },
          text: this.localizationService.instant('Exam::ExamRecord'),
          click: record => {
            this.router.navigateByUrl(`/exam-management/user-exam-user?examId=${record.id}`);
          }
        },
        {
          text: this.localizationService.instant('Exam::More'),
          children: [
            {
              text: this.localizationService.instant('Exam::Publish'),
              iif: record => {
                return this.permissionService.getGrantedPolicy('Exam.Exams.Publish') && record.status === ExaminationStatus.Draft;
              },
              click: (record, _modal, component) => {
                this.examService.publish(record.id).subscribe(response => {
                  this.st.reload();
                });
              }
            },
            {
              text: this.localizationService.instant('Exam::Terminate'),
              iif: record => {
                return this.permissionService.getGrantedPolicy('Exam.Exams.Terminate') && record.status === ExaminationStatus.Published;
              },
              click: (record, _modal, component) => {
                this.examService.terminate(record.id).subscribe(response => {
                  this.st.reload();
                });
              }
            },
            {
              text: this.localizationService.instant('Exam::Complete'),
              iif: record => {
                return this.permissionService.getGrantedPolicy('Exam.Exams.Complete') && record.status === ExaminationStatus.Grading;
              },
              click: (record, _modal, component) => {
                this.examService.complete(record.id).subscribe(response => {
                  this.st.reload();
                });
              }
            },
            {
              text: this.localizationService.instant('Exam::Cancel'),
              iif: record => {
                return (
                  this.permissionService.getGrantedPolicy('Exam.Exams.Cancel') &&
                  record.status !== ExaminationStatus.Cancelled &&
                  record.status !== ExaminationStatus.Draft
                );
              },
              click: (record, _modal, component) => {
                this.examService.cancel(record.id).subscribe(response => {
                  this.st.reload();
                });
              }
            }
          ]
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
    this.examService
      .getList(this.params)
      .pipe(tap(() => (this.loading = false)))
      .subscribe(response => ((this.exams = response.items), (this.total = response.totalCount)));
  }
  resetParameters(): GetExamsInput {
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
    if (e.status !== null && e.status !== undefined) {
      this.params.status = e.status;
    } else {
      delete this.params.status;
    }
    this.st.load(1);
  }
  add() {
    this.modal.createStatic(ExamManagementExamEditComponent, { examId: null, paperId: null }).subscribe(() => this.st.reload());
  }
}
