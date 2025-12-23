import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoreModule } from '@abp/ng.core';
import { UserExamDetailDto_SectionDto_QuestionDto } from '@proxy/exam-management/user-exams';
import { SingleChoiceComponent } from '@shared/components/single-choice/single-choice.component';
import { MultipleChoiceComponent } from '@shared/components/multiple-choice/multiple-choice.component';
import { FillBlankComponent } from '@shared/components/fill-blank/fill-blank.component';
import { AnswerSubmission } from '@shared/components/choice-answer';

@Component({
  selector: 'app-single-question',
  templateUrl: './single-question.component.html',
  standalone: true,
  imports: [
    CommonModule,
    CoreModule,
    SingleChoiceComponent,
    MultipleChoiceComponent,
    FillBlankComponent,
  ],
})
export class SingleQuestionComponent {
  @Input() question!: UserExamDetailDto_SectionDto_QuestionDto;
  @Input() questionIndex: number = 0;
  @Input() totalQuestions: number = 0;
  @Input() selectedAnswers: Set<string> = new Set();

  @Output() singleChoiceSubmitted = new EventEmitter<{
    questionId: string;
    answers: Set<string>;
  }>();
  @Output() answerChanged = new EventEmitter<{ questionId: string; answerId: string }>();
  @Output() fillBlankChanged = new EventEmitter<{ questionId: string; answers: string[] }>();
  @Output() questionNavigation = new EventEmitter<number>();

  get isPrevDisabled(): boolean {
    return this.questionIndex === 0;
  }

  get isNextDisabled(): boolean {
    return this.questionIndex >= this.totalQuestions - 1;
  }

  get selectedAnswerId(): string | null {
    return this.selectedAnswers.size > 0 ? Array.from(this.selectedAnswers)[0] : null;
  }

  get fillBlankAnswers(): string[] {
    return this.selectedAnswers ? Array.from(this.selectedAnswers) : [];
  }

  onSingleChoiceSubmitted(submission: AnswerSubmission): void {
    this.singleChoiceSubmitted.emit({
      questionId: this.question.id!,
      answers: submission.answers,
    });
  }

  onAnswerChanged(answerId: string): void {
    this.answerChanged.emit({
      questionId: this.question.id!,
      answerId: answerId,
    });
  }

  onFillBlankChanged(answers: string[]): void {
    this.fillBlankChanged.emit({
      questionId: this.question.id!,
      answers: answers,
    });
  }

  onPrev(): void {
    if (!this.isPrevDisabled) {
      this.questionNavigation.emit(this.questionIndex - 1);
    }
  }

  onNext(): void {
    if (!this.isNextDisabled) {
      this.questionNavigation.emit(this.questionIndex + 1);
    }
  }
}
