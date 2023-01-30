import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GetQuestionForEditorOutput, GetQuestionsInput, QuestionCreateDto, QuestionListDto, QuestionUpdateDto } from '../question-management/questions/models';

@Injectable({
  providedIn: 'root',
})
export class QuestionService {
  apiName = 'Default';
  

  create = (input: QuestionCreateDto) =>
    this.restService.request<any, QuestionListDto>({
      method: 'POST',
      url: '/api/question-management/question',
      body: input,
    },
    { apiName: this.apiName });
  

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/question-management/question/${id}`,
    },
    { apiName: this.apiName });
  

  getEditor = (id: string) =>
    this.restService.request<any, GetQuestionForEditorOutput>({
      method: 'GET',
      url: `/api/question-management/question/${id}/editor`,
    },
    { apiName: this.apiName });
  

  getList = (input: GetQuestionsInput) =>
    this.restService.request<any, PagedResultDto<QuestionListDto>>({
      method: 'GET',
      url: '/api/question-management/question',
      params: { content: input.content, questionType: input.questionType, questionRepositoryIds: input.questionRepositoryIds, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });
  

  update = (id: string, input: QuestionUpdateDto) =>
    this.restService.request<any, QuestionListDto>({
      method: 'PUT',
      url: `/api/question-management/question/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
