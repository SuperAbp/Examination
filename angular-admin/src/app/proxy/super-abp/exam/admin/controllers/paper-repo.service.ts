import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GetPaperRepoForEditorOutput, GetPaperReposInput, PaperRepoCreateDto, PaperRepoListDto, PaperRepoUpdateDto } from '../paper-management/paper-repos/models';

@Injectable({
  providedIn: 'root',
})
export class PaperRepoService {
  apiName = 'Default';
  

  create = (input: PaperRepoCreateDto) =>
    this.restService.request<any, PaperRepoListDto>({
      method: 'POST',
      url: '/api/paper-repository',
      body: input,
    },
    { apiName: this.apiName });
  

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/paper-repository/${id}`,
    },
    { apiName: this.apiName });
  

  getEditor = (id: string) =>
    this.restService.request<any, GetPaperRepoForEditorOutput>({
      method: 'GET',
      url: `/api/paper-repository/${id}`,
    },
    { apiName: this.apiName });
  

  getList = (input: GetPaperReposInput) =>
    this.restService.request<any, PagedResultDto<PaperRepoListDto>>({
      method: 'GET',
      url: '/api/paper-repository',
      params: { examingId: input.examingId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });
  

  update = (id: string, input: PaperRepoUpdateDto) =>
    this.restService.request<any, PaperRepoListDto>({
      method: 'PATCH',
      url: `/api/paper-repository/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
