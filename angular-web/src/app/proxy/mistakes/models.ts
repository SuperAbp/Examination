import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetMistakesInput extends PagedAndSortedResultRequestDto {
  questionType?: number;
  questionContent?: string;
}

export interface MistakeListDto extends AuditedEntityDto<string> {
  questionId?: string;
  questionContent: string;
  errorCount: number;
  questionType: number;
}
