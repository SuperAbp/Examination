import { Environment } from '@abp/ng.core';

const baseUrl = 'https://examimg.superabp.com';

const oAuthConfig = {
  issuer: 'https://auth-examimg.superabp.com/',
  redirectUri: baseUrl,
  clientId: 'Exam_App',
  responseType: 'code',
  scope: 'offline_access Exam',
  requireHttps: true,
};

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'Exam',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://api-examimg.superabp.com',
      rootNamespace: 'SuperAbp.Exam',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
  remoteEnv: {
    url: '/getEnvConfig',
    mergeStrategy: 'deepmerge',
  },
} as Environment;
