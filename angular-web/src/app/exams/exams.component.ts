import { Component, OnInit, inject } from '@angular/core';
import { CoreModule, ListService, PagedResultDto, ConfigStateService } from '@abp/ng.core';
import { ExaminationService, UserExamService } from '@proxy/controllers';
import { ExamListDto, GetExamsInput } from '@proxy/exam-management/exams';
import { UserExamDetailDto } from '@proxy/exam-management/user-exams';
import { Router } from '@angular/router';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgxDatatableDefaultDirective, NgxDatatableListDirective } from '@abp/ng.theme.shared';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { NgbAlert } from '@ng-bootstrap/ng-bootstrap';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'app-exams',
  templateUrl: './exams.component.html',
  providers: [ListService],
  imports: [
    CoreModule,
    NgxDatatableModule,
    NgxDatatableDefaultDirective,
    NgxDatatableListDirective,
    ReactiveFormsModule,
    NgbAlert,
  ],
})
export class ExamsComponent implements OnInit {
  private examinationService = inject(ExaminationService);
  private userExamService = inject(UserExamService);
  private router = inject(Router);
  private fb = inject(FormBuilder);
  private configState = inject(ConfigStateService);

  exams: PagedResultDto<ExamListDto> = {
    items: [],
    totalCount: 0,
  };

  list = inject(ListService);
  searchForm: FormGroup;
  unfinishedExam: UserExamDetailDto | null = null;
  bufferTime: number = 0;

  constructor() {
    this.searchForm = this.fb.group({
      name: [''],
    });
  }

  ngOnInit() {
    this.bufferTime = Number(this.configState.getSetting('Exam.BufferTime')) || 0;

    this.loadUnfinishedExam();

    const examStreamCreator = (query: GetExamsInput) => {
      const searchValue = this.searchForm.get('name')?.value;
      return this.examinationService.getList({
        ...query,
        name: searchValue || undefined,
        status: 1,
      });
    };

    this.list.hookToQuery(examStreamCreator).subscribe(response => {
      this.exams = response;
    });
  }

  loadUnfinishedExam() {
    this.userExamService
      .getUnfinished()
      .pipe(catchError(() => of(null)))
      .subscribe(exam => {
        this.unfinishedExam = exam;
      });
  }

  search() {
    this.list.get();
  }

  clearSearch() {
    this.searchForm.patchValue({ name: '' });
    this.list.get();
  }

  StartAsync(id: string) {
    this.router.navigate(['/exams/welcome', id]);
  }

  isExamAvailable(exam: ExamListDto): boolean {
    const now = new Date().getTime();
    const startTime = new Date(exam.startTime).getTime();
    const endTime = new Date(exam.endTime).getTime();
    const bufferTimeMs = this.bufferTime * 60 * 1000;

    return exam.status === 1 && now >= startTime - bufferTimeMs && now <= endTime + bufferTimeMs;
  }
}
