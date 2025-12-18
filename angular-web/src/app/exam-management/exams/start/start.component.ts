import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserExamService } from '@proxy/controllers';
import {
  UserExamDetailDto,
  UserExamDetailDto_SectionDto_QuestionDto,
  UserExamAnswerDto,
} from '@proxy/exam-management/user-exams';
import { CoreModule } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { QuestionNumberComponent } from '@shared/components/question-number/question-number.component';
import { QuestionNumber } from '@shared/components/question-number/question-number';
import { AnswerSubmission } from '@shared/components/choice-answer';
import { SingleQuestionComponent } from './single-question/single-question.component';
import { FullPaperComponent } from './full-paper/full-paper.component';
import { CountdownComponent, CountdownConfig, CountdownEvent } from 'ngx-countdown';
import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';

// 答题模式：1=逐题模式，0=整卷模式
const AnswerMode = {
  SingleQuestion: 1,
  FullPaper: 0,
};

@Component({
  selector: 'app-exams-start',
  templateUrl: './start.component.html',
  styleUrls: ['./start.component.scss'],
  standalone: true,
  imports: [
    CoreModule,
    CommonModule,
    QuestionNumberComponent,
    SingleQuestionComponent,
    FullPaperComponent,
    CountdownComponent,
  ],
})
export class ExamsStartComponent implements OnInit {
  userExamId?: string;
  userExam?: UserExamDetailDto;
  answerMode: number = AnswerMode.SingleQuestion;
  AnswerMode = AnswerMode;

  // 当前选中的题目
  selectedQuestion?: UserExamDetailDto_SectionDto_QuestionDto;
  selectedQuestionIndex: number = 0;

  // 所有题目（从sections展平）
  get allQuestions(): UserExamDetailDto_SectionDto_QuestionDto[] {
    return this.userExam?.sections.flatMap(s => s.questions) || [];
  }

  // 答案映射
  answerMap: Map<string, Set<string>> = new Map();

  // 题号组件数据
  questionNumbers: QuestionNumber[] = [];

  // 已答题目集合
  answeredQuestionIds: Set<string> = new Set();

  // 提交状态
  isSubmitting = false;

  // 空Set用于模板绑定
  readonly emptySet = new Set<string>();

  // 倒计时相关
  @ViewChild(CountdownComponent, { static: false })
  countdownComponent?: CountdownComponent;
  countdownConfig?: CountdownConfig;
  isTimeWarning: boolean = false; // 剩余时间小于5分钟时警告
  isTimeDanger: boolean = false; // 剩余时间小于1分钟时危险

