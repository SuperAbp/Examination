import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetMistakesReviewInput extends PagedAndSortedResultRequestDto {
  questionType?: number;
  questionContent?: string;
  errorCount: number;
}

export interface MistakesReviewListDto extends AuditedEntityDto<string> {
  questionId?: string;
  questionContent: string;
  errorCount: number;
  questionType: number;
}
