import { Component, Input } from '@angular/core';
import { UserExamDetailDto_QuestionDto } from '@proxy/admin/exam-management/user-exams';

@Component({
  selector: 'choose',
  template: `
    <div class="chooses">
      <div
        *ngFor="let option of question.options; let j = index"
        class="choose-item"
        [ngClass]="isSelectedAnswer(question.answers, option.id) ? 'primary' : ''"
      >
        <div class="choose-item-content">
          <div>
            <span class="tag">{{ 65 + j | char }}</span>
            <span class="content">{{ option.content }}</span>
          </div>
          <div *ngIf="option.right">
            <span nz-icon nzType="check-circle" nzTheme="twotone" nzTwotoneColor="#52c41a"></span>
          </div>
        </div>
      </div>
    </div>
  `,
  styleUrl: './index.less'
})
export class ChooseComponent {
  @Input()
  question: UserExamDetailDto_QuestionDto;

  isSelectedAnswer(answers: string, optionId: string) {
    return answers !== null && answers.split('||').indexOf(optionId) !== -1;
  }
}
