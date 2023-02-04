import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type {
  ExamingRepoCreateDto,
  ExamingRepoCreateOrUpdateDtoBase,
  ExamingRepoListDto,
  ExamingRepoUpdateDto,
  GetExamingRepoForEditorOutput,
  GetExamingReposInput
} from '../exam-management/exam-repos/models';

@Injectable({
  providedIn: 'root'
})
export class ExamingRepoService {
  apiName = 'Default';

  createOrUpdate = (input: ExamingRepoCreateOrUpdateDtoBase) =>
    this.restService.request<any, ExamingRepoListDto>(
      {
        method: 'POST',
        url: '/api/examing-repository',
        body: input
      },
      { apiName: this.apiName }
    );

  delete = (examingId: string, questionRepositoryId: string) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/examing-repository/${examingId}/repository/${questionRepositoryId}`
      },
      { apiName: this.apiName }
    );

  getEditor = (examingId: string, questionRepositoryId: string) =>
    this.restService.request<any, GetExamingRepoForEditorOutput>(
      {
        method: 'GET',
        url: `/api/examing-repository/${examingId}/repository/${questionRepositoryId}/editor`
      },
      { apiName: this.apiName }
    );

  getList = (input: GetExamingReposInput) =>
    this.restService.request<any, PagedResultDto<ExamingRepoListDto>>(
      {
        method: 'GET',
        url: '/api/examing-repository',
        params: { examingId: input.examingId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount }
      },
      { apiName: this.apiName }
    );

  update = (examingId: string, questionRepositoryId: string, input: ExamingRepoUpdateDto) =>
    this.restService.request<any, ExamingRepoListDto>(
      {
        method: 'PUT',
        url: `/api/examing-repository/${examingId}/repository/${questionRepositoryId}`,
        body: input
      },
      { apiName: this.apiName }
    );

  constructor(private restService: RestService) {}
}
