import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetUserExamsInput extends PagedAndSortedResultRequestDto {
  examId?: string;
}

export interface UserExamAnswerDto {
  questionId?: string;
  answers: string;
}

export interface UserExamCreateDto {
  examId?: string;
}

export interface UserExamDetailDto extends EntityDto<string> {
  totalScore: number;
  isPassed?: boolean;
  userId?: string;
  examId?: string;
  examName: string;
  status: number;
  endTime?: string;
  answerMode: number;
  sections: UserExamDetailDto_SectionDto[];
}

export interface UserExamDetailDto_SectionDto {
  id?: string;
  title?: string;
  scoreEach: number;
  totalScore: number;
  order: number;
  totalCount: number;
  questions: UserExamDetailDto_SectionDto_QuestionDto[];
}

export interface UserExamDetailDto_SectionDto_QuestionDto {
  id?: string;
  content: string;
  questionType: number;
  analysis?: string;
  answers?: string;
  right?: boolean;
  score?: number;
  questionScore?: number;
  knowledgePoints: string[];
  options: UserExamDetailDto_SectionDto_QuestionDto_OptionDto[];
}

export interface UserExamDetailDto_SectionDto_QuestionDto_OptionDto {
  id?: string;
  content: string;
  right?: boolean;
}

export interface UserExamListDto extends EntityDto<string> {
  examId?: string;
  examName?: string;
  examStatus: number;
  totalScore: number;
  finishedTime?: string;
  creationTime?: string;
  isPassed?: boolean;
  status: number;
}
