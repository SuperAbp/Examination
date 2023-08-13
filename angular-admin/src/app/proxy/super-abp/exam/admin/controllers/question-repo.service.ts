import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GetQuestionRepoForEditorOutput, GetQuestionReposInput, QuestionRepoCountDto, QuestionRepoCreateDto, QuestionRepoDetailDto, QuestionRepoListDto, QuestionRepoUpdateDto } from '../question-management/question-repos/models';

@Injectable({
  providedIn: 'root',
})
export class QuestionRepoService {
  apiName = 'Default';
  

  create = (input: QuestionRepoCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionRepoListDto>({
      method: 'POST',
      url: '/api/question-management/question-repository',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/question-management/question-repository/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionRepoDetailDto>({
      method: 'GET',
      url: `/api/question-management/question-repository/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getEditor = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetQuestionRepoForEditorOutput>({
      method: 'GET',
      url: `/api/question-management/question-repository/${id}/editor`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetQuestionReposInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<QuestionRepoListDto>>({
      method: 'GET',
      url: '/api/question-management/question-repository',
      params: { title: input.title, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getQuestionCount = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionRepoCountDto>({
      method: 'GET',
      url: `/api/question-management/question-repository/${id}/question-count`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: QuestionRepoUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionRepoListDto>({
      method: 'PUT',
      url: `/api/question-management/question-repository/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
