import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetUserExamWithUsersInput extends PagedAndSortedResultRequestDto {
  examId?: string;
}

export interface GetUserExamsInput extends PagedAndSortedResultRequestDto {
  examId?: string;
  userId?: string;
}

export interface ReviewedQuestionDto {
  questionId?: string;
  right: boolean;
  score?: number;
  reason?: string;
}

export interface UserExamDetailDto {
  userId?: string;
  examId?: string;
  examName?: string;
  userName?: string;
  status: number;
  questions: UserExamDetailDto_QuestionDto[];
}

export interface UserExamDetailDto_QuestionDto {
  id?: string;
  content?: string;
  questionType: number;
  analysis?: string;
  answers?: string;
  right?: boolean;
  score?: number;
  questionScore?: number;
  reason?: string;
  knowledgePoints: string[];
  options: UserExamDetailDto_QuestionDto_OptionDto[];
}

export interface UserExamDetailDto_QuestionDto_OptionDto {
  id?: string;
  content?: string;
  right?: boolean;
}

export interface UserExamListDto {
  id?: string;
  totalScore: number;
  finishedTime?: string;
  creationTime?: string;
  status: number;
}

export interface UserExamWithUserDto {
  userId?: string;
  user?: string;
  totalCount: number;
  maxScore: number;
}
