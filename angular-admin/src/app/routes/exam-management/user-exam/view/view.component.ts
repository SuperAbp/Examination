import { CoreModule } from '@abp/ng.core';
import { Location } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { FooterToolbarModule } from '@delon/abc/footer-toolbar';
import { PageHeaderComponent } from '@delon/abc/page-header';
import { UserExamService } from '@proxy/admin/controllers';
import { UserExamDetailDto, UserExamDetailDto_QuestionDto } from '@proxy/admin/exam-management/user-exams';
import { SharedModule } from '@shared';
import { NzAffixModule } from 'ng-zorro-antd/affix';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { finalize, tap } from 'rxjs';
import { QuestionNumber, QuestionNumberItem } from 'src/app/shared/components/question-number';

@Component({
  selector: 'app-exam-management-user-exam-view',
  templateUrl: './view.component.html',
  styleUrl: './view.component.less',
  standalone: true,
  imports: [
    CoreModule,
    PageHeaderComponent,
    FooterToolbarModule,
    SharedModule,
    NzCardModule,
    NzSpinModule,
    NzButtonModule,
    NzRadioModule,
    NzIconModule,
    NzSwitchModule,
    NzAffixModule,
    NzDescriptionsModule
  ]
})
export class ExamManagementUserExamViewComponent implements OnInit {
  userExamId: string;
  userExam: UserExamDetailDto;
  loading: boolean;
  chineseNumber = ['一', '二', '三', '四', '五', '六', '七', '八', '九', '十'];
  form: FormGroup = null;
  isConfirmLoading = false;

  private location = inject(Location);
  private route = inject(ActivatedRoute);
  private messageService = inject(NzMessageService);
  private userExamService = inject(UserExamService);
  private fb = inject(FormBuilder);

  get questionForm(): FormArray {
    return this.form.get('questions') as FormArray;
  }
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

  get isReview() {
    return this.userExam.status === 3 || this.userExam.status === 2;
  }
  getOptions(question: UserExamDetailDto_QuestionDto) {
    return question.options.map(o => o.content).join('||');
  }
  getAnswer(amswers: string) {
    return amswers != null && amswers.split('||');
  }

  getQuestionNumbers(): QuestionNumber[] {
    const questionTypes = this.questionTypes;
    let questionNumbers: QuestionNumber[] = [];
    questionTypes.forEach(t => {
      let questionType = +t;
      let currentQuestions = this.questions
        .filter(q => q.questionType == questionType)
        .map(q => {
          return { id: q.id, score: q.questionScore };
        });
      let questionNumber: QuestionNumber = {
        questionType: questionType,
        questions: currentQuestions,
        totalScore: currentQuestions.reduce((acc, item) => acc + item.score, 0)
      };
      questionNumbers.push(questionNumber);
    });
    return questionNumbers;
  }

  ngOnInit(): void {
    this.loading = true;
    this.userExamId = this.route.snapshot.paramMap.get('id');
    this.userExamService.get(this.userExamId).subscribe(result => {
      this.userExam = result;
      this.loading = false;

      this.buildForm();
    });
  }

  buildForm() {
    this.form = this.fb.group({
      questions: this.fb.array([])
    });
    this.questions
      .filter(q => q.questionType == 3)
      .forEach(q => {
        let questionForm = this.fb.group({
          questionId: [q.id, [Validators.required]],
          right: [q.right || false, [Validators.required]],
          score: [q.score || 0, [Validators.required, Validators.max(q.questionScore)]],
          reason: [q.reason]
        });
        this.questionForm.push(questionForm);
      });
  }

  save() {
    if (!this.form.valid || this.isConfirmLoading) {
      for (const key of Object.keys(this.form.controls)) {
        this.form.controls[key].markAsDirty();
        this.form.controls[key].updateValueAndValidity();
      }
      return;
    }
    this.isConfirmLoading = true;
    console.log(this.questionForm.value);
    this.userExamService
      .reviewQuestions(this.userExamId, this.questionForm.value)
      .pipe(
        tap(() => {
          this.goback();
        }),
        finalize(() => (this.isConfirmLoading = false))
      )
      .subscribe();
  }
  back(e: MouseEvent) {
    e.preventDefault();
    this.goback();
  }
  goback() {
    this.location.back();
  }
}
