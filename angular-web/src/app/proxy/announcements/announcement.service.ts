import type { AnnouncementDetailDto, AnnouncementListDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AnnouncementService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AnnouncementDetailDto>({
      method: 'GET',
      url: `/api/exam/announcements/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getEffectiveList = (categoryId?: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<AnnouncementListDto>>({
      method: 'GET',
      url: '/api/exam/announcements/effective',
      params: { categoryId },
    },
    { apiName: this.apiName,...config });
}