import { CoreModule } from '@abp/ng.core';
import { Location } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PageHeaderModule } from '@delon/abc/page-header';
import { SVModule } from '@delon/abc/sv';
import { ExaminationService } from '@proxy/admin/controllers';
import { ExamDetailDto } from '@proxy/admin/exam-management/exams';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzSpinModule } from 'ng-zorro-antd/spin';

@Component({
  selector: 'app-exam-management-exam-view',
  templateUrl: './view.component.html',
  standalone: true,
  imports: [CoreModule, PageHeaderModule, NzCardModule, NzSpinModule, SVModule]
})
export class ExamManagementExamViewComponent implements OnInit {
  examId: string;
  exam: ExamDetailDto;

  private location = inject(Location);
  private route = inject(ActivatedRoute);
  public messageService = inject(NzMessageService);
  private examService = inject(ExaminationService);

  ngOnInit(): void {
    this.examId = this.route.snapshot.paramMap.get('id');

    this.examService.get(this.examId).subscribe(result => (this.exam = result));
  }
  back(e: MouseEvent) {
    e.preventDefault();
    this.location.back();
  }
}
