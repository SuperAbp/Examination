import { LocalizationParam, LocalizationService } from '@abp/ng.core';
import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse,
  HttpResponseBase
} from '@angular/common/http';
import { Inject, Injectable, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { _HttpClient } from '@delon/theme';
import { CookieService } from '@delon/util';
import { environment } from '@env/environment';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { Observable, of, throwError } from 'rxjs';
import { catchError, mergeMap } from 'rxjs/operators';
const CODEMESSAGE = {
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
/**
 * 默认HTTP拦截器，其注册细节见 `app.module.ts`
 */
@Injectable()
export class DefaultInterceptor implements HttpInterceptor {
  constructor(private injector: Injector) {}

  private get notification(): NzNotificationService {
    return this.injector.get(NzNotificationService);
  }
  private get location(): LocalizationService {
    return this.injector.get(LocalizationService);
  }

  private goTo(url: string) {
    setTimeout(() => this.injector.get(Router).navigateByUrl(url));
  }

  private checkStatus(ev: HttpResponseBase) {
    if ((ev.status >= 200 && ev.status < 300) || ev.status === 401) {
      return;
    }

    const errortext = CODEMESSAGE[ev.status] || ev.statusText;
    this.notification.error(`请求错误 ${ev.status}: ${ev.url}`, errortext);
  }

  private handleData(ev: HttpResponseBase): Observable<any> {
    // 可能会因为 `throw` 导出无法执行 `_HttpClient` 的 `end()` 操作
    if (ev.status > 0) {
      this.injector.get(_HttpClient).cleanLoading();
    }
    const body = ev?.['error']?.['error'] || {
      key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
      defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title
    };
    if (ev instanceof HttpErrorResponse && ev.headers.get('_AbpErrorFormat')) {
      const confirmation$ = this.showErrorWithRequestBody(body);

      if (ev.status === 401) {
        confirmation$.onClose.subscribe(() => {
          (this.injector.get(DA_SERVICE_TOKEN) as ITokenService).clear();
          this.goTo('/passport/login');
        });
      }
    } else {
      // this.checkStatus(ev);
      // if (ev.status >= 0 && ev.status < 400) {
      //   return of(ev);
      // }
      // 业务处理：一些通用操作
      switch (ev.status) {
        case 200:
          // 业务层级错误处理，以下是假定restful有一套统一输出格式（指不管成功与否都有相应的数据格式）情况下进行处理
          // 例如响应内容：
          //  错误内容：{ status: 1, msg: '非法参数' }
          //  正确内容：{ status: 0, response: {  } }
          // 则以下代码片断可直接适用
          // if (event instanceof HttpResponse) {
          //     const body: any = event.body;
          //     if (body && body.status !== 0) {
          //         this.msg.error(body.msg);
          //         // 继续抛出错误中断后续所有 Pipe、subscribe 操作，因此：
          //         // this.http.get('/').subscribe() 并不会触发
          //         return throwError({});
          //     } else {
          //         // 重新修改 `body` 内容为 `response` 内容，对于绝大多数场景已经无须再关心业务状态码
          //         return of(new HttpResponse(Object.assign(event, { body: body.response })));
          //         // 或者依然保持完整的格式
          //         return of(event);
          //     }
          // }
          break;
        case 401:
          // 清空 token 信息
          if (ev.url.indexOf('application-configuration') <= 0) {
            var tokenService = this.injector.get(DA_SERVICE_TOKEN) as ITokenService;
            var cookieService = this.injector.get(CookieService);
            cookieService.remove('.AspNetCore.Cookies');
            tokenService.clear();
            this.goTo('/passport/login');
          }
          break;
        case 403:
          this.showError(
            {
              key: 'AbpAccount::DefaultErrorMessage403',
              defaultValue: DEFAULT_ERROR_MESSAGES.defaultError403.title
            },
            {
              key: 'AbpAccount::DefaultErrorMessage403Detail',
              defaultValue: DEFAULT_ERROR_MESSAGES.defaultError403.details
            }
          );
          break;
        case 404:
          this.showError(
            {
              key: 'AbpAccount::DefaultErrorMessage404',
              defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.title
            },
            {
              key: 'AbpAccount::DefaultErrorMessage404Detail',
              defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.details
            }
          );
          break;
        case 500:
          this.goTo(`/exception/${ev.status}`);
          break;
        default:
          if (ev instanceof HttpErrorResponse) {
            this.showError(DEFAULT_ERROR_MESSAGES.defaultError.title, DEFAULT_ERROR_MESSAGES.defaultError.details);
            console.warn('未可知错误，大部分是由于后端不支持CORS或无效配置引起', ev);
          }
          break;
      }
    }
    if (ev instanceof HttpErrorResponse) {
      return throwError(() => ev);
    } else {
      return of(ev);
    }
  }

  protected showErrorWithRequestBody(body: any) {
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
      message = body.message || {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title
      };
      title = '';
    }

    return this.showError(title, message);
  }

  protected showError(message: LocalizationParam, title: LocalizationParam) {
    return this.notification.error(this.location.instant(title), this.location.instant(message));
  }
  // /**
  //  * 展示错误
  //  */
  // showError(title?: string, message?: string, body?: any) {
  //   if (body) {
  //     if (body.details) {
  //       message = body.details;
  //       title = body.message;
  //     } else if (body.message) {
  //       title = DEFAULT_ERROR_MESSAGES.defaultError.title;
  //       message = body.message;
  //     } else {
  //       message = body.message || DEFAULT_ERROR_MESSAGES.defaultError.title;
  //     }
  //   }
  //   return this.notification.error(title, message);
  // }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const tokenService = this.injector.get(DA_SERVICE_TOKEN) as ITokenService;
    // 统一加上服务端前缀
    let url = req.url;
    if (
      (tokenService.get().token === '' || tokenService.get().token === undefined) &&
      (url.indexOf('application-configuration') > 0 || url.indexOf('app/data') > 0)
    ) {
      url = `${url}?_allow_anonymous=true`;
    }
    if (!url.startsWith('https://') && !url.startsWith('http://')) {
      url = environment.apis.default.url + url;
    }

    const newReq = req.clone({ url });
    return next.handle(newReq).pipe(
      mergeMap((event: any) => {
        // 允许统一对请求错误处理
        if (event instanceof HttpResponseBase) {
          return this.handleData(event);
        }
        // 若一切都正常，则后续操作
        return of(event);
      }),
      catchError((err: HttpErrorResponse) => this.handleData(err))
    );
  }
}
