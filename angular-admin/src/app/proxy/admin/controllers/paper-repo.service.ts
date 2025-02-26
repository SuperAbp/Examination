import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GetPaperRepoForEditorOutput, GetPaperReposInput, PaperRepoListDto } from '../paper-management/paper-repos/models';

@Injectable({
  providedIn: 'root',
})
export class PaperRepoService {
  apiName = 'Default';
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/paper-repository/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getEditor = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetPaperRepoForEditorOutput>({
      method: 'GET',
      url: `/api/paper-repository/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetPaperReposInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PaperRepoListDto>>({
      method: 'GET',
      url: '/api/paper-repository',
      params: { paperId: input.paperId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
