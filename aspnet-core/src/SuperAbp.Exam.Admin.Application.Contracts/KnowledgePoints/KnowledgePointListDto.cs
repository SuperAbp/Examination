using System;

namespace SuperAbp.Exam.Admin.KnowledgePoints;

public class KnowledgePointListDto
{
    public Guid Id { get; set; }
    public string? ParentId { get; set; }

    public required string Name { get; set; }
}