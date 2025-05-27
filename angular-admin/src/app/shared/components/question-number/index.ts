import { Component, Input, input } from '@angular/core';

export class QuestionNumber {
  questionType: number;
  totalScore: number;
  questions: QuestionNumberItem[];
}
export class QuestionNumberItem {
  id: string;
  score: number;
}

@Component({
  selector: 'question-number',
  styles: [
    `
      nz-tag {
        cursor: pointer;
      }
    `
  ],
  template: `
    <nz-card>
      <div style="margin-bottom: 16px;" *ngFor="let questionNumber of questionNumbers">
        <div nz-flex nzJustify="space-between" nzAlign="center">
          <h4>
            {{ 'Exam::QuestionType:' + questionNumber.questionType | abpLocalization }}
          </h4>
          <span>{{ 'Exam::TotalScore{0}' | abpLocalization: questionNumber.totalScore + '' }}</span>
        </div>
        <div>
          <nz-space>
            <ng-container *ngFor="let questionNumberOption of questionNumber.questions; let i = index">
              <nz-tag (click)="scrollTo(questionNumberOption.id)" [nzColor]="getColor(questionNumberOption.id)">{{ i }}</nz-tag>
            </ng-container>
          </nz-space>
        </div>
      </div>
    </nz-card>
  `
})
export class QuestionNumberComponent {
  @Input()
  questionNumbers: QuestionNumber[];

  getColor(questionId) {
    return 'success';
  }

  scrollTo(id: string) {
    const el = document.getElementById(id);
    if (el) {
      el.scrollIntoView({ behavior: 'smooth' });
    }
  }
}
