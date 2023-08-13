import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ExamCreateDto, ExamDetailDto, ExamListDto, ExamUpdateDto, GetExamForEditorOutput, GetExamsInput } from '../exam-management/exams/models';

@Injectable({
  providedIn: 'root',
})
export class ExaminationService {
  apiName = 'Default';
  

  create = (input: ExamCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExamListDto>({
      method: 'POST',
      url: '/api/exam',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/exam/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExamDetailDto>({
      method: 'GET',
      url: `/api/exam/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getEditor = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetExamForEditorOutput>({
      method: 'GET',
      url: `/api/exam/${id}/editor`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetExamsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ExamListDto>>({
      method: 'GET',
      url: '/api/exam',
      params: { name: input.name, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: ExamUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExamListDto>({
      method: 'PUT',
      url: `/api/exam/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
