// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

import { provideMockConfig, mockInterceptor } from '@delon/mock';
import { Environment } from 'src/Environment';

import * as MOCKDATA from '../../_mock';

const baseUrl = 'http://admin.examimg.superabp.com/';
export const environment = {
  application: {
    baseUrl,
    name: '考乐试',
    logoUrl: ''
  },
  oAuthConfig: {
    issuer: 'http://auth.examimg.superabp.com/',
    redirectUri: baseUrl,
    clientId: 'Exam_Admin_App',
    responseType: 'code',
    scope: 'offline_access Exam',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'http://api.admin.examimg.superabp.com',
      rootNamespace: 'SuperAbp.Exam'
    }
  },
  resource: {
    mediaUrl: 'http://api.admin.examimg.superabp.com/api/super-abp/media',
    userUrl: 'https://auth.examimg.superabp.com'
  },
  api: {
    baseUrl: './',
    refreshTokenEnabled: true,
    refreshTokenType: 'auth-refresh'
  },
  production: true,
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
