import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GetQuestionForEditorOutput, GetQuestionsInput, QuestionCreateDto, QuestionImportDto, QuestionListDto, QuestionUpdateDto } from '../question-management/questions/models';

@Injectable({
  providedIn: 'root',
})
export class QuestionService {
  apiName = 'Default';
  

  create = (input: QuestionCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionListDto>({
      method: 'POST',
      url: '/api/question-management/question',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/question-management/question/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getEditor = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetQuestionForEditorOutput>({
      method: 'GET',
      url: `/api/question-management/question/${id}/editor`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetQuestionsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<QuestionListDto>>({
      method: 'GET',
      url: '/api/question-management/question',
      params: { content: input.content, questionType: input.questionType, questionBankIds: input.questionBankIds, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  import = (input: QuestionImportDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/question-management/question/import',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: QuestionUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionListDto>({
      method: 'PUT',
      url: `/api/question-management/question/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
