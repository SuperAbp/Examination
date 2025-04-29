import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetQuestionBankForEditorOutput extends QuestionBankCreateOrUpdateDtoBase {
}

export interface GetQuestionBanksInput extends PagedAndSortedResultRequestDto {
  title?: string;
}

export interface QuestionBankCountDto {
  singleCount: number;
  multiCount: number;
  judgeCount: number;
  blankCount: number;
}

export interface QuestionBankCreateDto extends QuestionBankCreateOrUpdateDtoBase {
}

export interface QuestionBankCreateOrUpdateDtoBase {
  title?: string;
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
