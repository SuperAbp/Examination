import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { GetKnowledgePointForEditorOutput, GetKnowledgePointsInput, KnowledgePointCreateDto, KnowledgePointNodeDto, KnowledgePointUpdateDto } from '../knowledge-points/models';

@Injectable({
  providedIn: 'root',
})
export class KnowledgePointService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  create = (input: KnowledgePointCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'POST',
      responseType: 'text',
      url: '/api/knowledge-point',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/knowledge-point/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getAll = (input: GetKnowledgePointsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<KnowledgePointNodeDto>>({
      method: 'GET',
      url: '/api/knowledge-point',
      params: { name: input.name },
    },
    { apiName: this.apiName,...config });
  

  getEditor = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetKnowledgePointForEditorOutput>({
      method: 'GET',
      url: `/api/knowledge-point/${id}/editor`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: KnowledgePointUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/knowledge-point/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
}