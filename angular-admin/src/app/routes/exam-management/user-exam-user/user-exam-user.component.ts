import { ConfigStateService, CoreModule, LocalizationService, PermissionService } from '@abp/ng.core';
import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PageHeaderModule } from '@delon/abc/page-header';
import { STChange, STColumn, STComponent, STData, STModule, STPage } from '@delon/abc/st';
import { DelonFormModule, SFSchema } from '@delon/form';
import { ModalHelper } from '@delon/theme';
import { UserExamService } from '@proxy/admin/controllers';
import { GetUserExamWithUsersInput, UserExamWithUserDto } from '@proxy/admin/exam-management/user-exams';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzMessageService } from 'ng-zorro-antd/message';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-exam-management-user-exam-user',
  templateUrl: './user-exam-user.component.html',
  standalone: true,
  imports: [CoreModule, PageHeaderModule, DelonFormModule, STModule, NzCardModule, NzButtonModule]
})
export class ExamManagementUserExamUserComponent implements OnInit {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private modal = inject(ModalHelper);
  private localizationService = inject(LocalizationService);
  private messageService = inject(NzMessageService);
  private permissionService = inject(PermissionService);
  private userExamUserService = inject(UserExamService);

  examId!: string;
  userExamUsers: UserExamWithUserDto[];
  total: number;
  loading = false;
  params: GetUserExamWithUsersInput;
  page: STPage = {
    show: true,
    showSize: true,
    front: false,
    pageSizes: [10, 20, 30, 40, 50]
  };
  searchSchema: SFSchema = {
    properties: {}
  };
  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[] = [
    { title: this.localizationService.instant('Exam::User'), index: 'user' },
    { title: this.localizationService.instant('Exam::TotalCount'), index: 'totalCount' },
    { title: this.localizationService.instant('Exam::MaxScore'), index: 'maxScore' },
    {
      title: this.localizationService.instant('Exam::Actions'),
      buttons: [
        {
          icon: 'info',
          type: 'modal',
          click: (record: STData, modal?: any, instance?: STComponent) => {
            this.router.navigateByUrl(`/exam-management/user-exam?examId=${this.examId}&userId=${record['userId']}`);
          }
        }
      ]
    }
  ];

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.examId = params['examId']!;
      this.params = this.resetParameters();
      this.getList();
    });
  }
  getList() {
    this.loading = true;
    this.userExamUserService
      .getListWithUser(this.params)
      .pipe(tap(() => (this.loading = false)))
      .subscribe(response => ((this.userExamUsers = response.items), (this.total = response.totalCount)));
  }
  resetParameters(): GetUserExamWithUsersInput {
    return {
      examId: this.examId,
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
}
