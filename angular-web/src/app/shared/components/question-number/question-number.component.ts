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
  @Input() correctQuestionIds: Set<string> = new Set();
  @Input() incorrectQuestionIds: Set<string> = new Set();
  @Output() questionNumberSelected = new EventEmitter<string>();

  showQuestion(id: string) {
    this.questionNumberSelected.emit(id);
  }

  getClassName(id: string): string {
    let className = 'bs-tag';
    if (this.selectedQuestionId === id) {
      className += ' bs-tag-warning';
    } else {
      if (this.correctQuestionIds.has(id)) {
        className += ' bs-tag-success';
      } else if (this.incorrectQuestionIds.has(id)) {
        className += ' bs-tag-danger';
      }
    }
    return className;
  }
}
