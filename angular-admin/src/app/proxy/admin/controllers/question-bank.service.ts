import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GetQuestionBankForEditorOutput, GetQuestionBanksInput, QuestionBankCountDto, QuestionBankCreateDto, QuestionBankDetailDto, QuestionBankListDto, QuestionBankUpdateDto } from '../question-management/question-banks/models';

@Injectable({
  providedIn: 'root',
})
export class QuestionBankService {
  apiName = 'Default';
  

  create = (input: QuestionBankCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionBankListDto>({
      method: 'POST',
      url: '/api/question-management/question-bank',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/question-management/question-bank/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionBankDetailDto>({
      method: 'GET',
      url: `/api/question-management/question-bank/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getEditor = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetQuestionBankForEditorOutput>({
      method: 'GET',
      url: `/api/question-management/question-bank/${id}/editor`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetQuestionBanksInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<QuestionBankListDto>>({
      method: 'GET',
      url: '/api/question-management/question-bank',
      params: { title: input.title, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getQuestionCount = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionBankCountDto>({
      method: 'GET',
      url: `/api/question-management/question-bank/${id}/question-count`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: QuestionBankUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionBankListDto>({
      method: 'PUT',
      url: `/api/question-management/question-bank/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
