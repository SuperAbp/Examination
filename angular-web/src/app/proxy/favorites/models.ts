import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface FavoriteListDto {
  id?: string;
  questionId?: string;
  questionContent: string;
  questionType: number;
  creationTime?: string;
}

export interface GetFavoritesInput extends PagedAndSortedResultRequestDto {
  questionType?: number;
  questionContent?: string;
}
