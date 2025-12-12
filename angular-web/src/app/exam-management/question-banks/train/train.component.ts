import { CoreModule } from '@abp/ng.core';
import { Component, inject, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  FavoriteService,
  QuestionBankService,
  QuestionService,
  TrainingService,
} from '@proxy/controllers';
import { QuestionBankDetailDto } from '@proxy/question-management/question-banks';
import { GetQuestionsInput, QuestionDetailDto } from '@proxy/question-management/questions';
import { GetTrainsInput, TrainingCreateDto } from '@proxy/training-management';
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
  answerMap: Map<string, { answerIds: Set<string>; right: boolean }> = new Map();
  selectedAnswerMap: Map<string, Set<string>> = new Map();
  allQuestionIds: string[] = [];
  isFavorited = false;
  correctQuestionIds: Set<string> = new Set();
  incorrectQuestionIds: Set<string> = new Set();
  trainingRecordIds: Map<string, string> = new Map();

  private readonly questionBankService = inject(QuestionBankService);
  private readonly trainingService = inject(TrainingService);
  private readonly questionService = inject(QuestionService);
  private readonly favoriteService = inject(FavoriteService);
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
      this.trainingService.getList({
        questionBankId: this.questionBankId,
        trainingSource: 1,
      } as GetTrainsInput),
    ]).subscribe(([questionBank, questionsResult, trainingResult]) => {
      this.questionBank = questionBank;

      let lastTrainedQuestionId: string | null = null;

      if (trainingResult && trainingResult.items) {
        trainingResult.items.forEach(training => {
          this.trainingRecordIds.set(training.questionId, training.id);

          if (training.right) {
            this.correctQuestionIds.add(training.questionId);
          } else {
            this.incorrectQuestionIds.add(training.questionId);
          }

          lastTrainedQuestionId = training.questionId;
        });
      }

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

        // 优先加载最后训练的题目，如果没有则加载第一题
        if (lastTrainedQuestionId && this.allQuestionIds.includes(lastTrainedQuestionId)) {
          this.onQuestionNumberSelected(lastTrainedQuestionId);
        } else if (questionsResult.items.length > 0) {
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
    this.showAnalysis = !!savedState;

    this.questionService.get(questionId).subscribe(question => {
      this.selectedQuestion = question;
    });

    this.checkFavoriteState(questionId);

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
    this.answerMap.set(this.selectedQuestionId, {
      answerIds: submission.answerIds,
      right: submission.isCorrect,
    });

    // 更新题号状态 Set
    if (submission.isCorrect) {
      this.correctQuestionIds.add(this.selectedQuestionId);
      this.incorrectQuestionIds.delete(this.selectedQuestionId);
    } else {
      this.incorrectQuestionIds.add(this.selectedQuestionId);
      this.correctQuestionIds.delete(this.selectedQuestionId);
    }

    const trainingRecordId = this.trainingRecordIds.get(this.selectedQuestionId);

    if (trainingRecordId) {
      // 已经回答过，调用 setIsRight 更新结果
      this.trainingService.setIsRight(trainingRecordId, submission.isCorrect).subscribe();
    } else {
      // 首次回答，调用 create 创建记录
      const trainingDto: TrainingCreateDto = {
        questionBankId: this.questionBankId,
        questionId: this.selectedQuestionId,
        trainingSource: 1,
        right: submission.isCorrect,
      };

      this.trainingService.create(trainingDto).subscribe(result => {
        this.trainingRecordIds.set(this.selectedQuestionId, result.id);
      });
    }
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

  favorite(): void {
    if (this.isFavorited) {
      this.favoriteService.delete(this.selectedQuestionId).subscribe(() => {
        this.isFavorited = false;
      });
    } else {
      this.favoriteService.create(this.selectedQuestionId).subscribe(() => {
        this.isFavorited = true;
      });
    }
  }

  private checkFavoriteState(questionId: string): void {
    this.favoriteService.getByQuestionId(questionId).subscribe(isFavorited => {
      this.isFavorited = isFavorited;
    });
  }
}
