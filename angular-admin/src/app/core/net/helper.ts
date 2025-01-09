import { LocalizationParam } from '@abp/ng.core';
import { HttpErrorResponse, HttpHeaders, HttpResponseBase } from '@angular/common/http';
import { Injector, inject } from '@angular/core';
import { Router } from '@angular/router';
import { DA_SERVICE_TOKEN } from '@delon/auth';
import { ALAIN_I18N_TOKEN } from '@delon/theme';
import { NzNotificationService } from 'ng-zorro-antd/notification';

export interface ReThrowHttpError {
  body: any;
  _throw: true;
}

export const CODEMESSAGE: { [key: number]: string } = {
  200: '服务器成功返回请求的数据。',
  201: '新建或修改数据成功。',
  202: '一个请求已经进入后台排队（异步任务）。',
  204: '删除数据成功。',
  400: '发出的请求有错误，服务器没有进行新建或修改数据的操作。',
  401: '用户没有权限（令牌、用户名、密码错误）。',
  403: '用户得到授权，但是访问是被禁止的。',
  404: '发出的请求针对的是不存在的记录，服务器没有进行操作。',
  406: '请求的格式不可得。',
  410: '请求的资源被永久删除，且不会再得到的。',
  422: '当创建一个对象时，发生一个验证错误。',
  500: '服务器发生错误，请检查服务器。',
  502: '网关错误。',
  503: '服务不可用，服务器暂时过载或维护。',
  504: '网关超时。'
};
export const DEFAULT_ERROR_MESSAGES = {
  defaultError: {
    title: 'An error has occurred!',
    details: 'Error detail not sent by server.'
  },
  defaultError401: {
    title: 'You are not authenticated!',
    details: 'You should be authenticated (sign in) in order to perform this operation.'
  },
  defaultError403: {
    title: 'You are not authorized!',
    details: 'You are not allowed to perform this operation.'
  },
  defaultError404: {
    title: 'Resource not found!',
    details: 'The resource requested could not found on the server.'
  },
  defaultError500: {
    title: 'Internal server error',
    details: 'Error detail not sent by server.'
  }
};

export const DEFAULT_ERROR_LOCALIZATIONS = {
  defaultError: {
    title: 'AbpUi::DefaultErrorMessage',
    details: 'AbpUi::DefaultErrorMessageDetail'
  },
  defaultError401: {
    title: 'AbpUi::DefaultErrorMessage401',
    details: 'AbpUi::DefaultErrorMessage401Detail'
  },
  defaultError403: {
    title: 'AbpUi::DefaultErrorMessage403',
    details: 'AbpUi::DefaultErrorMessage403Detail'
  },
  defaultError404: {
    title: 'AbpUi::DefaultErrorMessage404',
    details: 'AbpUi::DefaultErrorMessage404Detail'
  },
  defaultError500: {
    title: 'AbpUi::500Message',
    details: 'AbpUi::DefaultErrorMessage'
  }
};

export function goTo(injector: Injector, url: string): void {
  setTimeout(() => injector.get(Router).navigateByUrl(url));
}

export function toLogin(injector: Injector): void {
  injector.get(NzNotificationService).error(`未登录或登录已过期，请重新登录。`, ``);
  goTo(injector, injector.get(DA_SERVICE_TOKEN).login_url!);
}

export function getAdditionalHeaders(headers?: HttpHeaders): { [name: string]: string } {
  const res: { [name: string]: string } = {};
  const lang = inject(ALAIN_I18N_TOKEN).currentLang;
  if (!headers?.has('Accept-Language') && lang) {
    res['Accept-Language'] = lang;
  }

  return res;
}

export function checkStatus(injector: Injector, ev: HttpResponseBase): void {
  if ((ev.status >= 200 && ev.status < 300) || ev.status === 401) {
    return;
  }
  if (ev instanceof HttpErrorResponse && ev.headers.get('_AbpErrorFormat')) {
    const { message, title } = getErrorFromRequestBody(ev?.error?.error);
    injector.get(NzNotificationService).error(title.toString(), message.toString());
    return;
  }
  const errortext = CODEMESSAGE[ev.status] || ev.statusText;
  injector.get(NzNotificationService).error(`请求错误 ${ev.status}: ${ev.url}`, errortext);
}

export function getErrorFromRequestBody(body: { details?: string; message?: string } | undefined) {
  let message: LocalizationParam;
  let title: LocalizationParam;

  if (body.details) {
    message = body.details;
    title = body.message;
  } else if (body.message) {
    title = {
      key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
      defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title
    };
    message = body.message;
  } else {
    message = {
      key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
      defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title
    };
    title = '';
  }

  return { message, title };
}
