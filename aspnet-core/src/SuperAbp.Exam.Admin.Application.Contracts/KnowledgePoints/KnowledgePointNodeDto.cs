using System;
using System.Collections.Generic;

namespace SuperAbp.Exam.Admin.KnowledgePoints;

public class KnowledgePointNodeDto
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public List<KnowledgePointNodeDto> Children { get; set; }
}