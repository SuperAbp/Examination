import { CoreModule } from '@abp/ng.core';
import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PageHeaderComponent } from '@delon/abc/page-header';
import { UserExamService } from '@proxy/admin/controllers';
import { UserExamDetailDto, UserExamDetailDto_QuestionDto } from '@proxy/admin/exam-management/user-exams';
import { SharedModule } from '@shared';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzSwitchModule } from 'ng-zorro-antd/switch';

@Component({
  selector: 'app-exam-management-user-exam-view',
  templateUrl: './view.component.html',
  styleUrl: './view.component.less',
  standalone: true,
  imports: [
    CoreModule,
    PageHeaderComponent,
    SharedModule,
    NzCardModule,
    NzSpinModule,
    NzButtonModule,
    NzRadioModule,
    NzIconModule,
    NzSwitchModule
  ]
})
export class ExamManagementUserExamViewComponent implements OnInit {
  userExamId: string;
  userExam: UserExamDetailDto;
  loading: boolean;
  chineseNumber = ['一', '二', '三', '四', '五', '六', '七', '八', '九', '十'];

  constructor(
    private location: Location,
    private route: ActivatedRoute,
    public messageService: NzMessageService,
    private userExamService: UserExamService
  ) {}

  get questions() {
    return this.userExam.questions;
  }
  get questionTypeMaps() {
    return this.questions.reduce((acc: { [key: number]: UserExamDetailDto_QuestionDto[] }, item) => {
      const key = item.questionType;
      if (!acc[key]) {
        acc[key] = [];
      }
      acc[key].push(item);
      return acc;
    }, {});
  }
  get questionTypes() {
    return Object.keys(this.questionTypeMaps);
  }

  getOptions(question: UserExamDetailDto_QuestionDto) {
    return question.options.map(o => o.content).join('||');
  }
  getAnswer(amswers: string) {
    return amswers != null && amswers.split('||');
  }

  ngOnInit(): void {
    this.loading = true;
    this.userExamId = this.route.snapshot.paramMap.get('id');
    this.userExamService.get(this.userExamId).subscribe(result => {
      this.userExam = result;
      this.loading = false;
    });
  }
  back(e: MouseEvent) {
    e.preventDefault();
    this.location.back();
  }
}
