import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface ExamDetailDto extends EntityDto<string> {
  name: string;
  description?: string;
  score: number;
  passingScore: number;
  totalTime: number;
  paperId?: string;
  maxNumberOfTimesExceeded: boolean;
  startTime?: string;
  endTime?: string;
}

export interface ExamListDto extends EntityDto<string> {
  name: string;
  score: number;
  passingScore: number;
  totalTime: number;
  status: number;
  startTime?: string;
  endTime?: string;
}

export interface ExamRankingDto {
  userExamId?: string;
  userId?: string;
  userName?: string;
  totalScore: number;
  isPassed?: boolean;
  finishedTime?: string;
  rank: number;
}

export interface GetExamsInput extends PagedAndSortedResultRequestDto {
  name?: string;
  status?: number;
}
