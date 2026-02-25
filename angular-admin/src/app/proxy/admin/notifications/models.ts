import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetMyNotificationsInput extends PagedAndSortedResultRequestDto {
  isRead?: boolean;
  filter?: string;
}

export interface GetNotificationsInput extends PagedAndSortedResultRequestDto {
  receiverId?: string;
  type?: number;
  isRead?: boolean;
  channel?: number;
}

export interface NotificationListDto extends EntityDto<string> {
  receiverId?: string;
  type: number;
  isRead: boolean;
}

export interface NotificationMyListDto extends EntityDto<string> {
  type: number;
  data?: string;
  isRead: boolean;
  readTime?: string;
  relatedEntityId?: string;
  relatedEntityType?: string;
  creationTime?: string;
}
