import { Component, inject, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionBankService, QuestionService } from '@proxy/controllers';
import { QuestionBankDetailDto } from '@proxy/question-management/question-banks';
import { GetQuestionsInput, QuestionDetailDto } from '@proxy/question-management/questions';
import { QuestionNumber } from '@shared/components/question-number/question-number';
import { QuestionNumberComponent } from '@shared/components/question-number/question-number.component';
import { CharPipe } from '@shared/pipes/char/char.pipe';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-question-banks-train',
  templateUrl: './train.component.html',
  styleUrls: ['./train.component.scss'],
  imports: [QuestionNumberComponent, CharPipe],
})
export class QuestionBanksTrainComponent implements OnInit {
  @Input() mode: number;
  @Input() type?: number;
  questionBankId: string;

  questionBank: QuestionBankDetailDto;
  questionNumbers: QuestionNumber[] = [];
  selectedQuestion: QuestionDetailDto;
  selectedQuestionId: string;

  private readonly questionBankService = inject(QuestionBankService);
  private readonly questionService = inject(QuestionService);
  private readonly router = inject(Router);
  private readonly activatedRoute = inject(ActivatedRoute);
  constructor() {
    this.activatedRoute.params.subscribe(params => {
      this.questionBankId = params['id'];
    });
  }

  ngOnInit() {
    forkJoin([
      this.questionBankService.get(this.questionBankId),
      this.questionService.getList({
        questionBankId: this.questionBankId,
        questionType: this.type,
      } as GetQuestionsInput),
    ]).subscribe(([questionBank, questionsResult]) => {
      this.questionBank = questionBank;

      this.questionNumbers = [];
      if (questionsResult && questionsResult.items) {
        // 按questionType分组并转换为QuestionNumber数组
        this.questionNumbers = Array.from(
          questionsResult.items
            .reduce((acc, question) => {
              const type = question.questionType;
              if (!acc.has(type)) {
                acc.set(type, new QuestionNumber(type));
              }
              acc.get(type)!.addQuestions([question]);
              return acc;
            }, new Map<number, QuestionNumber>())
            .values()
        );

        // 默认加载第一个问题
        if (questionsResult.items.length > 0) {
          this.onQuestionSelected(questionsResult.items[0].id);
        }
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/question-banks/' + this.questionBankId]);
  }

  onQuestionSelected(questionId: string): void {
    this.selectedQuestionId = questionId;
    this.questionService.get(questionId).subscribe(question => {
      this.selectedQuestion = question;
    });
  }
}
