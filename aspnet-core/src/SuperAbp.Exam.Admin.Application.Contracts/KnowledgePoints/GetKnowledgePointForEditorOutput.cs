using System;

namespace SuperAbp.Exam.Admin.KnowledgePoints;

public class GetKnowledgePointForEditorOutput
{
    public Guid? ParentId { get; set; }

    public required string Name { get; set; }
}