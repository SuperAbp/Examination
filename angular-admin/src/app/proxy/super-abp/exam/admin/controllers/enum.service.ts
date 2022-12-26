import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EnumService {
  apiName = 'Default';
  

  getQuestionType = () =>
    this.restService.request<any, Record<number, string>>({
      method: 'GET',
      url: '/api/enum/question-type',
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
