import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoreModule } from '@abp/ng.core';
import { QuestionAnswerDto } from '@proxy/question-management/questions';
import { AnswerSubmission } from '@shared/components/choice-answer';
import { CharPipe } from '@shared/pipes/char/char.pipe';

@Component({
  selector: 'app-multiple-choice',
  templateUrl: './multiple-choice.component.html',
  styleUrls: ['./multiple-choice.component.scss'],
  imports: [CommonModule, CoreModule, CharPipe],
})
export class MultipleChoiceComponent {
  @Input() options: QuestionAnswerDto[] = [];
  @Input() selectedAnswerIds: Set<string> = new Set();
  @Input() showAnalysis = false;
  @Input() disabled = false;

  @Output() submitted = new EventEmitter<AnswerSubmission>();
  @Output() answerChanged = new EventEmitter<string>();

  onOptionToggled(option: QuestionAnswerDto): void {
    this.answerChanged.emit(option.id);
  }

  isSelectedOption(option: QuestionAnswerDto): boolean {
    return this.selectedAnswerIds.has(option.id);
  }

  onSubmit(): void {
    const rightAnswerIds = this.options.filter(opt => opt.right).map(opt => opt.id);
    const isCorrect = this.isAnswerCorrect(this.selectedAnswerIds, rightAnswerIds);

    this.submitted.emit({
      answerIds: this.selectedAnswerIds,
      isCorrect: isCorrect,
    });
  }

  isSubmitDisabled(): boolean {
    return this.disabled || this.selectedAnswerIds.size === 0;
  }

  private isAnswerCorrect(selectedIds: Set<string>, rightIds: string[]): boolean {
    if (selectedIds.size !== rightIds.length) {
      return false;
    }
    const selectedSet = new Set(selectedIds);
    return rightIds.every(id => selectedSet.has(id));
  }
}
