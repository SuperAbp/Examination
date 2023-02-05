import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ExamingRepoCreateDto, ExamingRepoListDto, ExamingRepoUpdateDto, GetExamingRepoForEditorOutput, GetExamingReposInput } from '../exam-management/exam-repos/models';

@Injectable({
  providedIn: 'root',
})
export class ExamingRepoService {
  apiName = 'Default';
  

  create = (input: ExamingRepoCreateDto) =>
    this.restService.request<any, ExamingRepoListDto>({
      method: 'POST',
      url: '/api/examing-repository',
      body: input,
    },
    { apiName: this.apiName });
  

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/examing-repository/${id}`,
    },
    { apiName: this.apiName });
  

  getEditor = (id: string) =>
    this.restService.request<any, GetExamingRepoForEditorOutput>({
      method: 'GET',
      url: `/api/examing-repository/${id}`,
    },
    { apiName: this.apiName });
  

  getList = (input: GetExamingReposInput) =>
    this.restService.request<any, PagedResultDto<ExamingRepoListDto>>({
      method: 'GET',
      url: '/api/examing-repository',
      params: { examingId: input.examingId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });
  

  update = (id: string, input: ExamingRepoUpdateDto) =>
    this.restService.request<any, ExamingRepoListDto>({
      method: 'PATCH',
      url: `/api/examing-repository/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
