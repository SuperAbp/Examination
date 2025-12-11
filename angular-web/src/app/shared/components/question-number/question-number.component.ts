import { Component, EventEmitter, Input, Output } from '@angular/core';
import { QuestionNumber } from './question-number';
import { CoreModule } from '@abp/ng.core';

@Component({
  selector: 'app-question-number',
  templateUrl: './question-number.component.html',
  styleUrls: ['./question-number.component.scss'],
  imports: [CoreModule],
})
export class QuestionNumberComponent {
  @Input() questionNumbers: QuestionNumber[] = [];
  @Input() selectedQuestionId: string;
  @Input() answerMap: Map<string, { answerId: string; right: boolean }> = new Map();
  @Output() questionNumberSelected = new EventEmitter<string>();

  showQuestion(id: string) {
    this.questionNumberSelected.emit(id);
  }

  getClassName(id: string): string {
    let className = 'bs-tag';
    if (this.selectedQuestionId === id) {
      className += ' bs-tag-warning';
    }
    // 根据答案状态添加正确或错误的class
    if (this.answerMap.has(id)) {
      const answerState = this.answerMap.get(id);
      if (answerState.right) {
        className += ' bs-tag-success';
      } else {
        className += ' bs-tag-danger';
      }
    }
    return className;
  }
}
