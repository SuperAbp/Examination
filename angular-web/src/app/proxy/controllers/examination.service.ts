import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { ExamDetailDto, ExamListDto, ExamRankingDto, GetExamsInput } from '../exam-management/exams/models';

@Injectable({
  providedIn: 'root',
})
export class ExaminationService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExamDetailDto>({
      method: 'GET',
      url: `/api/exams/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetExamsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ExamListDto>>({
      method: 'GET',
      url: '/api/exams',
      params: { name: input.name, status: input.status, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getRankingList = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<ExamRankingDto>>({
      method: 'GET',
      url: `/api/exams/${id}/ranking`,
    },
    { apiName: this.apiName,...config });
}