// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

import { provideMockConfig, mockInterceptor } from '@delon/mock';
import { Environment } from 'src/Environment';

import * as MOCKDATA from '../../_mock';

const baseUrl = 'http://localhost:4200';
export const environment = {
  application: {
    baseUrl,
    name: '123',
    logoUrl: ''
  },
  oAuthConfig: {
    issuer: 'https://localhost:44386/',
    redirectUri: baseUrl,
    clientId: 'Exam_Admin_App',
    responseType: 'code',
    scope: 'offline_access Exam',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44388',
      rootNamespace: 'Lzez.Exam'
    }
  },
  resource: {
    mediaUrl: 'https://localhost:44388/api/super-abp/media',
    userUrl: 'https://passport.lzez.com.cn',
    erpUrl: 'https://erp.ahsanle.cn/'
  },
  identity: {
    url: 'https://passport.lzez.com.cn',
    loginCallback: 'ejaLfSbcqezvj9WGUAxoCzq+GvfAAiXWu/38eLB9fsWP2rA/H6eh4b2Ugp1sUF6v',
    logoutCallback: 'ejaLfSbcqezvj9WGUAxoC8aQim04tniO'
  },
  api: {
    baseUrl: './',
    refreshTokenEnabled: true,
    refreshTokenType: 'auth-refresh'
  },
  production: false,
  useHash: true,
  providers: [provideMockConfig({ data: MOCKDATA })],
  interceptorFns: [mockInterceptor],
  modules: []
} as Environment;

/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
