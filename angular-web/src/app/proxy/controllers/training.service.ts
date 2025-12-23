import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { GetTrainsInput, TrainingCreateDto, TrainingListDto } from '../training-management/models';

@Injectable({
  providedIn: 'root',
})
export class TrainingService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  create = (input: TrainingCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TrainingListDto>({
      method: 'POST',
      url: '/api/training',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetTrainsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<TrainingListDto>>({
      method: 'GET',
      url: '/api/training',
      params: { questionBankId: input.questionBankId, trainingSource: input.trainingSource },
    },
    { apiName: this.apiName,...config });
  

  setIsRight = (id: string, right: boolean, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PATCH',
      url: '/api/training',
      params: { id, right },
    },
    { apiName: this.apiName,...config });
}