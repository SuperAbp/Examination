import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface ExamCreateDto extends ExamCreateOrUpdateDtoBase {
}

export interface ExamCreateOrUpdateDtoBase {
  name?: string;
  description?: string;
  score: number;
  passingScore: number;
  totalTime: number;
  paperId?: string;
  startTime?: string;
  endTime?: string;
}

export interface ExamDetailDto extends EntityDto<string> {
  name?: string;
  score: number;
  passingScore: number;
  totalTime: number;
  startTime?: string;
  endTime?: string;
}

export interface ExamListDto extends EntityDto<string> {
  name?: string;
  score: number;
  passingScore: number;
  totalTime: number;
  startTime?: string;
  endTime?: string;
}

export interface ExamUpdateDto extends ExamCreateOrUpdateDtoBase {
}

export interface GetExamForEditorOutput extends ExamCreateOrUpdateDtoBase {
}

export interface GetExamsInput extends PagedAndSortedResultRequestDto {
  name?: string;
}
