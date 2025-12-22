import { Component, Input, input } from '@angular/core';

export class QuestionNumber {
  title: string;
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
      @for (questionNumber of questionNumbers; track $index) {
        <div style="margin-bottom: 16px;">
          <div nz-flex nzJustify="space-between" nzAlign="center">
            <h4>
              {{ questionNumber.title }}
            </h4>
            <span>{{ 'Exam::TotalScore{0}' | abpLocalization: questionNumber.totalScore + '' }}</span>
          </div>
          <div>
            <nz-space nzWrap>
              @for (questionNumberOption of questionNumber.questions; track $index; let i = $index) {
                <nz-tag (click)="scrollTo(questionNumberOption.id)" [nzColor]="getColor(questionNumberOption.id)">{{ i + 1 }}</nz-tag>
              }
            </nz-space>
          </div>
        </div>
      }
    </nz-card>
  `,
  standalone: false
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
