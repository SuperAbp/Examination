// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

import { DelonMockModule } from '@delon/mock';
import { Environment } from 'src/Environment';

import * as MOCKDATA from '../../_mock';

const baseUrl = 'http://admin.exam.lzez.com.cn/';
export const environment = {
  application: {
    baseUrl,
    name: '123',
    logoUrl: ''
  },
  oAuthConfig: {
    issuer: '',
    redirectUri: baseUrl
    //   clientId: 'CertificationAuthority_App',
    //   responseType: 'code',
    //   scope: 'offline_access CertificationAuthority',
  },
  apis: {
    default: {
      url: 'http://api.admin.exam.lzez.com.cn',
      rootNamespace: 'Lzez.Exam'
    }
  },
  resource: {
    mediaUrl: 'http://api.admin.exam.lzez.com.cn/api/super-abp/media',
    userUrl: 'https://passport.lzez.com.cn',
    erpUrl: 'https://erp.ahsanle.cn/'
  },
  identity: {
    url: 'https://passport.lzez.com.cn',
    loginCallback: 'OX/BPTEMAKcxz5zEC2B57sK7fCkwlYN+PkJOn8xaEVfOuM8NFw/bDtPebYHB9r9sGVOZZ9tF5RY=',
    logoutCallback: 'OX/BPTEMAKcxz5zEC2B57sK7fCkwlYN+mONLLComz8I='
  },
  api: {
    baseUrl: './',
    refreshTokenEnabled: true,
    refreshTokenType: 'auth-refresh'
  },
  production: true,
  useHash: true,
  modules: [DelonMockModule.forRoot({ data: MOCKDATA })]
} as Environment;

/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
