import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetQuestionCategoriesInput extends PagedAndSortedResultRequestDto {
  parentId?: string;
  name?: string;
}

export interface GetQuestionCategoryForEditorOutput {
  parentId?: string;
  name?: string;
}

export interface QuestionCategoryCreateDto extends GetQuestionCategoryForEditorOutput {
}

export interface QuestionCategoryListDto {
  id?: string;
  parentName?: string;
  name?: string;
}

export interface QuestionCategoryUpdateDto extends GetQuestionCategoryForEditorOutput {
}
