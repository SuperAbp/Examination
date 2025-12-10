import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { GetQuestionsInput, QuestionDetailDto, QuestionListDto } from '../question-management/questions/models';

@Injectable({
  providedIn: 'root',
})
export class QuestionService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionDetailDto>({
      method: 'GET',
      url: `/api/questions/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getIds = (input: GetQuestionsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<string>>({
      method: 'GET',
      url: '/api/questions/ids',
      params: { content: input.content, questionType: input.questionType, isFavorite: input.isFavorite, questionId: input.questionId, questionBankId: input.questionBankId },
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetQuestionsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<QuestionListDto>>({
      method: 'GET',
      url: '/api/questions',
      params: { content: input.content, questionType: input.questionType, isFavorite: input.isFavorite, questionId: input.questionId, questionBankId: input.questionBankId },
    },
    { apiName: this.apiName,...config });
}