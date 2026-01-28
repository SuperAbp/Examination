import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { GetQuestionCountInput } from '../question-management/question-banks/models';
import type { GetQuestionForEditorOutput, GetQuestionWithDetailInput, GetQuestionsInput, QuestionCreateDto, QuestionDetailDto, QuestionImportDto, QuestionListDto, QuestionUpdateDto } from '../question-management/questions/models';

@Injectable({
  providedIn: 'root',
})
export class QuestionService {
  private restService = inject(RestService);
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
  

  deleteAnswer = (id: string, answerId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/question-management/question/${id}/answer/${answerId}`,
    },
    { apiName: this.apiName,...config });
  

  getCount = (input: GetQuestionCountInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, number>({
      method: 'GET',
      url: '/api/question-management/question/count',
      params: { questionBankId: input.questionBankId, questionType: input.questionType, knowledgePointId: input.knowledgePointId },
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
      params: { content: input.content, questionType: input.questionType, questionBankIds: input.questionBankIds, knowledgePointId: input.knowledgePointId, excludeIds: input.excludeIds, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getListWithDetail = (input: GetQuestionWithDetailInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionDetailDto[]>({
      method: 'GET',
      url: '/api/question-management/question/details',
      params: { questionBankId: input.questionBankId, questionType: input.questionType, includeIds: input.includeIds, excludeIds: input.excludeIds, count: input.count },
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
}