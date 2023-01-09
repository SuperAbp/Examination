import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetQuestionAnswerForEditorOutput extends QuestionAnswerCreateOrUpdateDtoBase {}

export interface GetQuestionAnswersInput extends PagedAndSortedResultRequestDto {
  questionId?: string;
}

export interface QuestionAnswerCreateDto extends QuestionAnswerCreateOrUpdateDtoBase {}

export interface QuestionAnswerCreateOrUpdateDtoBase {
  right: boolean;
  sort: number;
  content?: string;
  analysis?: string;
  questionId?: string;
}

export interface QuestionAnswerListDto extends EntityDto<string> {
  right: boolean;
  sort: number;
  content?: string;
  analysis?: string;
}

export interface QuestionAnswerUpdateDto extends QuestionAnswerCreateOrUpdateDtoBase {}
