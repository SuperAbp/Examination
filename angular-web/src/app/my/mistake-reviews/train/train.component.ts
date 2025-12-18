import { LocalizationService } from '@abp/ng.core';
import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MistakeReviewService, QuestionService, TrainingService } from '@proxy/controllers';
import { GetTrainsInput, TrainingListDto } from '@proxy/training-management';
import { QuestionNumber } from '@shared/components/question-number/question-number';
import { TrainComponent, TrainConfig } from '@shared/components/train';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-mistake-reviews-train',
  templateUrl: './train.component.html',
  styleUrls: ['./train.component.css'],
  standalone: true,
  imports: [TrainComponent],
})
export class MistakeReviewsTrainComponent implements OnInit {
  mode: number;
  type?: number;
  questionId?: string;
  questionContent?: string;
  trainConfig: TrainConfig;
  questionNumbers: QuestionNumber[] = [];
  trainingRecords: TrainingListDto[] = [];

  private readonly mistakeReviewService = inject(MistakeReviewService);
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
      trainingSource: 3,
      title: this.localizationService.instant('::Menu:MyMistakeReview'),
      mode: this.mode,
    };

    // 如果指定了单个题目，直接获取该题目详情
    if (this.questionId) {
      forkJoin([
        this.questionService.get(this.questionId),
        this.trainingService.getList({
          trainingSource: 3,
        } as GetTrainsInput),
      ]).subscribe(([questionDetail, trainingResult]) => {
        if (questionDetail) {
          const questionNumber = new QuestionNumber(questionDetail.questionType);
          questionNumber.addQuestionIds([this.questionId!]);
          this.questionNumbers = [questionNumber];
        }

        if (trainingResult && trainingResult.items) {
          this.trainingRecords = trainingResult.items;
        }
      });
    } else {
      // 构建获取错题列表的参数
      const getMistakesInput: any = {
        errorCount: 1,
        questionType: this.type,
        questionContent: this.questionContent,
        maxResultCount: 1000, // 获取所有错题
      };

      // 通过 MistakesReviewService 获取错题列表
      forkJoin([
        this.mistakeReviewService.getList(getMistakesInput),
        this.trainingService.getList({
          trainingSource: 3,
        } as GetTrainsInput),
      ]).subscribe(([mistakesResult, trainingResult]) => {
        if (mistakesResult && mistakesResult.items) {
          this.questionNumbers = Array.from(
            mistakesResult.items
              .reduce((acc, mistake) => {
                const type = mistake.questionType;
                if (!acc.has(type)) {
                  acc.set(type, new QuestionNumber(type));
                }
                acc.get(type)!.addQuestionIds([mistake.questionId]);
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
  }

  goBack(): void {
    this.router.navigate(['/my/mistakes-reviews']);
  }
}
