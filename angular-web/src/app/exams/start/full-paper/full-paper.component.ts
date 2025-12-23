import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoreModule } from '@abp/ng.core';
import {
  UserExamDetailDto_SectionDto,
  UserExamDetailDto_SectionDto_QuestionDto,
} from '@proxy/exam-management/user-exams';
import { SingleChoiceComponent } from '@shared/components/single-choice/single-choice.component';
import { MultipleChoiceComponent } from '@shared/components/multiple-choice/multiple-choice.component';
import { FillBlankComponent } from '@shared/components/fill-blank/fill-blank.component';

@Component({
  selector: 'app-full-paper',
  templateUrl: './full-paper.component.html',
  standalone: true,
  imports: [
    CommonModule,
    CoreModule,
    SingleChoiceComponent,
    MultipleChoiceComponent,
    FillBlankComponent,
  ],
})
export class FullPaperComponent {
  @Input() sections: UserExamDetailDto_SectionDto[] = [];
  @Input() answerMap: Map<string, Set<string>> = new Map();

  @Output() questionAnswered = new EventEmitter<{ questionId: string; answers: Set<string> }>();
  @Output() multipleChoiceChanged = new EventEmitter<{ questionId: string; answerId: string }>();
  @Output() fillBlankChanged = new EventEmitter<{ questionId: string; answers: string[] }>();

  getSingleChoiceAnswer(questionId: string): string | null {
    const answers = this.answerMap.get(questionId);
    return answers && answers.size > 0 ? Array.from(answers)[0] : null;
  }

  getQuestionAnswers(questionId: string): Set<string> {
    return this.answerMap.get(questionId) || new Set<string>();
  }

  getFillBlankAnswers(questionId: string): string[] {
    const answers = this.answerMap.get(questionId);
    return answers ? Array.from(answers) : [];
  }

  onQuestionAnswered(questionId: string, answers: Set<string>): void {
    this.questionAnswered.emit({ questionId, answers });
  }

  onMultipleChoiceChanged(questionId: string, answerId: string): void {
    this.multipleChoiceChanged.emit({ questionId, answerId });
  }

  onFillBlankChanged(questionId: string, answers: string[]): void {
    this.fillBlankChanged.emit({ questionId, answers });
  }

  // 获取题目的绝对索引（用于显示第几题）
  getQuestionNumber(sectionIndex: number, questionIndex: number): number {
    let count = 0;
    for (let i = 0; i < sectionIndex; i++) {
      count += this.sections[i].questions.length;
    }
    return count + questionIndex + 1;
  }

  // 将数字转换为中文数字
  getChineseNumber(num: number): string {
    const chineseNumbers = ['', '一', '二', '三', '四', '五', '六', '七', '八', '九', '十'];
    if (num <= 10) {
      return chineseNumbers[num];
    } else if (num < 20) {
      return '十' + chineseNumbers[num - 10];
    } else {
      const tens = Math.floor(num / 10);
      const ones = num % 10;
      return chineseNumbers[tens] + '十' + (ones > 0 ? chineseNumbers[ones] : '');
    }
  }
}
