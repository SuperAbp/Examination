import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GetQuestionAnswerForEditorOutput, GetQuestionAnswersInput, QuestionAnswerCreateDto, QuestionAnswerListDto, QuestionAnswerUpdateDto } from '../question-management/question-answers/models';

@Injectable({
  providedIn: 'root',
})
export class QuestionAnswerService {
  apiName = 'Default';
  

  create = (input: QuestionAnswerCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionAnswerListDto>({
      method: 'POST',
      url: '/api/question-management/question-answer',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/question-management/question-answer/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getEditor = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetQuestionAnswerForEditorOutput>({
      method: 'GET',
      url: `/api/question-management/question-answer/${id}/editor`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetQuestionAnswersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<QuestionAnswerListDto>>({
      method: 'GET',
      url: '/api/question-management/question-answer',
      params: { questionId: input.questionId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: QuestionAnswerUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionAnswerListDto>({
      method: 'PUT',
      url: `/api/question-management/question-answer/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
