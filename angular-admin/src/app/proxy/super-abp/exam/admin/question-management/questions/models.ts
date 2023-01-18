import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { QuestionType } from '../../../question-management/questions/question-type.enum';

export interface GetQuestionForEditorOutput extends QuestionCreateOrUpdateDtoBase {
}

export interface GetQuestionsInput extends PagedAndSortedResultRequestDto {
  content?: string;
  questionType?: QuestionType;
  questionRepositoryIds: string[];
}

export interface QuestionCreateDto extends QuestionCreateOrUpdateDtoBase {
}

export interface QuestionCreateOrUpdateDtoBase {
  questionType: QuestionType;
  content?: string;
  analysis?: string;
  questionRepositoryId?: string;
}

export interface QuestionListDto extends EntityDto<string> {
  questionRepository?: string;
  questionType: QuestionType;
  content?: string;
  analysis?: string;
  creationTime?: string;
}

export interface QuestionUpdateDto extends QuestionCreateOrUpdateDtoBase {
}
