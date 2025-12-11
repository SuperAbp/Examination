import { CoreModule } from '@abp/ng.core';
import { Component, inject, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionBankService, QuestionService } from '@proxy/controllers';
import { QuestionBankDetailDto } from '@proxy/question-management/question-banks';
import {
  GetQuestionsInput,
  QuestionAnswerDto,
  QuestionDetailDto,
} from '@proxy/question-management/questions';
import { QuestionNumber } from '@shared/components/question-number/question-number';
import { QuestionNumberComponent } from '@shared/components/question-number/question-number.component';
import { CharPipe } from '@shared/pipes/char/char.pipe';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-question-banks-train',
  templateUrl: './train.component.html',
  styleUrls: ['./train.component.scss'],
  imports: [CoreModule, QuestionNumberComponent, CharPipe],
})
export class QuestionBanksTrainComponent implements OnInit {
  mode: number;
  type?: number;
  questionBankId: string;

  questionBank: QuestionBankDetailDto;
  questionNumbers: QuestionNumber[] = [];
  selectedQuestion: QuestionDetailDto;
  selectedQuestionId: string;
  selectedAnswerId: string | null = null;
  showAnalysis = false;
  answerMap: Map<string, { answerId: string; right: boolean }> = new Map();
  allQuestionIds: string[] = [];
  favorites: Set<string> = new Set();

  private readonly questionBankService = inject(QuestionBankService);
  private readonly questionService = inject(QuestionService);
  private readonly router = inject(Router);
  private readonly activatedRoute = inject(ActivatedRoute);
  constructor() {
    this.activatedRoute.params.subscribe(params => {
      this.questionBankId = params['id'];
    });
    this.activatedRoute.queryParams.subscribe(queryParams => {
      this.mode = +queryParams['mode'];
      this.type = queryParams['type'] ? +queryParams['type'] : undefined;
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

        this.allQuestionIds = [];
        this.questionNumbers.forEach(qn => {
          qn.questions.forEach(q => {
            this.allQuestionIds.push(q.id);
          });
        });

        if (questionsResult.items.length > 0) {
          this.onQuestionNumberSelected(questionsResult.items[0].id);
        }
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/question-banks/' + this.questionBankId]);
  }

  onQuestionNumberSelected(questionId: string): void {
    this.selectedQuestionId = questionId;
    const savedState = this.answerMap.get(questionId);
    if (savedState) {
      this.selectedAnswerId = savedState.answerId;
      this.showAnalysis = true;
    } else {
      this.selectedAnswerId = null;
      this.showAnalysis = false;
    }
    this.questionService.get(questionId).subscribe(question => {
      this.selectedQuestion = question;
    });
    if (this.mode === 1) {
      this.showAnalysis = true;
    }
  }

  onOptionSelected(option: QuestionAnswerDto): void {
    this.selectedAnswerId = option.id;
    this.showAnalysis = true;
    this.answerMap.set(this.selectedQuestionId, {
      answerId: option.id,
      right: option.right,
    });
  }

  isOptionDisabled(): boolean {
    return this.showAnalysis;
  }
  isSelectedOption(option: QuestionAnswerDto): boolean {
    return this.selectedAnswerId === option.id;
  }

  prev(): void {
    const currentIndex = this.allQuestionIds.findIndex(id => id === this.selectedQuestionId);
    if (currentIndex > 0) {
      this.onQuestionNumberSelected(this.allQuestionIds[currentIndex - 1]);
    }
  }

  next(): void {
    const currentIndex = this.allQuestionIds.findIndex(id => id === this.selectedQuestionId);
    if (currentIndex < this.allQuestionIds.length - 1) {
      this.onQuestionNumberSelected(this.allQuestionIds[currentIndex + 1]);
    }
  }

  isPrevDisabled(): boolean {
    return (
      this.allQuestionIds.length === 0 ||
      this.allQuestionIds.findIndex(id => id === this.selectedQuestionId) === 0
    );
  }

  isNextDisabled(): boolean {
    return (
      this.allQuestionIds.length === 0 ||
      this.allQuestionIds.findIndex(id => id === this.selectedQuestionId) ===
        this.allQuestionIds.length - 1
    );
  }

  isFavorited(): boolean {
    return this.favorites.has(this.selectedQuestionId);
  }

  favorite(): void {
    if (this.isFavorited()) {
      this.favorites.delete(this.selectedQuestionId);
      // 调用取消收藏接口，传递 false
      this.onFavoriteChange(false);
    } else {
      this.favorites.add(this.selectedQuestionId);
      // 调用收藏接口，传递 true
      this.onFavoriteChange(true);
    }
  }

  private onFavoriteChange(isFavorited: boolean): void {
    // 这里你可以通过 isFavorited 值来调用不同的接口
    // true: 调用收藏接口
    // false: 调用取消收藏接口
    console.log(`Question ${this.selectedQuestionId} favorited: ${isFavorited}`);
  }
}
