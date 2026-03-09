import { CoreModule, LocalizationService, PermissionService } from '@abp/ng.core';
import { Location } from '@angular/common';
import { Component, inject, Input, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PageHeaderModule } from '@delon/abc/page-header';
import { STChange, STColumn, STComponent, STData, STModule, STPage } from '@delon/abc/st';
import { DelonFormModule, SFSchema } from '@delon/form';
import { UserExamService } from '@proxy/admin/controllers';
import { GetUserExamsInput, UserExamListDto } from '@proxy/admin/exam-management/user-exams';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzMessageService } from 'ng-zorro-antd/message';
import { tap } from 'rxjs/operators';
import { UserExamStatus } from '@shared';
import { NzIconModule } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-exam-management-user-exam',
  templateUrl: './user-exam.component.html',
  imports: [CoreModule, PageHeaderModule, DelonFormModule, STModule, NzCardModule, NzButtonModule, NzIconModule]
})
export class ExamManagementUserExamComponent implements OnInit {
  @Input()
  examId!: string;
  @Input()
  userId!: string;

  private router = inject(Router);

  private location = inject(Location);
  private route = inject(ActivatedRoute);
  private localizationService = inject(LocalizationService);
  private messageService = inject(NzMessageService);
  private permissionService = inject(PermissionService);
  private userExamService = inject(UserExamService);
  private cdr = inject(ChangeDetectorRef);

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
    { title: this.localizationService.instant('Exam::TotalScore'), index: 'totalScore' },
    { title: this.localizationService.instant('Exam::ExamTime'), render: 'examTime' },
    { title: this.localizationService.instant('Exam::Status'), render: 'status' },
    { title: this.localizationService.instant('Exam::Active'), index: 'isActive', type: 'yn' },
    { title: this.localizationService.instant('Exam::Passed'), render: 'passed' },
    {
      title: this.localizationService.instant('Exam::Actions'),
      buttons: [
        {
          icon: 'info',
          type: 'modal',
          iif: record => {
            return record.status === UserExamStatus.Scored || record.status === UserExamStatus.Submitted;
          },
          click: (record: STData, modal?: any, instance?: STComponent) => {
            const url = this.router.serializeUrl(this.router.createUrlTree(['/exam-management/user-exam/', record['id']]));
            const fullUrl = window.location.origin + this.location.prepareExternalUrl(url);
            window.open(fullUrl);
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
      .pipe(
        tap(() => {
          this.loading = false;
          this.cdr.detectChanges();
        })
      )
      .subscribe(response => {
        this.userExams = response.items;
        this.cdr.detectChanges();
      });
  }
  resetParameters(): GetUserExamsInput {
    return {
      examId: this.examId,
      userId: this.userId,
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
  back(e: MouseEvent) {
    e.preventDefault();
    this.location.back();
  }
}
