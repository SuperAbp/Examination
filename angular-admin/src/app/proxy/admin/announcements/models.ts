import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface AnnouncementCategoryCreateDto extends AnnouncementCategoryCreateOrUpdateDtoBase {}

export interface AnnouncementCategoryCreateOrUpdateDtoBase {
  name?: string;
  sort: number;
  remark?: string;
}

export interface AnnouncementCategoryDetailDto extends FullAuditedEntityDto<string> {
  name?: string;
  sort: number;
  remark?: string;
}

export interface AnnouncementCategoryListDto extends FullAuditedEntityDto<string> {
  name?: string;
  sort: number;
  remark?: string;
}

export interface AnnouncementCategoryUpdateDto extends AnnouncementCategoryCreateOrUpdateDtoBase {}

export interface AnnouncementCreateDto extends AnnouncementCreateOrUpdateDtoBase {}

export interface AnnouncementCreateOrUpdateDtoBase {
  title?: string;
  content?: string;
  scheduledPublishTime?: string;
  scheduledExpirationTime?: string;
  sort: number;
  categoryId?: string;
}

export interface AnnouncementDetailDto extends FullAuditedEntityDto<string> {
  title?: string;
  content?: string;
  scheduledPublishTime?: string;
  scheduledExpirationTime?: string;
  isPublished: boolean;
  sort: number;
  categoryId?: string;
  categoryName?: string;
}

export interface AnnouncementListDto extends FullAuditedEntityDto<string> {
  title?: string;
  content?: string;
  scheduledPublishTime?: string;
  scheduledExpirationTime?: string;
  isPublished: boolean;
  sort: number;
  categoryId?: string;
  categoryName?: string;
}

export interface AnnouncementUpdateDto extends AnnouncementCreateOrUpdateDtoBase {}

export interface GetAnnouncementsInput extends PagedAndSortedResultRequestDto {
  title?: string;
  categoryId?: string;
  isPublished?: boolean;
}
