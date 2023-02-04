import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface ExamingRepoCreateDto extends ExamingRepoCreateOrUpdateDtoBase {}

export interface ExamingRepoCreateOrUpdateDtoBase {
  examingId?: string;
  questionRepositoryId?: string;
  singleCount?: number;
  singleScore?: number;
  multiCount?: number;
  multiScore?: number;
  judgeCount?: number;
  judgeScore?: number;
}

export interface ExamingRepoListDto extends EntityDto<string> {
  singleCount?: number;
  singleScore?: number;
  multiCount?: number;
  multiScore?: number;
  judgeCount?: number;
  judgeScore?: number;
}

export interface ExamingRepoUpdateDto extends ExamingRepoCreateOrUpdateDtoBase {}

export interface GetExamingRepoForEditorOutput extends ExamingRepoCreateOrUpdateDtoBase {}

export interface GetExamingReposInput extends PagedAndSortedResultRequestDto {
  examingId: string;
}
