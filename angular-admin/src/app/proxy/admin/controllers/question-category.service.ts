import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GetQuestionCategoriesInput, GetQuestionCategoryForEditorOutput, QuestionCategoryCreateDto, QuestionCategoryListDto, QuestionCategoryUpdateDto } from '../question-management/question-categories/models';

@Injectable({
  providedIn: 'root',
})
export class QuestionCategoryService {
  apiName = 'Default';
  

  create = (input: QuestionCategoryCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'POST',
      responseType: 'text',
      url: '/api/app/question-category',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getEditor = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetQuestionCategoryForEditorOutput>({
      method: 'GET',
      url: `/${id}/editor`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetQuestionCategoriesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<QuestionCategoryListDto>>({
      method: 'GET',
      url: '/api/app/question-category',
      params: { parentId: input.parentId, name: input.name, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: QuestionCategoryUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
