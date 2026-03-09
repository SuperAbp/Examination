import { CoreModule, PagedResultDto } from '@abp/ng.core';
import { Component, inject, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { UserExamService } from '@proxy/controllers';
import { UserExamDetailDto } from '@proxy/exam-management/user-exams';
import { DatePipe } from '@angular/common';
import { QuestionNumberComponent } from '@shared/components/question-number/question-number.component';
import { SingleChoiceComponent } from '@shared/components/single-choice/single-choice.component';
import { MultipleChoiceComponent } from '@shared/components/multiple-choice/multiple-choice.component';
import { FillBlankComponent } from '@shared/components/fill-blank/fill-blank.component';
import { QuestionNumber } from '@shared/components/question-number/question-number';

@Component({
  selector: 'app-exam-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.scss'],
  imports: [
    CoreModule,
    QuestionNumberComponent,
    SingleChoiceComponent,
    MultipleChoiceComponent,
    FillBlankComponent,
  ],
})
export class ExamDetailComponent implements OnInit {
  private readonly userExamService = inject(UserExamService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly offcanvasService = inject(NgbOffcanvas);

  userExamId: string;
  examDetail: UserExamDetailDto;
  loading = true;
  questionNumbers: QuestionNumber[] = [];
  correctQuestionIds: Set<string> = new Set();
  incorrectQuestionIds: Set<string> = new Set();

  ngOnInit() {
    this.userExamId = this.route.snapshot.paramMap.get('id');
    this.loadDetail();
  }

  loadDetail() {
    this.loading = true;
    this.userExamService.get(this.userExamId).subscribe({
      next: response => {
        this.examDetail = response;
        if (this.examDetail.status !== 3) {
          this.router.navigate(['/my/exams']);
          return;
        }
        this.buildQuestionNumbers();
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.router.navigate(['/my/exams']);
      },
    });
  }

  buildQuestionNumbers() {
    this.examDetail.sections?.forEach(section => {
      const qn = new QuestionNumber(section.id ? 0 : 0, section.title, section.totalScore);
      const questionIds: string[] = [];

      section.questions?.forEach(question => {
        questionIds.push(question.id);
        if (question.right === true) {
          this.correctQuestionIds.add(question.id);
        } else if (question.right === false) {
          this.incorrectQuestionIds.add(question.id);
        }
      });

      qn.addQuestionIds(questionIds);
      this.questionNumbers.push(qn);
    });
  }

  goBack() {
    this.router.navigate(['/my/exams']);
  }

  openQuestionNumberOffcanvas(content: TemplateRef<any>) {
    this.offcanvasService.open(content, {
      position: 'bottom',
      panelClass: 'question-number-offcanvas',
    });
  }

  scrollToQuestion(questionId: string) {
    const element = document.getElementById('question-' + questionId);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }

  getChineseNumber(num: number): string {
    const chineseNumbers = ['零', '一', '二', '三', '四', '五', '六', '七', '八', '九', '十'];
    if (num <= 10) {
      return chineseNumbers[num];
    }
    return num.toString();
  }

  getOptionLabel(index: number): string {
    return String.fromCharCode(65 + index); // A, B, C, D...
  }

  getSelectedAnswerIds(question: any): Set<string> {
    const ids = new Set<string>();
    if (!question.answers) return ids;

    const answerIds = question.answers
      .split('||')
      .map((a: string) => a.trim())
      .filter(Boolean);

    answerIds.forEach((answerId: string) => {
      const option = question.options?.find((opt: any) => opt.id === answerId);
      if (option) {
        ids.add(option.id);
      }
    });

    return ids;
  }

  getSelectedAnswerId(question: any): string | null {
    const ids = this.getSelectedAnswerIds(question);
    return ids.size > 0 ? Array.from(ids)[0] : null;
  }

  getFillBlankAnswers(question: any): string[] {
    if (!question.answers) return [];
    return question.answers.split('||').map((a: string) => a.trim());
  }

  getTotalQuestions(): number {
    if (this.examDetail?.sections && this.examDetail.sections.length > 0) {
      return this.examDetail.sections.reduce(
        (sum, section) => sum + (section.questions?.length || 0),
        0,
      );
    }
    return 0;
  }

  getCorrectCount(): number {
    if (this.examDetail?.sections && this.examDetail.sections.length > 0) {
      return this.examDetail.sections.reduce((sum, section) => {
        return sum + (section.questions?.filter(q => q.right === true).length || 0);
      }, 0);
    }
    return 0;
  }

  getWrongCount(): number {
    if (this.examDetail?.sections && this.examDetail.sections.length > 0) {
      return this.examDetail.sections.reduce((sum, section) => {
        return sum + (section.questions?.filter(q => q.right === false && q.answers).length || 0);
      }, 0);
    }
    return 0;
  }

  getUnansweredCount(): number {
    if (this.examDetail?.sections && this.examDetail.sections.length > 0) {
      return this.examDetail.sections.reduce((sum, section) => {
        return sum + (section.questions?.filter(q => !q.answers || q.answers === '').length || 0);
      }, 0);
    }
    return 0;
  }

  getAccuracyRate(): number {
    const total = this.getTotalQuestions();
    if (total === 0) return 0;
    const correct = this.getCorrectCount();
    return Math.round((correct / total) * 100);
  }
}
