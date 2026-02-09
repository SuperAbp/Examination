import type { EntityDto } from '@abp/ng.core';

export interface AnnouncementCategoryDto {
  id?: string;
  name?: string;
  sort: number;
  remark?: string;
  creationTime?: string;
}

export interface AnnouncementDetailDto extends EntityDto<string> {
  title?: string;
  content?: string;
  categoryId?: string;
  categoryName?: string;
}

export interface AnnouncementListDto extends EntityDto<string> {
  title?: string;
  briefContent?: string;
  categoryId?: string;
  categoryName?: string;
}
