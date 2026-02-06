import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { AnnouncementCategoryCreateDto, AnnouncementCategoryDetailDto, AnnouncementCategoryListDto, AnnouncementCategoryUpdateDto } from '../announcements/models';

@Injectable({
  providedIn: 'root',
})
export class AnnouncementCategoryService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  create = (input: AnnouncementCategoryCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AnnouncementCategoryDetailDto>({
      method: 'POST',
      url: '/api/announcement-categories',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/announcement-categories/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AnnouncementCategoryDetailDto>({
      method: 'GET',
      url: `/api/announcement-categories/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<AnnouncementCategoryListDto>>({
      method: 'GET',
      url: '/api/announcement-categories',
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: AnnouncementCategoryUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AnnouncementCategoryDetailDto>({
      method: 'PUT',
      url: `/api/announcement-categories/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
}