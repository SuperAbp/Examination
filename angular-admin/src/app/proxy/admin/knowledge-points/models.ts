
export interface GetKnowledgePointForEditorOutput {
  parentId?: string;
  name?: string;
}

export interface GetKnowledgePointsInput {
  name?: string;
}

export interface KnowledgePointCreateDto extends GetKnowledgePointForEditorOutput {
}

export interface KnowledgePointNodeDto {
  id?: string;
  name?: string;
  children: KnowledgePointNodeDto[];
}

export interface KnowledgePointUpdateDto extends GetKnowledgePointForEditorOutput {
}
