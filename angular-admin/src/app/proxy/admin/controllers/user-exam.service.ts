import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GetUserExamWithUsersInput, GetUserExamsInput, ReviewedQuestionDto, UserExamDetailDto, UserExamListDto, UserExamWithUserDto } from '../exam-management/user-exams/models';

@Injectable({
  providedIn: 'root',
})
export class UserExamService {
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
  

  getListWithUser = (input: GetUserExamWithUsersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<UserExamWithUserDto>>({
      method: 'GET',
      url: '/api/user-exam/user',
      params: { examId: input.examId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
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

  constructor(private restService: RestService) {}
}
