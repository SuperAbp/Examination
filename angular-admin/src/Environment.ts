import { Environment as abpEnv } from '@abp/ng.core';
import { Environment as delonEnv } from '@delon/theme';

export interface Environment extends delonEnv, abpEnv {
  resource: {
    mediaUrl;
  };
}
