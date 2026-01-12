import type { ExtensibleFullAuditedEntityDto } from '@abp/ng.core';

export interface IdentityUserDto extends ExtensibleFullAuditedEntityDto<string> {
  tenantId?: string;
  userName?: string;
  name?: string;
  surname?: string;
  email?: string;
  emailConfirmed: boolean;
  phoneNumber?: string;
  phoneNumberConfirmed: boolean;
  isActive: boolean;
  lockoutEnabled: boolean;
  accessFailedCount: number;
  lockoutEnd?: string;
  concurrencyStamp?: string;
  entityVersion: number;
  lastPasswordChangeTime?: string;
}
