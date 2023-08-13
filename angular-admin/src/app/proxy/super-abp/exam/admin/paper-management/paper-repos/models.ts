import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetPaperRepoForEditorOutput extends PaperRepoCreateOrUpdateDtoBase {}

export interface GetPaperReposInput extends PagedAndSortedResultRequestDto {
  paperId?: string;
}

export interface PaperRepoCreateDto extends PaperRepoCreateOrUpdateDtoBase {
  paperId?: string;
  questionRepositoryId?: string;
}

export interface PaperRepoCreateOrUpdateDtoBase {
  singleCount?: number;
  singleScore?: number;
  multiCount?: number;
  multiScore?: number;
  judgeCount?: number;
  judgeScore?: number;
  blankCount?: number;
  blankScore?: number;
}

export interface PaperRepoListDto extends EntityDto<string> {
  questionRepository?: string;
  questionRepositoryId?: string;
  singleCount?: number;
  singleScore?: number;
  multiCount?: number;
  multiScore?: number;
  judgeCount?: number;
  judgeScore?: number;
  blankCount?: number;
  blankScore?: number;
}

export interface PaperRepoUpdateDto extends PaperRepoCreateOrUpdateDtoBase {}
