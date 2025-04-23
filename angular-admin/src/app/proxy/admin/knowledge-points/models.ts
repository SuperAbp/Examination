
export interface GetKnowledgePointForEditorOutput {
  parentId?: string;
  name?: string;
}

export interface GetKnowledgePointsInput {
  name?: string;
}

export interface KnowledgePointCreateDto extends GetKnowledgePointForEditorOutput {
}

export interface KnowledgePointListDto {
  id?: string;
  parentId?: string;
  name?: string;
}

export interface KnowledgePointUpdateDto extends GetKnowledgePointForEditorOutput {
}
