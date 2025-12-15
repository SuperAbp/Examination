import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { GetMistakesReviewInput, MistakesReviewListDto } from '../mistakes-reviews/models';

@Injectable({
  providedIn: 'root',
})
export class MistakesReviewService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  getList = (input: GetMistakesReviewInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<MistakesReviewListDto>>({
      method: 'GET',
      url: '/api/mistakes-reviews',
      params: { questionType: input.questionType, questionContent: input.questionContent, errorCount: input.errorCount, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
}