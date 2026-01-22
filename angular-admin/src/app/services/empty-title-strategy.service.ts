import { AbpTitleStrategy } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { TitleStrategy } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class EmptyTitleStrategy extends AbpTitleStrategy {
  override updateTitle(): void {}
}
