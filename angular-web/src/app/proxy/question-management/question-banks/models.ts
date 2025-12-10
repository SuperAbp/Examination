import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetQuestionBanksInput extends PagedAndSortedResultRequestDto {
  title?: string;
}

export interface QuestionBankDetailDto {
  title?: string;
  remark?: string;
}

export interface QuestionBankListDto extends FullAuditedEntityDto<string> {
  title?: string;
}
