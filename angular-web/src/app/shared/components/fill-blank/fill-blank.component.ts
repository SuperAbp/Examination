import {
  Component,
  EventEmitter,
  Input,
  Output,
  OnInit,
  OnChanges,
  SimpleChanges,
} from '@angular/core';

import { CoreModule } from '@abp/ng.core';
import { FormsModule } from '@angular/forms';
import { QuestionOptionDto } from '@proxy/question-management/questions';
import { AnswerSubmission } from '@shared/components/choice-answer';

@Component({
  selector: 'app-fill-blank',
  templateUrl: './fill-blank.component.html',
  styleUrls: ['./fill-blank.component.scss'],
  imports: [CoreModule, FormsModule],
})
export class FillBlankComponent implements OnInit, OnChanges {
  @Input() options: QuestionOptionDto[] = [];
  @Input() fixedOrder: boolean = false;
  @Input() optionsCount: number = 0;
  @Input() selectedAnswers: string[] = [];
  @Input() showAnalysis = false;
  @Input() disabled = false;
  @Input() hideSubmitButton = false; // 隐藏提交按钮（用于考试环境）

  @Output() submitted = new EventEmitter<AnswerSubmission>();
  @Output() answerChanged = new EventEmitter<string[]>();

  answers: string[] = [];

  ngOnInit(): void {
    this.initializeAnswers();
  }

  ngOnChanges(changes: SimpleChanges): void {
    // 只在 options 变化时（即切换题目时）才重新初始化
    if (changes['options'] && !changes['options'].firstChange) {
      this.initializeAnswers();
    }
  }

  private initializeAnswers(): void {
    this.answers =
      this.selectedAnswers.length > 0
        ? [...this.selectedAnswers]
        : new Array(this.optionsCount).fill('');
  }

  onAnswerChange(): void {
    this.answerChanged.emit(this.answers);
  }

  onSubmit(): void {
    const isCorrect = this.isAnswerCorrect();

    // 使用用户输入的文本作为答案
    const userAnswers = new Set<string>(this.answers);

    this.submitted.emit({
      answers: userAnswers,
      isCorrect: isCorrect,
    });
  }

  isSubmitDisabled(): boolean {
    return this.disabled || this.answers.some(answer => !answer || answer.trim() === '');
  }

  isAnswerCorrect(): boolean {
    // 检查所有用户输入是否都在正确答案中，不需要顺序一致
    const correctAnswers = this.options.map(opt => opt.content?.trim().toLowerCase() || '');
    const userAnswers = this.answers.map(ans => ans?.trim().toLowerCase() || '');

    // 检查用户答案数量是否与题目要求一致
    if (userAnswers.length !== correctAnswers.length) {
      return false;
    }

    // 检查每个用户答案是否都在正确答案集合中
    return userAnswers.every(userAns => correctAnswers.includes(userAns));
  }
}
