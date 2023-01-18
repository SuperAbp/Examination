import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface ExamingCreateDto extends ExamingCreateOrUpdateDtoBase {
}

export interface ExamingCreateOrUpdateDtoBase {
  name?: string;
  description?: string;
  score: number;
  passingScore: number;
  totalTime: number;
  startTime?: string;
  endTime?: string;
}

export interface ExamingDetailDto extends EntityDto<string> {
  name?: string;
  score: number;
  passingScore: number;
  totalTime: number;
  startTime?: string;
  endTime?: string;
}

export interface ExamingListDto extends EntityDto<string> {
  name?: string;
  score: number;
  passingScore: number;
  totalTime: number;
  startTime?: string;
  endTime?: string;
}

export interface ExamingUpdateDto extends ExamingCreateOrUpdateDtoBase {
}

export interface GetExamingForEditorOutput extends ExamingCreateOrUpdateDtoBase {
}

export interface GetExamingsInput extends PagedAndSortedResultRequestDto {
}
