import { HttpClient, HttpHandlerFn, HttpRequest, HttpResponseBase } from '@angular/common/http';
import { APP_INITIALIZER, Injector, Provider } from '@angular/core';
import { DA_SERVICE_TOKEN } from '@delon/auth';
import { BehaviorSubject, Observable, catchError, filter, from, switchMap, take, throwError } from 'rxjs';
import { OAuthService, TokenResponse } from 'angular-oauth2-oidc';

import { toLogin } from './helper';

let refreshToking = false;
let refreshToken$: BehaviorSubject<any> = new BehaviorSubject<any>(null);

/**
 * 重新附加新 Token 信息
 *
 * > 由于已经发起的请求，不会再走一遍 `@delon/auth` 因此需要结合业务情况重新附加新的 Token
 */
function reAttachToken(injector: Injector, req: HttpRequest<any>): HttpRequest<any> {
  const token = injector.get(DA_SERVICE_TOKEN).get()?.token;
  return req.clone({
    setHeaders: {
      token: `Bearer ${token}`
    }
  });
}

function refreshTokenRequest(injector: Injector): Observable<TokenResponse> {
  const oAuthService = injector.get(OAuthService);
  return from(oAuthService.refreshToken());
}

/**
 * 刷新Token方式一：使用 401 重新刷新 Token
 */
export function tryRefreshToken(injector: Injector, ev: HttpResponseBase, req: HttpRequest<any>, next: HttpHandlerFn): Observable<any> {
  // 1、如果 `refreshToking` 为 `true` 表示已经在请求刷新 Token 中，后续所有请求转入等待状态，直至结果返回后再重新发起请求
  if (refreshToking) {
    return refreshToken$.pipe(
      filter(v => !!v),
      take(1),
      switchMap(() => next(reAttachToken(injector, req)))
    );
  }
  // 2、尝试调用刷新 Token
  refreshToking = true;
  refreshToken$.next(null);

  return refreshTokenRequest(injector).pipe(
    switchMap(res => {
      // 通知后续请求继续执行
      refreshToking = false;
      refreshToken$.next(res);
      // 重新保存新 token
      injector.get(DA_SERVICE_TOKEN).set({
        token: res.access_token,
        expired: res.expires_in
      });
      // 重新发起请求
      return next(reAttachToken(injector, req));
    }),
    catchError(err => {
      refreshToking = false;
      toLogin(injector);
      return throwError(() => err);
    })
  );
}

function buildAuthRefresh(injector: Injector) {
  const tokenSrv = injector.get(DA_SERVICE_TOKEN);
  tokenSrv.refresh
    .pipe(
      filter(() => !refreshToking),
      switchMap(res => {
        refreshToking = true;
        return refreshTokenRequest(injector);
      })
    )
    .subscribe({
      next: res => {
        refreshToking = false;
        tokenSrv.set({
          token: res.access_token,
          expired: res.expires_in
        });
      },
      error: () => toLogin(injector)
    });
}

/**
 * 刷新Token方式二：使用 `@delon/auth` 的 `refresh` 接口，需要在 `app.config.ts` 中注册 `provideBindAuthRefresh`
 */
export function provideBindAuthRefresh(): Provider[] {
  return [
    {
      provide: APP_INITIALIZER,
      useFactory: (injector: Injector) => () => buildAuthRefresh(injector),
      deps: [Injector],
      multi: true
    }
  ];
}
