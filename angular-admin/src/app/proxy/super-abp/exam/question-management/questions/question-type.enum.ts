import { mapEnumToOptions } from '@abp/ng.core';

export enum QuestionType {
  SingleSelect = 0,
  MultiSelect = 1,
  Judge = 2,
  FillInTheBlanks = 3,
}

export const questionTypeOptions = mapEnumToOptions(QuestionType);
