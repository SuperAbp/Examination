import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetQuestionForEditorOutput extends QuestionCreateOrUpdateDtoBase {
  questionType: number;
  answers: QuestionOptionDto[];
}

export interface GetQuestionWithDetailInput {
  questionBankId?: string;
  questionType?: number;
  includeIds: string[];
  excludeIds: string[];
  count?: number;
}

export interface GetQuestionsInput extends PagedAndSortedResultRequestDto {
  content?: string;
  questionType?: number;
  questionBankIds: string[];
  excludeIds: string[];
}

export interface QuestionCreateDto extends QuestionCreateOrUpdateDtoBase {
  questionType: number;
  options: QuestionCreateOrUpdateAnswerDto[];
}

export interface QuestionCreateOrUpdateAnswerDto {
  id?: string;
  right: boolean;
  content: string;
  analysis?: string;
  sort: number;
}

export interface QuestionCreateOrUpdateDtoBase {
  content: string;
  analysis?: string;
  questionBankId?: string;
  knowledgePointIds: string[];
  fixedOrder: boolean;
}

export interface QuestionDetailDto extends EntityDto<string> {
  content: string;
  analysis?: string;
  questionType: number;
  questionBankId?: string;
  options: QuestionOptionDto[];
}

export interface QuestionImportDto {
  questionBankId?: string;
  questionType: number;
  content?: string;
}

export interface QuestionListDto extends EntityDto<string> {
  questionBank: string;
  knowledgePoints: string[];
  questionType: number;
  content: string;
  analysis?: string;
  creationTime?: string;
}

export interface QuestionOptionDto extends EntityDto<string> {
  right: boolean;
  content: string;
  analysis?: string;
  sort: number;
}

export interface QuestionUpdateDto extends QuestionCreateOrUpdateDtoBase {
  options: QuestionCreateOrUpdateAnswerDto[];
}
