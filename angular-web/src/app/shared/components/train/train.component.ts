import { CoreModule } from '@abp/ng.core';
import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
  TemplateRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { FavoriteService, QuestionService, TrainingService } from '@proxy/controllers';
import { QuestionDetailDto } from '@proxy/question-management/questions';
import { TrainingCreateDto, TrainingListDto } from '@proxy/training-management';
import { QuestionNumber } from '@shared/components/question-number/question-number';
import { QuestionNumberComponent } from '@shared/components/question-number/question-number.component';
import { SingleChoiceComponent } from '@shared/components/single-choice/single-choice.component';
import { MultipleChoiceComponent } from '@shared/components/multiple-choice/multiple-choice.component';
import { FillBlankComponent } from '@shared/components/fill-blank/fill-blank.component';
import { AnswerSubmission } from '@shared/components/choice-answer';

export interface TrainConfig {
  // 标题（显示在卡片头部）
  title: string;
  // 模式：1=背题模式, 0=练题模式
  mode: number;
  // 训练来源：1=题库, 2=收藏夹
  trainingSource: number;
}

@Component({
  selector: 'app-train',
  templateUrl: './train.component.html',
  styleUrls: ['./train.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    CoreModule,
    QuestionNumberComponent,
    SingleChoiceComponent,
    MultipleChoiceComponent,
    FillBlankComponent,
  ],
})
export class TrainComponent implements OnChanges {
  @Input() config!: TrainConfig;
  @Input() questionNumbers: QuestionNumber[] = [];
  @Input() trainingRecords: TrainingListDto[] = [];
  @Output() backClick = new EventEmitter<void>();

  selectedQuestion: QuestionDetailDto;
  selectedQuestionId: string;
  showAnalysis = false;
  answerMap: Map<string, { answers: Set<string>; submitted: boolean; right?: boolean }> = new Map();
  allQuestionIds: string[] = [];
  isFavorited = false;
  correctQuestionIds: Set<string> = new Set();
  incorrectQuestionIds: Set<string> = new Set();
  trainingRecordIds: Map<string, string> = new Map();

  private readonly trainingService = inject(TrainingService);
  private readonly questionService = inject(QuestionService);
  private readonly favoriteService = inject(FavoriteService);
  private readonly offcanvasService = inject(NgbOffcanvas);

  openQuestionNumberOffcanvas(content: TemplateRef<any>) {
    this.offcanvasService.open(content, { 
      position: 'bottom',
      panelClass: 'question-number-offcanvas'
    });
  }

  getCurrentQuestionIndex(): number {
    return this.allQuestionIds.indexOf(this.selectedQuestionId) + 1;
  }

  getTotalQuestions(): number {
    return this.allQuestionIds.length;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['trainingRecords'] && this.trainingRecords) {
      this.initializeTrainingRecords();
    }

    if (changes['questionNumbers'] && this.questionNumbers) {
      this.initializeQuestions();
    }
  }

  private initializeTrainingRecords(): void {
    this.correctQuestionIds.clear();
    this.incorrectQuestionIds.clear();
    this.trainingRecordIds.clear();

    let lastTrainedQuestionId: string | null = null;

    this.trainingRecords.forEach(training => {
      this.trainingRecordIds.set(training.questionId, training.id);

      if (training.right) {
        this.correctQuestionIds.add(training.questionId);
      } else {
        this.incorrectQuestionIds.add(training.questionId);
      }

      lastTrainedQuestionId = training.questionId;
    });

    // 如果有题目且还没选中，自动选择第一题或最后答题
    if (this.allQuestionIds.length > 0 && !this.selectedQuestionId) {
      this.selectInitialQuestion(lastTrainedQuestionId);
    }
  }

  private initializeQuestions(): void {
    this.allQuestionIds = [];
    this.questionNumbers.forEach(qn => {
      this.allQuestionIds.push(...qn.questionIds);
    });

    // 如果有题目且还没选中，自动选择第一题
    if (this.allQuestionIds.length > 0 && !this.selectedQuestionId) {
      const lastTrainedQuestionId = this.getLastTrainedQuestionId();
      this.selectInitialQuestion(lastTrainedQuestionId);
    }
  }

  private getLastTrainedQuestionId(): string | null {
    if (this.trainingRecords.length === 0) return null;
    return this.trainingRecords[this.trainingRecords.length - 1]?.questionId || null;
  }

  private selectInitialQuestion(lastTrainedQuestionId: string | null): void {
    // mode为1(背题模式)时，从第一题开始；否则跳转到最新答题
    if (this.config.mode === 1) {
      if (this.allQuestionIds.length > 0) {
        this.onQuestionNumberSelected(this.allQuestionIds[0]);
      }
    } else {
      if (lastTrainedQuestionId && this.allQuestionIds.includes(lastTrainedQuestionId)) {
        this.onQuestionNumberSelected(lastTrainedQuestionId);
      } else if (this.allQuestionIds.length > 0) {
        this.onQuestionNumberSelected(this.allQuestionIds[0]);
      }
    }
  }

  goBack(): void {
    this.backClick.emit();
  }

  onQuestionNumberSelected(questionId: string): void {
    this.selectedQuestionId = questionId;

    const savedState = this.answerMap.get(questionId);
    this.showAnalysis = savedState?.submitted || false;

    this.questionService.get(questionId).subscribe(question => {
      this.selectedQuestion = question;
    });

    this.checkFavoriteState(questionId);

    if (this.config.mode === 1) {
      this.showAnalysis = true;
    }
  }

  onAnswerChanged(answerId: string): void {
    const savedState = this.answerMap.get(this.selectedQuestionId);
    const currentAnswers = savedState?.answers || new Set<string>();

    if (currentAnswers.has(answerId)) {
      currentAnswers.delete(answerId);
    } else {
      currentAnswers.add(answerId);
    }

    this.answerMap.set(this.selectedQuestionId, {
      answers: currentAnswers,
      submitted: false,
    });
  }

  onSubmitted(submission: AnswerSubmission): void {
    this.showAnalysis = true;
    this.answerMap.set(this.selectedQuestionId, {
      answers: submission.answers,
      submitted: true,
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
        questionBankId: this.selectedQuestion.questionBankId,
        questionId: this.selectedQuestionId,
        trainingSource: this.config.trainingSource,
        right: submission.isCorrect,
      };

      this.trainingService.create(trainingDto).subscribe(result => {
        this.trainingRecordIds.set(this.selectedQuestionId, result.id);
      });
    }
  }

  selectedMultipleChoiceAnswerIds(): Set<string> {
    const savedState = this.answerMap.get(this.selectedQuestionId);
    return savedState?.answers || new Set<string>();
  }

  selectedFillBlankAnswers(): string[] {
    const savedState = this.answerMap.get(this.selectedQuestionId);
    return savedState?.answers ? Array.from(savedState.answers) : [];
  }

  onFillBlankAnswerChanged(answers: string[]): void {
    // 暂存填空题答案，但不提交
    this.answerMap.set(this.selectedQuestionId, {
      answers: new Set(answers),
      submitted: false,
    });
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

  getCorrectQuestionIds(): Set<string> {
    return this.config.mode === 1 ? new Set() : this.correctQuestionIds;
  }

  getIncorrectQuestionIds(): Set<string> {
    return this.config.mode === 1 ? new Set() : this.incorrectQuestionIds;
  }
}
