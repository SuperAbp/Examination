import { RestService, Rest } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class OptionService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  getQuestionTypes = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Record<number, string>>({
      method: 'GET',
      url: '/api/options/question-types',
    },
    { apiName: this.apiName,...config });
}