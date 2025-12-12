import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AnswerSubmission } from '@shared/components/choice-answer';
import { CharPipe } from '@shared/pipes/char/char.pipe';
import { QuestionOptionDto } from '@proxy/question-management/questions';

@Component({
  selector: 'app-single-choice',
  templateUrl: './single-choice.component.html',
  styleUrls: ['./single-choice.component.scss'],
  imports: [CommonModule, CharPipe],
})
export class SingleChoiceComponent {
  @Input() options: QuestionOptionDto[] = [];
  @Input() selectedAnswerId: string | null = null;
  @Input() showAnalysis = false;
  @Input() disabled = false;

  @Output() submitted = new EventEmitter<AnswerSubmission>();

  onOptionSelected(option: QuestionOptionDto): void {
    if (!this.disabled) {
      this.submitted.emit({
        answerIds: new Set([option.id]),
        isCorrect: option.right,
      });
    }
  }

  isSelectedOption(option: QuestionOptionDto): boolean {
    return this.selectedAnswerId === option.id;
  }
}
