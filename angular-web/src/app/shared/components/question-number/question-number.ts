import { QuestionListDto } from '@proxy/question-management/questions';

export class QuestionNumber {
  questionType: number;
  questions: QuestionListDto[] = [];

  constructor(questionType: number = 0) {
    this.questionType = questionType;
  }

  /**
   * 添加questions
   * @param questions 问题列表
   */
  addQuestions(questions: QuestionListDto[]): void {
    if (!questions || questions.length === 0) {
      return;
    }
    this.questions.push(...questions);
  }

  /**
   * 获取问题列表
   */
  getQuestions(): QuestionListDto[] {
    return this.questions;
  }

  /**
   * 获取问题总数
   */
  getTotalCount(): number {
    return this.questions.length;
  }

  /**
   * 清空所有数据
   */
  clear(): void {
    this.questions = [];
  }
}
