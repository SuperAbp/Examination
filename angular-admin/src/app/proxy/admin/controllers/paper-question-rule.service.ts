import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GetPaperQuestionRuleForEditorOutput, GetPaperQuestionRulesInput, PaperQuestionRuleListDto } from '../paper-management/paper-question-rules/models';

@Injectable({
  providedIn: 'root',
})
export class PaperQuestionRuleService {
  apiName = 'Default';
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/paper-question-rule/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getEditor = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetPaperQuestionRuleForEditorOutput>({
      method: 'GET',
      url: `/api/paper-question-rule/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetPaperQuestionRulesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PaperQuestionRuleListDto>>({
      method: 'GET',
      url: '/api/paper-question-rule',
      params: { paperId: input.paperId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
