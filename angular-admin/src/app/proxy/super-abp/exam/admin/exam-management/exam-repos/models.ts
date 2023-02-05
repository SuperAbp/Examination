import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface ExamingRepoCreateDto extends ExamingRepoCreateOrUpdateDtoBase {
  examingId?: string;
  questionRepositoryId?: string;
}

export interface ExamingRepoCreateOrUpdateDtoBase {
  singleCount?: number;
  singleScore?: number;
  multiCount?: number;
  multiScore?: number;
  judgeCount?: number;
  judgeScore?: number;
}

export interface ExamingRepoListDto extends EntityDto<string> {
  questionRepositoryId?: string;
  questionRepository?: string;
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
  examingId?: string;
}
