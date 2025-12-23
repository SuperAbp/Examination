import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { GetMistakeReviewsInput, MistakeReviewListDto } from '../mistake-reviews/models';

@Injectable({
  providedIn: 'root',
})
export class MistakeReviewService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  getList = (input: GetMistakeReviewsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<MistakeReviewListDto>>({
      method: 'GET',
      url: '/api/mistake-reviews',
      params: { questionType: input.questionType, questionContent: input.questionContent, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
}