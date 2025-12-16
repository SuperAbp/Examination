import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { GetUserExamsInput, UserExamAnswerDto, UserExamCreateDto, UserExamDetailDto, UserExamListDto } from '../exam-management/user-exams/models';

@Injectable({
  providedIn: 'root',
})
export class UserExamService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  answer = (id: string, input: UserExamAnswerDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PATCH',
      url: `/api/user-exams/${id}/answer`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  create = (input: UserExamCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserExamListDto>({
      method: 'POST',
      url: '/api/user-exams',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  finished = (id: string, input: UserExamAnswerDto[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PATCH',
      url: `/api/user-exams/${id}/finished`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserExamDetailDto>({
      method: 'GET',
      url: `/api/user-exams/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetUserExamsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<UserExamListDto>>({
      method: 'GET',
      url: '/api/user-exams',
      params: { userExamId: input.userExamId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getUnfinished = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserExamDetailDto>({
      method: 'GET',
      url: '/api/user-exams/unfinished',
    },
    { apiName: this.apiName,...config });
  

  start = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PATCH',
      url: `/api/user-exams/${id}/start`,
    },
    { apiName: this.apiName,...config });
}