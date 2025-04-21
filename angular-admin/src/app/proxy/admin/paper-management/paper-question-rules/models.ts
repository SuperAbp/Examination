import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetPaperQuestionRuleForEditorOutput extends PaperQuestionRuleCreateOrUpdateDtoBase {
}

export interface GetPaperQuestionRulesInput extends PagedAndSortedResultRequestDto {
  paperId?: string;
}

export interface PaperQuestionRuleCreateOrUpdateDtoBase {
  singleCount?: number;
  singleScore?: number;
  multiCount?: number;
  multiScore?: number;
  judgeCount?: number;
  judgeScore?: number;
  blankCount?: number;
  blankScore?: number;
}

export interface PaperQuestionRuleListDto extends EntityDto<string> {
  questionBank?: string;
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
