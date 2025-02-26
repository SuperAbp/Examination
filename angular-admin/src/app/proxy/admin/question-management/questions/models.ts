import type { QuestionType } from '../../../question-management/questions/question-type.enum';
import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetQuestionForEditorOutput extends QuestionCreateOrUpdateDtoBase {
  questionType: QuestionType;
}

export interface GetQuestionsInput extends PagedAndSortedResultRequestDto {
  content?: string;
  questionType?: QuestionType;
  questionRepositoryIds: string[];
}

export interface QuestionCreateDto extends QuestionCreateOrUpdateDtoBase {
  questionType: QuestionType;
  options: QuestionCreateOrUpdateAnswerDto[];
}

export interface QuestionCreateOrUpdateAnswerDto {
  id?: string;
  right: boolean;
  content?: string;
  analysis?: string;
  sort: number;
}

export interface QuestionCreateOrUpdateDtoBase {
  content?: string;
  analysis?: string;
  questionRepositoryId?: string;
}

export interface QuestionImportDto {
  questionRepositoryId?: string;
  questionType: QuestionType;
  content?: string;
}

export interface QuestionListDto extends EntityDto<string> {
  questionRepository?: string;
  questionType: QuestionType;
  content?: string;
  analysis?: string;
  creationTime?: string;
}

export interface QuestionUpdateDto extends QuestionCreateOrUpdateDtoBase {
  options: QuestionCreateOrUpdateAnswerDto[];
}
