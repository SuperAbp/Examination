import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface ExamCreateDto extends ExamCreateOrUpdateDtoBase {
}

export interface ExamCreateOrUpdateDtoBase {
  name: string;
  description?: string;
  score: number;
  passingScore: number;
  totalTime: number;
  paperId?: string;
  startTime?: string;
  endTime?: string;
  published: boolean;
  randomOrderOfOption: boolean;
  maxNumberOfTimes: number;
  answerMode: number;
  reviewMode: number;
}

export interface ExamDetailDto extends EntityDto<string> {
  name?: string;
  score: number;
  passingScore: number;
  totalTime: number;
  startTime?: string;
  endTime?: string;
  answerMode: number;
  randomOrderOfOption: boolean;
}

export interface ExamListDto extends EntityDto<string> {
  name: string;
  score: number;
  passingScore: number;
  totalTime: number;
  startTime?: string;
  endTime?: string;
  status: number;
  randomOrderOfOption: boolean;
  answerMode: number;
  reviewMode: number;
}

export interface ExamUpdateDto extends ExamCreateOrUpdateDtoBase {
}

export interface ExamUserExamDto {
  userId?: string;
  rank: number;
  user: string;
  totalCount: number;
  isPassed?: boolean;
  maxScore: number;
}

export interface GetExamForEditorOutput extends ExamCreateOrUpdateDtoBase {
  status: number;
}

export interface GetExamsInput extends PagedAndSortedResultRequestDto {
  name?: string;
  status?: number;
}
