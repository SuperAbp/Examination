import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetUserExamWithUsersInput extends PagedAndSortedResultRequestDto {
  examId?: string;
}

export interface GetUserExamsInput extends PagedAndSortedResultRequestDto {
  examId?: string;
  userId?: string;
}

export interface UserExamDetailDto {
  userId?: string;
  examId?: string;
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
  knowledgePoints: string[];
  options: UserExamDetailDto_QuestionDto_OptionDto[];
}

export interface UserExamDetailDto_QuestionDto_OptionDto {
  id?: string;
  content?: string;
  right?: boolean;
}

export interface UserExamListDto {
  totalScore: number;
  finished: boolean;
  finishedTime?: string;
  creationTime?: string;
}

export interface UserExamWithUserDto extends EntityDto<string> {
  user?: string;
  totalCount: number;
  maxScore: number;
}
