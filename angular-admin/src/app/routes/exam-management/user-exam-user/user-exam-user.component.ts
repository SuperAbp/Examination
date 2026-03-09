import { CoreModule, LocalizationService } from '@abp/ng.core';
import { Location } from '@angular/common';
import { Component, inject, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PageHeaderModule } from '@delon/abc/page-header';
import { STChange, STColumn, STComponent, STModule, STPage } from '@delon/abc/st';
import { DelonFormModule, SFSchema, SFSchemaEnumType, SFSelectWidgetSchema } from '@delon/form';
import { ExaminationService, UserExamService } from '@proxy/admin/controllers';
import { IdentityUserService } from '@super-abp/ng.identity/proxy';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { map, tap } from 'rxjs/operators';
import { ExamManagementUserExamComponent } from '../user-exam/user-exam.component';
import { ExamUserExamDto } from '@proxy/admin/exam-management/exams';

@Component({
  selector: 'app-exam-management-user-exam-user',
  templateUrl: './user-exam-user.component.html',
  imports: [CoreModule, PageHeaderModule, DelonFormModule, STModule, NzCardModule, NzButtonModule]
})
export class ExamManagementUserExamUserComponent implements OnInit {
  private location = inject(Location);
  private route = inject(ActivatedRoute);
  private localizationService = inject(LocalizationService);
  private examinationService = inject(ExaminationService);
  private userExamUserService = inject(UserExamService);
  private userService = inject(IdentityUserService);
  private cdr = inject(ChangeDetectorRef);

  examId!: string;
  userExamUsers: ExamUserExamDto[];
  total: number;
  loading = false;
  page: STPage = {
    show: false
  };
  searchSchema: SFSchema = {
    properties: {}
  };
  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[] = [
    { title: this.localizationService.instant('Exam::Rank'), index: 'rank' },
    { title: this.localizationService.instant('Exam::User'), index: 'user' },
    {
      title: this.localizationService.instant('Exam::TotalCount'),
      index: 'totalCount'
    },
    { title: this.localizationService.instant('Exam::MaxScore'), index: 'maxScore' },
    { title: this.localizationService.instant('Exam::Passed'), index: 'isPassed', type: 'yn' },
    {
      title: this.localizationService.instant('Exam::Actions'),
      buttons: [
        {
          text: this.localizationService.instant('Exam::ExamRecord'),
          type: 'modal',
          modal: {
            component: ExamManagementUserExamComponent,
            params: (record: any) => ({
              examId: this.examId,
              userId: record.userId
            })
          }
        }
      ]
    }
  ];

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.examId = params['examId']!;
      this.getList();
    });
  }
  getList() {
    this.loading = true;
    this.examinationService
      .getExamUserExams(this.examId)
      .pipe(
        tap(() => {
          this.loading = false;
          this.cdr.detectChanges();
        })
      )
      .subscribe(response => {
        this.userExamUsers = response.items;
        this.cdr.detectChanges();
      });
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
