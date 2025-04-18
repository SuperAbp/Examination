import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetQuestionForEditorOutput extends QuestionCreateOrUpdateDtoBase {
  questionType: number;
}

export interface GetQuestionsInput extends PagedAndSortedResultRequestDto {
  content?: string;
  questionType?: number;
  questionRepositoryIds: string[];
}

export interface QuestionCreateDto extends QuestionCreateOrUpdateDtoBase {
  questionType: number;
  options: QuestionCreateOrUpdateAnswerDto[];
}

export interface QuestionCreateOrUpdateAnswerDto {
  id?: string;
  right: boolean;
  content?: string;
  analysis?: string;
  sort: number;
}

export interface QuestionCreateOrUpdateDtoBase {
  content?: string;
  analysis?: string;
  questionRepositoryId?: string;
}

export interface QuestionImportDto {
  questionRepositoryId?: string;
  questionType: number;
  content?: string;
}

export interface QuestionListDto extends EntityDto<string> {
  questionRepository?: string;
  questionType: number;
  content?: string;
  analysis?: string;
  creationTime?: string;
}

export interface QuestionUpdateDto extends QuestionCreateOrUpdateDtoBase {
  options: QuestionCreateOrUpdateAnswerDto[];
}
