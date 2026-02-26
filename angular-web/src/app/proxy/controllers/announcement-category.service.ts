import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { AnnouncementCategoryDto } from '../announcements/models';

@Injectable({
  providedIn: 'root',
})
export class AnnouncementCategoryService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AnnouncementCategoryDto>({
      method: 'GET',
      url: `/api/announcement-categories/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<AnnouncementCategoryDto>>({
      method: 'GET',
      url: '/api/announcement-categories',
    },
    { apiName: this.apiName,...config });
}