// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

import { provideMockConfig, mockInterceptor } from '@delon/mock';
import { Environment } from 'src/Environment';

import * as MOCKDATA from '../../_mock';

const baseUrl = 'http://121.41.239.155:8084/';
export const environment = {
  application: {
    baseUrl,
    name: '考乐试',
    logoUrl: ''
  },
  oAuthConfig: {
    issuer: 'http://121.41.239.155:8082/',
    redirectUri: baseUrl,
    clientId: 'Exam_Admin_App',
    responseType: 'code',
    scope: 'offline_access Exam',
    requireHttps: false
  },
  apis: {
    default: {
      url: 'http://121.41.239.155:8080',
      rootNamespace: 'SuperAbp.Exam'
    }
  },
  resource: {
    mediaUrl: 'http://121.41.239.155:8080/api/super-abp/media',
    userUrl: 'http://121.41.239.155:8082'
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
