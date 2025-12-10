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
  @Output() questionSelected = new EventEmitter<string>();

  showQuestion(id: string) {
    this.questionSelected.emit(id);
  }

  getClassName(id: string): string {
    let className = 'bs-tag';
    if (this.selectedQuestionId === id) {
      className += ' bs-tag-primary';
    }
    return className;
  }
}
