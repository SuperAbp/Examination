import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { FavoriteListDto, GetFavoritesInput } from '../favorites/models';

@Injectable({
  providedIn: 'root',
})
export class FavoriteService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  create = (questionId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/favorites',
      params: { questionId },
    },
    { apiName: this.apiName,...config });
  

  delete = (questionId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/favorites',
      params: { questionId },
    },
    { apiName: this.apiName,...config });
  

  getByQuestionId = (questionId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'GET',
      url: `/api/favorites/question/${questionId}`,
    },
    { apiName: this.apiName,...config });
  

  getCount = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, number>({
      method: 'GET',
      url: '/api/favorites/count',
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetFavoritesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<FavoriteListDto>>({
      method: 'GET',
      url: '/api/favorites',
      params: { questionType: input.questionType, questionContent: input.questionContent, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
}