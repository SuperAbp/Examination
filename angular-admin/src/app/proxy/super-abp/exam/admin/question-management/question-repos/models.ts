import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetQuestionRepoForEditorOutput extends QuestionRepoCreateOrUpdateDtoBase {
}

export interface GetQuestionReposInput extends PagedAndSortedResultRequestDto {
  title?: string;
}

export interface QuestionRepoCreateDto extends QuestionRepoCreateOrUpdateDtoBase {
}

export interface QuestionRepoCreateOrUpdateDtoBase {
  title?: string;
  remark?: string;
}

export interface QuestionRepoDetailDto extends EntityDto<string> {
  title?: string;
  remark?: string;
}

export interface QuestionRepoListDto extends EntityDto<string> {
  title?: string;
  remark?: string;
}

export interface QuestionRepoUpdateDto extends QuestionRepoCreateOrUpdateDtoBase {
}
