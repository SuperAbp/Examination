/**
 * 考试状态
 * Examination Status
 */
export enum ExaminationStatus {
  /** 草稿 */
  Draft = 0,
  /** 已发布 */
  Published = 1,
  /** 评分中 */
  Grading = 2,
  /** 已完成 */
  Completed = 3,
  /** 已取消 */
  Cancelled = 4
}

/**
 * 用户考试状态
 * UserExam Status
 */
export enum UserExamStatus {
  /** 等待中 */
  Waiting = 0,
  /** 进行中 */
  InProgress = 1,
  /** 已提交 */
  Submitted = 2,
  /** 已出分 */
  Scored = 3,
  /** 超时 */
  Timeout = 98,
  /** 无效 */
  Invalidated = 99
}

/**
 * 答题模式
 * Answer Mode
 */
export enum AnswerMode {
  /** 顺序答题 */
  Sequential = 0,
  /** 自由答题 */
  Free = 1
}

/**
 * 审核模式
 * Review Mode
 */
export enum ReviewMode {
  /** 统一审核 */
  Unified = 0,
  /** 实时审核 */
  RealTime = 1
}
