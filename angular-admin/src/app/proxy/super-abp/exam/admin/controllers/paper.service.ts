import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GetPaperForEditorOutput, GetPapersInput, PaperCreateDto, PaperListDto, PaperUpdateDto } from '../paper-management/papers/models';

@Injectable({
  providedIn: 'root',
})
export class PaperService {
  apiName = 'Default';
  

  create = (input: PaperCreateDto) =>
    this.restService.request<any, PaperListDto>({
      method: 'POST',
      url: '/api/paper',
      body: input,
    },
    { apiName: this.apiName });
  

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/paper/${id}`,
    },
    { apiName: this.apiName });
  

  getEditor = (id: string) =>
    this.restService.request<any, GetPaperForEditorOutput>({
      method: 'GET',
      url: `/api/paper/${id}/editor`,
    },
    { apiName: this.apiName });
  

  getList = (input: GetPapersInput) =>
    this.restService.request<any, PagedResultDto<PaperListDto>>({
      method: 'GET',
      url: '/api/paper',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });
  

  update = (id: string, input: PaperUpdateDto) =>
    this.restService.request<any, PaperListDto>({
      method: 'PUT',
      url: `/api/paper/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
