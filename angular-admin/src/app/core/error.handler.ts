import { HttpErrorReporterService } from '@abp/ng.core';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { filter } from 'rxjs/operators';

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

@Injectable({ providedIn: 'root' })
export class NgAbpErrorHandler extends ErrorHandler {
  constructor(protected httpErrorReporter: HttpErrorReporterService, private router: Router) {
    super();
    this.listenToRestError();
  }

  protected listenToRestError() {
    this.httpErrorReporter.reporter$
      .pipe(
        filter(this.filterRestErrors),
        tap(res => {
          if (!(res instanceof HttpErrorResponse)) {
            var url = `exception/${res['status']}`;
            this.router.navigateByUrl(url);
          }
        })
      )
      .subscribe();
  }

  protected filterRestErrors = ({ status }: HttpErrorResponse): boolean => {
    this.httpErrorReporter.errors;
    if (typeof status !== 'number') return false;

    const skipHandledErrorCodes = [401, 403, 404, 500];
    return !!skipHandledErrorCodes && skipHandledErrorCodes.findIndex(code => code === status) > 0;
  };
}
