import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { GetMistakesInput, MistakeListDto } from '../mistakes/models';

@Injectable({
  providedIn: 'root',
})
export class MistakeService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  getList = (input: GetMistakesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<MistakeListDto>>({
      method: 'GET',
      url: '/api/mistakes',
      params: { questionType: input.questionType, questionContent: input.questionContent, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
}