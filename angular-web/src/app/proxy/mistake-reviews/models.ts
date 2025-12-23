import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetMistakeReviewsInput extends PagedAndSortedResultRequestDto {
  questionType?: number;
  questionContent?: string;
}

export interface MistakeReviewListDto extends AuditedEntityDto<string> {
  questionId?: string;
  questionContent: string;
  errorCount: number;
  questionType: number;
}
