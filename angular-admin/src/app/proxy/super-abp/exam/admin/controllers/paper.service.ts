import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type {
  GetPaperForEditorOutput,
  GetPapersInput,
  PaperCreateDto,
  PaperListDto,
  PaperUpdateDto
} from '../paper-management/papers/models';

@Injectable({
  providedIn: 'root'
})
export class PaperService {
  apiName = 'Default';

  create = (input: PaperCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PaperListDto>(
      {
        method: 'POST',
        url: '/api/paper',
        body: input
      },
      { apiName: this.apiName, ...config }
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/paper/${id}`
      },
      { apiName: this.apiName, ...config }
    );

  getEditor = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetPaperForEditorOutput>(
      {
        method: 'GET',
        url: `/api/paper/${id}/editor`
      },
      { apiName: this.apiName, ...config }
    );

  getList = (input: GetPapersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PaperListDto>>(
      {
        method: 'GET',
        url: '/api/paper',
        params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount, name: input.name }
      },
      { apiName: this.apiName, ...config }
    );

  update = (id: string, input: PaperUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PaperListDto>(
      {
        method: 'PUT',
        url: `/api/paper/${id}`,
        body: input
      },
      { apiName: this.apiName, ...config }
    );

  constructor(private restService: RestService) {}
}
