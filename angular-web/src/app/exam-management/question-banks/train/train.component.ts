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
import { SingleChoiceComponent } from '@shared/components/single-choice/single-choice.component';
import { MultipleChoiceComponent } from '@shared/components/multiple-choice/multiple-choice.component';
import { AnswerSubmission } from '@shared/components/choice-answer';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-question-banks-train',
  templateUrl: './train.component.html',
  styleUrls: ['./train.component.scss'],
  imports: [CoreModule, QuestionNumberComponent, SingleChoiceComponent, MultipleChoiceComponent],
})
export class QuestionBanksTrainComponent implements OnInit {
  mode: number;
  type?: number;
  questionBankId: string;

  questionBank: QuestionBankDetailDto;
  questionNumbers: QuestionNumber[] = [];
  selectedQuestion: QuestionDetailDto;
  selectedQuestionId: string;
  showAnalysis = false;
  submittedAnswerMap: Map<string, { answerIds: Set<string>; right: boolean }> = new Map();
  selectedAnswerMap: Map<string, Set<string>> = new Map();
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
            .values(),
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
    const savedState = this.submittedAnswerMap.get(questionId);

    if (savedState) {
      this.showAnalysis = true;
    } else {
      this.showAnalysis = false;
    }

    this.questionService.get(questionId).subscribe(question => {
      this.selectedQuestion = question;
    });

    if (this.mode === 1) {
      this.showAnalysis = true;
    }
  }

  onAnswerChanged(answerId: string): void {
    const currentAnswers = this.selectedAnswerMap.get(this.selectedQuestionId) || new Set<string>();
    if (currentAnswers.has(answerId)) {
      currentAnswers.delete(answerId);
    } else {
      currentAnswers.add(answerId);
    }
    this.selectedAnswerMap.set(this.selectedQuestionId, currentAnswers);
  }

  onSubmitted(submission: AnswerSubmission): void {
    this.showAnalysis = true;
    this.submittedAnswerMap.set(this.selectedQuestionId, {
      answerIds: submission.answerIds,
      right: submission.isCorrect,
    });
  }

  selectedMultipleChoiceAnswerIds(): Set<string> {
    return this.selectedAnswerMap.get(this.selectedQuestionId) || new Set<string>();
  }
  isOptionDisabled(): boolean {
    return this.showAnalysis;
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
