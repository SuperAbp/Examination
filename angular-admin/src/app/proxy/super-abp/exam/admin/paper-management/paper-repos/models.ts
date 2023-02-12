import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetPaperRepoForEditorOutput extends PaperRepoCreateOrUpdateDtoBase {
}

export interface GetPaperReposInput extends PagedAndSortedResultRequestDto {
  examingId?: string;
}

export interface PaperRepoCreateDto extends PaperRepoCreateOrUpdateDtoBase {
  examingId?: string;
  questionRepositoryId?: string;
}

export interface PaperRepoCreateOrUpdateDtoBase {
  singleCount?: number;
  singleScore?: number;
  multiCount?: number;
  multiScore?: number;
  judgeCount?: number;
  judgeScore?: number;
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
}

export interface PaperRepoUpdateDto extends PaperRepoCreateOrUpdateDtoBase {
}
