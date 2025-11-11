import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetPaperForEditorOutput extends PaperCreateOrUpdateDtoBase {
}

export interface GetPapersInput extends PagedAndSortedResultRequestDto {
  name?: string;
}

export interface PaperCreateDto extends PaperCreateOrUpdateDtoBase {
}

export interface PaperCreateOrUpdateDtoBase {
  name?: string;
  description?: string;
  score: number;
  paperType: number;
  sections: PaperCreateOrUpdateDtoBase_PaperSectionDto[];
}

export interface PaperCreateOrUpdateDtoBase_PaperSectionDto {
  id?: string;
  title?: string;
  scoreEach: number;
  totalScore: number;
  order: number;
  totalCount: number;
  remark?: string;
  paperQuestionRules: PaperCreateOrUpdateDtoBase_PaperSectionDto_PaperQuestionRuleDto[];
  paperQuestions: PaperCreateOrUpdateDtoBase_PaperSectionDto_PaperQuestionDto[];
}

export interface PaperCreateOrUpdateDtoBase_PaperSectionDto_PaperQuestionDto {
  questionId?: string;
  score: number;
  order: number;
}

export interface PaperCreateOrUpdateDtoBase_PaperSectionDto_PaperQuestionRuleDto {
  id?: string;
  questionBankId?: string;
  questionType: number;
  count: number;
  score: number;
}

export interface PaperListDto extends EntityDto<string> {
  name?: string;
  score: number;
  paperType: number;
  manualReview: boolean;
}

export interface PaperUpdateDto extends PaperCreateOrUpdateDtoBase {
}
