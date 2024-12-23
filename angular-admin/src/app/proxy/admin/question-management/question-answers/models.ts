import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetQuestionAnswerForEditorOutput extends QuestionAnswerCreateOrUpdateDtoBase {
}

export interface GetQuestionAnswersInput extends PagedAndSortedResultRequestDto {
  questionId?: string;
}

export interface QuestionAnswerCreateDto extends QuestionAnswerCreateOrUpdateDtoBase {
}

export interface QuestionAnswerCreateOrUpdateDtoBase {
  right: boolean;
  content?: string;
  analysis?: string;
  sort: number;
  questionId?: string;
}

export interface QuestionAnswerListDto extends EntityDto<string> {
  right: boolean;
  content?: string;
  analysis?: string;
  sort: number;
}

export interface QuestionAnswerUpdateDto extends QuestionAnswerCreateOrUpdateDtoBase {
}