  private readonly userExamService = inject(UserExamService);
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly confirmation = inject(ConfirmationService);
  private readonly toaster = inject(ToasterService);

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      this.userExamId = params['id'];
      this.loadUserExam();
    });
  }

  private loadUserExam(): void {
    if (!this.userExamId) return;

    // 直接使用userExamId获取考试详情
    this.userExamService.get(this.userExamId).subscribe({
      next: userExam => {
        this.initializeUserExam(userExam);
      },
      error: () => {
        // 获取失败，返回考试列表
        this.router.navigate(['/exam/exams']);
      },
    });
  }

  private initializeUserExam(userExam: UserExamDetailDto): void {
    this.userExam = userExam;
    this.answerMode = userExam.answerMode;

    // 尝试从localStorage恢复答案
    const savedAnswers = this.loadAnswersFromCache();

    // 构建题号组件数据
    this.questionNumbers = [];

    userExam.sections.forEach((section, sectionIndex) => {
      const questionIds: string[] = [];
      section.questions.forEach(question => {
        questionIds.push(question.id!);

        // 优先使用缓存的答案，否则使用后端返回的答案
        const cachedAnswers = savedAnswers.get(question.id!);
        if (cachedAnswers && cachedAnswers.size > 0) {
          this.answerMap.set(question.id!, cachedAnswers);
          this.answeredQuestionIds.add(question.id!);
        } else if (question.answers) {
          const answers = question.answers.split(',').filter(a => a.trim());
          this.answerMap.set(question.id!, new Set(answers));
          if (answers.length > 0) {
            this.answeredQuestionIds.add(question.id!);
          }
        }
      });

      const qn = new QuestionNumber(0, section.title, section.totalScore);
      qn.addQuestionIds(questionIds);
      this.questionNumbers.push(qn);
    });

    // 选中第一题
    if (this.allQuestions.length > 0) {
      this.selectQuestion(0);
    }

    // 启动倒计时
    this.startCountdown();
  }

  selectQuestion(index: number): void {
    if (index < 0 || index >= this.allQuestions.length) return;

    this.selectedQuestionIndex = index;
    this.selectedQuestion = this.allQuestions[index];
  }

  onQuestionNumberSelected(questionId: string): void {
    const index = this.allQuestions.findIndex(q => q.id === questionId);
    if (index !== -1) {
      // 更新选中的题目（两种模式都需要）
      this.selectQuestion(index);

      if (this.answerMode === AnswerMode.FullPaper) {
        // 整卷模式：额外滚动到对应题目
        const element = document.getElementById('question-' + questionId);
        if (element) {
          element.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
      }
    }
  }

  // 统一的答案更新方法
  private updateAnswer(questionId: string, answers: Set<string>): void {
    this.answerMap.set(questionId, answers);

    // 更新已答状态
    if (answers.size > 0) {
      this.answeredQuestionIds.add(questionId);
    } else {
      this.answeredQuestionIds.delete(questionId);
    }

    // 保存到本地缓存
    this.saveAnswersToCache();
  }

  // 单选题答题（逐题和整卷模式通用）
  onQuestionAnswered(event: { questionId: string; answers: Set<string> }): void {
    this.updateAnswer(event.questionId, event.answers);
  }

  // 多选题答题（逐题和整卷模式通用）
  onMultipleChoiceChanged(event: { questionId: string; answerId: string }): void {
    const currentAnswers = this.answerMap.get(event.questionId) || new Set<string>();

    // 切换答案
    if (currentAnswers.has(event.answerId)) {
      currentAnswers.delete(event.answerId);
    } else {
      currentAnswers.add(event.answerId);
    }

    this.updateAnswer(event.questionId, currentAnswers);
  }

  // 填空题答题（逐题和整卷模式通用）
  onFillBlankChanged(event: { questionId: string; answers: string[] }): void {
    const answerSet = new Set(event.answers.filter(a => a && a.trim()));
    this.updateAnswer(event.questionId, answerSet);
  }

  // 获取当前题目的已选答案
  getSelectedAnswers(): Set<string> {
    if (!this.selectedQuestion) return new Set();
    return this.answerMap.get(this.selectedQuestion.id!) || new Set();
  }

  // 获取localStorage的key
  private getCacheKey(): string {
    return `exam_answers_${this.userExamId}`;
  }

  // 保存答案到localStorage
  private saveAnswersToCache(): void {
    if (!this.userExamId) return;

    const answersObject: { [key: string]: string[] } = {};
    this.answerMap.forEach((answers, questionId) => {
      answersObject[questionId] = Array.from(answers);
    });

    try {
      localStorage.setItem(this.getCacheKey(), JSON.stringify(answersObject));
    } catch (error) {
      console.error('保存答案到缓存失败:', error);
    }
  }

  // 从localStorage加载答案
  private loadAnswersFromCache(): Map<string, Set<string>> {
    const result = new Map<string, Set<string>>();
    if (!this.userExamId) return result;

    try {
      const cached = localStorage.getItem(this.getCacheKey());
      if (cached) {
        const answersObject = JSON.parse(cached);
        Object.keys(answersObject).forEach(questionId => {
          result.set(questionId, new Set(answersObject[questionId]));
        });
      }
    } catch (error) {
      console.error('从缓存加载答案失败:', error);
    }

    return result;
  }

  // 清除localStorage中的答案
  private clearAnswersCache(): void {
    if (!this.userExamId) return;
    try {
      localStorage.removeItem(this.getCacheKey());
    } catch (error) {
      console.error('清除答案缓存失败:', error);
    }
  }

  submitExam(): void {
    if (this.isSubmitting) return;

    // 检查是否有未答题
    const unansweredCount = this.getUnansweredCount();
    let message: string;
    if (unansweredCount > 0) {
      message = `还有 ${unansweredCount} 道题未作答，确定要提交试卷吗？`;
    } else {
      message = '确定要提交试卷吗？提交后将无法修改答案。';
    }

    this.confirmation.warn(message, '提交试卷').subscribe(status => {
      if (status === 'confirm') {
        this.doSubmit();
      }
    });
  }

  private doSubmit(): void {
    // 收集所有答案
    const answers: UserExamAnswerDto[] = [];
    this.allQuestions.forEach(question => {
      const answerSet = this.answerMap.get(question.id!);
      if (answerSet && answerSet.size > 0) {
        answers.push({
          questionId: question.id!,
          answers: Array.from(answerSet).join(','),
        });
      }
    });

    this.isSubmitting = true;
    this.userExamService.finished(this.userExam!.id, answers).subscribe({
      next: () => {
        // 清除本地缓存
        this.clearAnswersCache();

        // 提交成功，跳转到提交成功页面
        this.router.navigate(['/exams/submitted', this.userExam!.id], {
          queryParams: { examName: this.userExam!.examName },
        });
      },
      error: err => {
        this.isSubmitting = false;
        this.toaster.error('提交失败，请重试。');
      },
    });
  }

  getAnsweredQuestionIds(): Set<string> {
    return this.answeredQuestionIds;
  }

  getUnansweredCount(): number {
    return this.allQuestions.length - this.answeredQuestionIds.size;
  }

  goBack(): void {
    this.router.navigate(['/exams']);
  }

  // 配置倒计时
  private startCountdown(): void {
    if (!this.userExam?.endTime) return;

    const now = new Date().getTime();
    const endTime = new Date(this.userExam.endTime).getTime();
    const leftTime = Math.max(0, Math.floor((endTime - now) / 1000));

    this.countdownConfig = {
      leftTime,
      format: this.getCountdownFormat(leftTime),
      notify: [300, 60], // 5分钟和1分钟时触发通知
    };
  }

  // 根据剩余时间选择合适的显示格式
  private getCountdownFormat(seconds: number): string {
    const days = Math.floor(seconds / 86400);
    const hours = Math.floor((seconds % 86400) / 3600);
    const minutes = Math.floor((seconds % 3600) / 60);

    if (days > 0) {
      return 'D天 HH小时';
    } else if (hours > 0) {
      return 'HH小时 mm分钟';
    } else if (minutes >= 5) {
      return 'mm分 ss秒';
    } else {
      return 'mm分 ss秒';
    }
  }

  // 倒计时事件处理
  onCountdownEvent(event: CountdownEvent): void {
    if (event.action === 'notify') {
      // 剩余时间警告
      if (event.left <= 60000) {
        // 小于1分钟
        this.isTimeDanger = true;
        this.isTimeWarning = false;
      } else if (event.left <= 300000) {
        // 小于5分钟
        this.isTimeWarning = true;
        this.isTimeDanger = false;
      }
    } else if (event.action === 'done') {
      // 倒计时结束，自动提交
      this.isTimeDanger = true;
      if (!this.isSubmitting) {
        this.submitExam();
      }
    }
  }
}
