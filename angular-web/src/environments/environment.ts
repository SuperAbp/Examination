import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4201';

const oAuthConfig = {
  issuer: 'https://localhost:44386/',
  redirectUri: baseUrl,
  clientId: 'Exam_App',
  responseType: 'code',
  scope: 'offline_access Exam',
  requireHttps: true,
};

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'Exam',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44389',
      rootNamespace: 'SuperAbp.Exam',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
} as Environment;
