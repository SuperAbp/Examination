import { RestService, Rest } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DynamicClaimsService {
  private restService = inject(RestService);
  apiName = 'AbpAccount';
  

  refresh = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/account/dynamic-claims/refresh',
    },
    { apiName: this.apiName,...config });
}