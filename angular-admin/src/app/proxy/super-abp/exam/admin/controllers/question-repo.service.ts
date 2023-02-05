import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type {
  GetQuestionRepoForEditorOutput,
  GetQuestionReposInput,
  QuestionRepoCountDto,
  QuestionRepoCreateDto,
  QuestionRepoDetailDto,
  QuestionRepoListDto,
  QuestionRepoUpdateDto
} from '../question-management/question-repos/models';

@Injectable({
  providedIn: 'root'
})
export class QuestionRepoService {
  apiName = 'Default';

  create = (input: QuestionRepoCreateDto) =>
    this.restService.request<any, QuestionRepoListDto>(
      {
        method: 'POST',
        url: '/api/question-management/question-repository',
        body: input
      },
      { apiName: this.apiName }
    );

  delete = (id: string) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/question-management/question-repository/${id}`
      },
      { apiName: this.apiName }
    );

  get = (id: string) =>
    this.restService.request<any, QuestionRepoDetailDto>(
      {
        method: 'GET',
        url: `/api/question-management/question-repository/${id}`
      },
      { apiName: this.apiName }
    );

  getQuestionCount = (id: string) =>
    this.restService.request<any, QuestionRepoCountDto>(
      {
        method: 'GET',
        url: `/api/question-management/question-repository/${id}/question-count`
      },
      { apiName: this.apiName }
    );

  getEditor = (id: string) =>
    this.restService.request<any, GetQuestionRepoForEditorOutput>(
      {
        method: 'GET',
        url: `/api/question-management/question-repository/${id}/editor`
      },
      { apiName: this.apiName }
    );

  getList = (input: GetQuestionReposInput) =>
    this.restService.request<any, PagedResultDto<QuestionRepoListDto>>(
      {
        method: 'GET',
        url: '/api/question-management/question-repository',
        params: { title: input.title, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount }
      },
      { apiName: this.apiName }
    );

  update = (id: string, input: QuestionRepoUpdateDto) =>
    this.restService.request<any, QuestionRepoListDto>(
      {
        method: 'PUT',
        url: `/api/question-management/question-repository/${id}`,
        body: input
      },
      { apiName: this.apiName }
    );

  constructor(private restService: RestService) {}
}
