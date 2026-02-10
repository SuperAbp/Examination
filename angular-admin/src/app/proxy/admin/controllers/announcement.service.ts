import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { AnnouncementCreateDto, AnnouncementDetailDto, AnnouncementListDto, AnnouncementUpdateDto, GetAnnouncementsInput } from '../announcements/models';

@Injectable({
  providedIn: 'root',
})
export class AnnouncementService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  create = (input: AnnouncementCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AnnouncementDetailDto>({
      method: 'POST',
      url: '/api/announcements',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/announcements/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AnnouncementDetailDto>({
      method: 'GET',
      url: `/api/announcements/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetAnnouncementsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<AnnouncementListDto>>({
      method: 'GET',
      url: '/api/announcements',
      params: { title: input.title, categoryId: input.categoryId, isPublished: input.isPublished, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  publish = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PATCH',
      url: `/api/announcements/${id}/publish`,
    },
    { apiName: this.apiName,...config });
  

  unpublish = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PATCH',
      url: `/api/announcements/${id}/unpublish`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: AnnouncementUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AnnouncementDetailDto>({
      method: 'PUT',
      url: `/api/announcements/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
}