import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetPaperForEditorOutput extends PaperCreateOrUpdateDtoBase {
}

export interface GetPapersInput extends PagedAndSortedResultRequestDto {
  name?: string;
}

export interface PaperCreateDto extends PaperCreateOrUpdateDtoBase {
}

export interface PaperCreateOrUpdateDtoBase {
  name?: string;
  description?: string;
  score: number;
}

export interface PaperListDto extends EntityDto<string> {
  name?: string;
  score: number;
}

export interface PaperUpdateDto extends PaperCreateOrUpdateDtoBase {
}
