import type { EntityDto } from '@abp/ng.core';

export interface GetQuestionsInput {
  content?: string;
  questionType?: number;
  isFavorite: boolean;
  questionId?: string;
  questionBankId?: string;
}

export interface QuestionDetailDto extends EntityDto<string> {
  questionType: number;
  questionBankId?: string;
  content?: string;
  analysis?: string;
  knowledgePoints: string[];
  options: QuestionOptionDto[];
}

export interface QuestionListDto extends EntityDto<string> {
  questionType: number;
}

export interface QuestionOptionDto extends EntityDto<string> {
  right: boolean;
  content: string;
  analysis?: string;
  sort: number;
}
