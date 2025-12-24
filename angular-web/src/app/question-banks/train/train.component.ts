import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionBankService, QuestionService, TrainingService } from '@proxy/controllers';
import { QuestionBankDetailDto } from '@proxy/question-management/question-banks';
import { GetQuestionsInput } from '@proxy/question-management/questions';
import { GetTrainsInput, TrainingListDto } from '@proxy/training-management';
import { QuestionNumber } from '@shared/components/question-number/question-number';
import { TrainComponent, TrainConfig } from '@shared/components/train';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-question-banks-train',
  templateUrl: './train.component.html',
  styleUrls: ['./train.component.scss'],
  standalone: true,
  imports: [TrainComponent],
})
export class QuestionBanksTrainComponent implements OnInit {
  mode: number;
  type?: number;
  questionBankId: string;
  questionBank: QuestionBankDetailDto;
  trainConfig: TrainConfig;
  questionIds: string[] = [];
  trainingRecords: TrainingListDto[] = [];

  private readonly questionBankService = inject(QuestionBankService);
  private readonly questionService = inject(QuestionService);
  private readonly trainingService = inject(TrainingService);
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
        isFavorite: false,
      } as GetQuestionsInput),
      this.trainingService.getList({
        questionBankId: this.questionBankId,
        trainingSource: 1,
      } as GetTrainsInput),
    ]).subscribe(([questionBank, questionsResult, trainingResult]) => {
      this.questionBank = questionBank;

      this.trainConfig = {
        trainingSource: 1,
        title: questionBank.title,
        mode: this.mode,
      };

      if (questionsResult && questionsResult.items) {
        this.questionIds = questionsResult.items.map(q => q.id);
      }

      if (trainingResult && trainingResult.items) {
        this.trainingRecords = trainingResult.items;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/question-banks/' + this.questionBankId]);
  }
}
