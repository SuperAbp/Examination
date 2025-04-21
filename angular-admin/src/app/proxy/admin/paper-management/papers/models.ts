import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetPaperForEditorOutput extends PaperCreateOrUpdateDtoBase {
}

export interface GetPapersInput extends PagedAndSortedResultRequestDto {
  name?: string;
}

export interface PaperCreateDto extends PaperCreateOrUpdateDtoBase {
}

export interface PaperCreateOrUpdateDtoBase {
  paperQuestionRules: PaperCreateOrUpdatePaperQuestionRuleDto[];
  name?: string;
  description?: string;
  score: number;
}

export interface PaperCreateOrUpdatePaperQuestionRuleDto {
  id?: string;
  questionBankId?: string;
  singleCount?: number;
  singleScore?: number;
  multiCount?: number;
  multiScore?: number;
  judgeCount?: number;
  judgeScore?: number;
  blankCount?: number;
  blankScore?: number;
}

export interface PaperListDto extends EntityDto<string> {
  name?: string;
  score: number;
}

export interface PaperUpdateDto extends PaperCreateOrUpdateDtoBase {
}
