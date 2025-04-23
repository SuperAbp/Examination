import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GetKnowledgePointForEditorOutput, GetKnowledgePointsInput, KnowledgePointCreateDto, KnowledgePointListDto, KnowledgePointUpdateDto } from '../knowledge-points/models';

@Injectable({
  providedIn: 'root',
})
export class KnowledgePointService {
  apiName = 'Default';
  

  create = (input: KnowledgePointCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'POST',
      responseType: 'text',
      url: '/knowledge-point',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/knowledge-point/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getAll = (input: GetKnowledgePointsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<KnowledgePointListDto>>({
      method: 'GET',
      url: '/knowledge-point',
      params: { name: input.name },
    },
    { apiName: this.apiName,...config });
  

  getEditor = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetKnowledgePointForEditorOutput>({
      method: 'GET',
      url: `/knowledge-point/${id}/editor`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: KnowledgePointUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/knowledge-point/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
