import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { GetQuestionBanksInput, QuestionBankDetailDto, QuestionBankListDto } from '../question-management/question-banks/models';

@Injectable({
  providedIn: 'root',
})
export class QuestionBankService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionBankDetailDto>({
      method: 'GET',
      url: `/api/question-banks/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetQuestionBanksInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<QuestionBankListDto>>({
      method: 'GET',
      url: '/api/question-banks',
      params: { title: input.title, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getQuestionTypes = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<number>>({
      method: 'GET',
      url: `/api/question-banks/${id}/question-types`,
    },
    { apiName: this.apiName,...config });
}