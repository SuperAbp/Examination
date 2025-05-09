import { Component } from '@angular/core';

class QuestionNumber {
  questionType: number;
  totalScore: number;
  questions: QuestionNumberItem[];
}
class QuestionNumberItem {
  id: string;
  score: number;
}

@Component({
  selector: 'question-number',
  template: `
    <nz-card>
      <div *ngFor="let questionNumber of questionNumbers">
        <h4>{{ 'QuestionType:' + questionNumber.questionType | abpLocalization }}</h4>
        <span>{{ 'TotalScore{0}' + questionNumber.totalScore | abpLocalization }}</span>
        <div *ngFor="let questionNumberOption of questionNumber.questions; let i = index">
          <nz-tag [nzColor]="getColor(questionNumberOption.id)">{{ i }}</nz-tag>
        </div>
      </div>
    </nz-card>
  `
})
export class QuestionNumberComponent {
  questionNumbers: QuestionNumber[];

  getColor(questionId) {
    return 'success';
  }
}
