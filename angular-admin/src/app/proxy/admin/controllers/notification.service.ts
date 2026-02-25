import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { GetMyNotificationsInput, GetNotificationsInput, NotificationListDto, NotificationMyListDto } from '../notifications/models';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  getList = (input: GetNotificationsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<NotificationListDto>>({
      method: 'GET',
      url: '/api/notifications',
      params: { receiverId: input.receiverId, type: input.type, isRead: input.isRead, channel: input.channel, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getMyList = (input: GetMyNotificationsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<NotificationMyListDto>>({
      method: 'GET',
      url: '/api/notifications/my',
      params: { isRead: input.isRead, filter: input.filter, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getUnreadCount = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, number>({
      method: 'GET',
      url: '/api/notifications/unread-count',
    },
    { apiName: this.apiName,...config });
  

  markAllAsRead = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/notifications/mark-all-as-read',
    },
    { apiName: this.apiName,...config });
  

  markAsRead = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/notifications/${id}/mark-as-read`,
    },
    { apiName: this.apiName,...config });
}