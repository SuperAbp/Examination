import { CoreModule, ListService, LocalizationService, PagedResultDto } from '@abp/ng.core';
import { NgxDatatableDefaultDirective, NgxDatatableListDirective } from '@abp/ng.theme.shared';
import { Component, inject, OnInit } from '@angular/core';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectComponent } from '@ng-select/ng-select';
import { UserExamService, ExaminationService } from '@proxy/controllers';
import { UserExamListDto, GetUserExamsInput } from '@proxy/exam-management/user-exams';
import { ExamListDto } from '@proxy/exam-management/exams';

@Component({
  selector: 'app-my-exams',
  templateUrl: './my-exams.component.html',
  styleUrls: ['./my-exams.component.scss'],
  providers: [ListService],
  imports: [
    CoreModule,
    DatePipe,
    FormsModule,
    NgSelectComponent,
    NgxDatatableModule,
    NgxDatatableDefaultDirective,
    NgxDatatableListDirective,
  ],
})
export class MyExamsComponent implements OnInit {
  exams = { items: [], totalCount: 0 } as PagedResultDto<UserExamListDto>;
  protected readonly list = inject(ListService<GetUserExamsInput>);
  private localizationService = inject(LocalizationService);
  private readonly userExamService = inject(UserExamService);
  private readonly examinationService = inject(ExaminationService);
  private readonly router = inject(Router);

  filters: Partial<GetUserExamsInput> = {
    examId: undefined,
  };

  examList: ExamListDto[] = [];

  ngOnInit() {
    this.loadExams();

    this.list
      .hookToQuery(query => {
        query.examId = this.filters.examId;
        return this.userExamService.getList(query);
      })
      .subscribe(response => {
        this.exams = response;
      });
  }

  loadExams() {
    this.examinationService
      .getList({
        maxResultCount: 1000,
      })
      .subscribe(response => {
        this.examList = response.items;
      });
  }

  search() {
    this.list.get();
  }

  clearFilters() {
    this.filters = {
      examId: undefined,
    };
    this.search();
  }

  viewExam(id: string) {
    this.router.navigate(['/exams/submitted', id]);
  }
}
