import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GetQuestionAnswerForEditorOutput, GetQuestionAnswersInput, QuestionAnswerCreateDto, QuestionAnswerListDto, QuestionAnswerUpdateDto } from '../question-management/question-answers/models';

@Injectable({
  providedIn: 'root',
})
export class QuestionAnswerService {
  apiName = 'Default';
  

  create = (input: QuestionAnswerCreateDto) =>
    this.restService.request<any, QuestionAnswerListDto>({
      method: 'POST',
      url: '/api/question-management/question-answer',
      body: input,
    },
    { apiName: this.apiName });
  

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/question-management/question-answer/${id}`,
    },
    { apiName: this.apiName });
  

  getEditor = (id: string) =>
    this.restService.request<any, GetQuestionAnswerForEditorOutput>({
      method: 'GET',
      url: `/api/question-management/question-answer/${id}/editor`,
    },
    { apiName: this.apiName });
  

  getList = (input: GetQuestionAnswersInput) =>
    this.restService.request<any, PagedResultDto<QuestionAnswerListDto>>({
      method: 'GET',
      url: '/api/question-management/question-answer',
      params: { questionId: input.questionId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });
  

  update = (id: string, input: QuestionAnswerUpdateDto) =>
    this.restService.request<any, QuestionAnswerListDto>({
      method: 'PUT',
      url: `/api/question-management/question-answer/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
