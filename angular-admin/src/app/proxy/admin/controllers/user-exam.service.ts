import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { GetUserExamsInput, ReviewedQuestionDto, UserExamDetailDto, UserExamListDto } from '../exam-management/user-exams/models';

@Injectable({
  providedIn: 'root',
})
export class UserExamService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserExamDetailDto>({
      method: 'GET',
      url: `/api/user-exam/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetUserExamsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<UserExamListDto>>({
      method: 'GET',
      url: '/api/user-exam',
      params: { examId: input.examId, userId: input.userId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  reviewQuestions = (id: string, input: ReviewedQuestionDto[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PATCH',
      url: '/api/user-exam/review',
      params: { id },
      body: input,
    },
    { apiName: this.apiName,...config });
}