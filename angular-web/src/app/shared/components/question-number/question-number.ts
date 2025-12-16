export class QuestionNumber {
  questionType: number;
  questionIds: string[] = [];

  constructor(questionType: number = 0) {
    this.questionType = questionType;
  }

  /**
   * 添加问题ID
   * @param questionIds 问题ID列表
   */
  addQuestionIds(questionIds: string[]): void {
    if (!questionIds || questionIds.length === 0) {
      return;
    }
    this.questionIds.push(...questionIds);
  }

  /**
   * 获取问题ID列表
   */
  getQuestionIds(): string[] {
    return this.questionIds;
  }

  /**
   * 获取问题总数
   */
  getTotalCount(): number {
    return this.questionIds.length;
  }

  /**
   * 清空所有数据
   */
  clear(): void {
    this.questionIds = [];
  }
}
