import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ExamingCreateDto, ExamingDetailDto, ExamingListDto, ExamingUpdateDto, GetExamingForEditorOutput, GetExamingsInput } from '../exam-management/exams/models';

@Injectable({
  providedIn: 'root',
})
export class ExamingService {
  apiName = 'Default';
  

  create = (input: ExamingCreateDto) =>
    this.restService.request<any, ExamingListDto>({
      method: 'POST',
      url: '/api/examing',
      body: input,
    },
    { apiName: this.apiName });
  

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/examing/${id}`,
    },
    { apiName: this.apiName });
  

  get = (id: string) =>
    this.restService.request<any, ExamingDetailDto>({
      method: 'GET',
      url: `/api/examing/${id}`,
    },
    { apiName: this.apiName });
  

  getEditor = (id: string) =>
    this.restService.request<any, GetExamingForEditorOutput>({
      method: 'GET',
      url: `/api/examing/${id}/editor`,
    },
    { apiName: this.apiName });
  

  getList = (input: GetExamingsInput) =>
    this.restService.request<any, PagedResultDto<ExamingListDto>>({
      method: 'GET',
      url: '/api/examing',
      params: { name: input.name, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });
  

  update = (id: string, input: ExamingUpdateDto) =>
    this.restService.request<any, ExamingListDto>({
      method: 'PUT',
      url: `/api/examing/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
