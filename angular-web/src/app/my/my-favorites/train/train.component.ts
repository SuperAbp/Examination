import { LocalizationService } from '@abp/ng.core';
import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionService, TrainingService } from '@proxy/controllers';
import { GetQuestionsInput } from '@proxy/question-management/questions';
import { GetTrainsInput, TrainingListDto } from '@proxy/training-management';
import { QuestionNumber } from '@shared/components/question-number/question-number';
import { TrainComponent, TrainConfig } from '@shared/components/train';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-my-favorites-train',
  templateUrl: './train.component.html',
  styleUrls: ['./train.component.css'],
  standalone: true,
  imports: [TrainComponent],
})
export class MyFavoritesTrainComponent implements OnInit {
  mode: number;
  type?: number;
  questionId?: string;
  questionContent?: string;
  trainConfig: TrainConfig;
  questionNumbers: QuestionNumber[] = [];
  trainingRecords: TrainingListDto[] = [];

  private readonly questionService = inject(QuestionService);
  private readonly trainingService = inject(TrainingService);
  private readonly router = inject(Router);
  private readonly activatedRoute = inject(ActivatedRoute);
  private localizationService = inject(LocalizationService);

  constructor() {
    this.activatedRoute.queryParams.subscribe(queryParams => {
      this.mode = +queryParams['mode'];
      this.type = queryParams['type'] ? +queryParams['type'] : undefined;
      this.questionId = queryParams['questionId'];
      this.questionContent = queryParams['questionContent'];
    });
  }

  ngOnInit() {
    this.trainConfig = {
      trainingSource: 2,
      title: this.localizationService.instant('::MyFavorite'),
      mode: this.mode,
    };

    const getQuestionsInput: GetQuestionsInput = {
      isFavorite: true,
      questionType: this.type,
    };

    if (this.questionId) {
      getQuestionsInput.questionId = this.questionId;
    }

    if (this.questionContent) {
      getQuestionsInput.content = this.questionContent;
    }

    forkJoin([
      this.questionService.getList(getQuestionsInput),
      this.trainingService.getList({
        trainingSource: 2,
      } as GetTrainsInput),
    ]).subscribe(([questionsResult, trainingResult]) => {
      if (questionsResult && questionsResult.items) {
        this.questionNumbers = Array.from(
          questionsResult.items
            .reduce((acc, question) => {
              const type = question.questionType;
              if (!acc.has(type)) {
                acc.set(type, new QuestionNumber(type));
              }
              acc.get(type)!.addQuestionIds([question.id]);
              return acc;
            }, new Map<number, QuestionNumber>())
            .values(),
        );
      }

      if (trainingResult && trainingResult.items) {
        this.trainingRecords = trainingResult.items;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/my/favorites']);
  }
}
