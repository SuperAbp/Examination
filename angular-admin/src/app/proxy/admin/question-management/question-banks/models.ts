import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetQuestionBankForEditorOutput extends QuestionBankCreateOrUpdateDtoBase {
}

export interface GetQuestionBanksInput extends PagedAndSortedResultRequestDto {
  title?: string;
}

export interface GetQuestionCountInput {
  questionBankId?: string;
  questionType?: number;
  knowledgePointId?: string;
}

export interface QuestionBankCreateDto extends QuestionBankCreateOrUpdateDtoBase {
}

export interface QuestionBankCreateOrUpdateDtoBase {
  title: string;
  remark?: string;
}

export interface QuestionBankDetailDto extends EntityDto<string> {
  title?: string;
  remark?: string;
}

export interface QuestionBankListDto extends EntityDto<string> {
  title?: string;
  singleCount: number;
  judgeCount: number;
  multiCount: number;
  blankCount: number;
}

export interface QuestionBankUpdateDto extends QuestionBankCreateOrUpdateDtoBase {
}
