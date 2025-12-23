import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44398/',
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
      url: 'https://localhost:44398',
      rootNamespace: 'SuperAbp.Exam',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
  remoteEnv: {
    url: '/getEnvConfig',
    mergeStrategy: 'deepmerge'
  }
} as Environment;
