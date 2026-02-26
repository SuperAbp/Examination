import type { EntityDto } from '@abp/ng.core';

export interface NotificationListDto extends EntityDto<string> {
  type: number;
  data?: string;
  isRead: boolean;
  readTime?: string;
  relatedEntityId?: string;
  relatedEntityType?: string;
  creationTime?: string;
}
