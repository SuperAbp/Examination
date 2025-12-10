import type { EntityDto } from '@abp/ng.core';

export interface GetTrainsInput {
  questionBankId?: string;
  trainingSource?: number;
}

export interface TrainingCreateDto {
  questionBankId?: string;
  questionId?: string;
  trainingSource: number;
  right: boolean;
}

export interface TrainingListDto extends EntityDto<string> {
  questionId?: string;
  right: boolean;
}
