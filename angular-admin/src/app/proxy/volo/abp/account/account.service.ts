import type { RegisterDto, ResetPasswordDto, SendPasswordResetCodeDto, VerifyPasswordResetTokenInput } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { IdentityUserDto } from '../identity/models';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private restService = inject(RestService);
  apiName = 'AbpAccount';
  

  register = (input: RegisterDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityUserDto>({
      method: 'POST',
      url: '/api/account/register',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  resetPassword = (input: ResetPasswordDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/account/reset-password',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  sendPasswordResetCode = (input: SendPasswordResetCodeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/account/send-password-reset-code',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  verifyPasswordResetToken = (input: VerifyPasswordResetTokenInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'POST',
      url: '/api/account/verify-password-reset-token',
      body: input,
    },
    { apiName: this.apiName,...config });
}